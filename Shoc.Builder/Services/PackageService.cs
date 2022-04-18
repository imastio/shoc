using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using Imast.Ext.Core;
using Shoc.ApiCore;
using Shoc.Builder.Data;
using Shoc.Builder.Model;
using Shoc.Builder.Model.Package;
using Shoc.Builder.Model.Project;
using Shoc.Builder.Model.Registry;
using Shoc.Core;
using Shoc.Identity.Model;
using Shoc.ModelCore;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Shoc.Builder.Services
{
    /// <summary>
    /// The package service implementation
    /// </summary>
    public class PackageService
    {
        /// <summary>
        /// The package upload progress
        /// </summary>
        private const int PACKAGE_UPLOAD_PROGRESS = 25;

        /// <summary>
        /// The package repository
        /// </summary>
        private readonly IPackageRepository packageRepository;

        /// <summary>
        /// The project repository
        /// </summary>
        private readonly IProjectRepository projectRepository;

        /// <summary>
        /// The docker registry repository
        /// </summary>
        private readonly IDockerRegistryRepository dockerRegistryRepository;

        /// <summary>
        /// The builder settings
        /// </summary>
        private readonly BuilderSettings builderSettings;

        /// <summary>
        /// Creates new instance of package service
        /// </summary>
        /// <param name="packageRepository">The package repository</param>
        /// <param name="projectRepository">The project repository</param>
        /// <param name="dockerRegistryRepository">The registry repository</param>
        /// <param name="builderSettings">The builder settings</param>
        public PackageService(IPackageRepository packageRepository, IProjectRepository projectRepository, IDockerRegistryRepository dockerRegistryRepository, BuilderSettings builderSettings)
        {
            this.packageRepository = packageRepository;
            this.projectRepository = projectRepository;
            this.dockerRegistryRepository = dockerRegistryRepository;
            this.builderSettings = builderSettings;
        }

        /// <summary>
        /// Gets the packages by given filter
        /// </summary>
        /// <param name="principal">The authenticated principal</param>
        /// <param name="query">The package query</param>
        /// <returns></returns>
        public async Task<IEnumerable<ShocPackage>> GetBy(ShocPrincipal principal, PackageQuery query)
        {
            // gets the project by id with valid access
            var _ = await this.RequireProject(principal, query.ProjectId);

            // get the packages with given query
            return await this.packageRepository.GetBy(query);
        }

        /// <summary>
        /// Gets the package by the given id
        /// </summary>
        /// <param name="principal">The authenticated principals</param>
        /// <param name="projectId">The project id</param>
        /// <param name="id">The package id</param>
        /// <returns></returns>
        public async Task<ShocPackage> GetById(ShocPrincipal principal, string projectId, string id)
        {
            // gets the project by id with valid access
            var _ = await this.RequireProject(principal, projectId);

            // try get by id
            var result = await this.packageRepository.GetById(id);

            // check if package is there
            if (result == null)
            {
                throw ErrorDefinition.NotFound().AsException();
            }

            return result;
        }

        /// <summary>
        /// Creates new package with given input
        /// </summary>
        /// <param name="principal">The authenticated principal</param>
        /// <param name="projectId">The project id</param>
        /// <param name="id">The id of package</param>
        /// <param name="stream">The stream to upload</param>
        /// <returns></returns>
        public async Task<PackageBundleReference> UploadBundle(ShocPrincipal principal, string projectId, string id, Stream stream)
        {
            // get package with access check
            var package = await this.GetById(principal, projectId, id);

            // update package status
            package = await this.packageRepository.UpdateStatus(new PackageStatusModel
            {
                Id = package.Id,
                Status = PackageStatuses.UPLOADING,
                Progress = 0,
                ProgressMessage = "Uploading the bundle for the package"
            });
            
            // get a temporary file
            var temp = Path.GetTempFileName();

            // create writer to temp file
            await using (var writer = new FileStream(temp, FileMode.OpenOrCreate, FileAccess.Write))
            {
                // copy stream to writer
                await stream.CopyToAsync(writer);
            }

            // create a folder for the bundle
            var bundleRoot = Path.Combine(builderSettings.SandboxRoot, package.Id, Guid.NewGuid().ToString("N"));
            
            // unzip files to the bundle root
            await Task.Run(() => ZipFile.ExtractToDirectory(temp, bundleRoot));

            // delete temporary zip file
            File.Delete(temp);

            // create bundle in the database
            var bundle = await this.packageRepository.CreateBundle(new PackageBundleModel
            {
                PackageId = package.Id,
                BundleRoot = bundleRoot
            });

            // update status of the package
            _ = await this.packageRepository.UpdateStatus(new PackageStatusModel
            {
                Id = package.Id,
                Status = PackageStatuses.UPLOADED,
                Progress = PACKAGE_UPLOAD_PROGRESS,
                ProgressMessage = "The package bundle is updated"
            });

            // return package
            return new PackageBundleReference
            {
                Id = bundle.Id
            };
        }

        /// <summary>
        /// Creates new package with given input
        /// </summary>
        /// <param name="principal">The authenticated principal</param>
        /// <param name="projectId">The project id</param>
        /// <param name="input">The package input</param>
        /// <returns></returns>
        public async Task<ShocPackage> Create(ShocPrincipal principal, string projectId, CreatePackageInput input)
        {
            // gets the project with access 
            var _ = await this.RequireProject(principal, projectId);

            // make sure default parameters are set
            input.ProjectId = projectId;
            input.Status = PackageStatuses.INIT;
            input.Progress = 0;
            input.ProgressMessage = string.Empty;

            // prepare yaml deserializer
            var deserializer = new DeserializerBuilder()
                .IgnoreUnmatchedProperties()
                .WithNamingConvention(HyphenatedNamingConvention.Instance)
                .Build();

            // deserialize build spec
            var spec = deserializer.Deserialize<BuildSpec>(input.BuildSpec);

            // resolves the registry based on the spec
            var registry = await this.ResolveRegistry(principal, spec);

            // assign registry 
            input.RegistryId = registry.Id;

            // create the package
            return await this.packageRepository.Create(input);
        }
        
        /// <summary>
        /// Deletes the package from the project
        /// </summary>
        /// <param name="principal">The authenticated principal</param>
        /// <param name="projectId">The project id</param>
        /// <param name="id">The id of package to delete</param>
        /// <returns></returns>
        public async Task<ShocPackage> DeleteById(ShocPrincipal principal, string projectId, string id)
        {
            // gets the project by id with valid access
            var _ = await this.RequireProject(principal, projectId);

            // try delete by id
            var result = await this.packageRepository.DeleteById(id);

            // check if package is there
            if (result == null)
            {
                throw ErrorDefinition.NotFound().AsException();
            }

            return result;
        }

        /// <summary>
        /// Require the project by id
        /// </summary>
        /// <param name="principal">The authenticated principal</param>
        /// <param name="projectId">The id of project</param>
        /// <returns></returns>
        private async Task<ProjectModel> RequireProject(ShocPrincipal principal, string projectId)
        {
            // try load the result
            var result = await this.projectRepository.GetById(projectId);

            // not found
            if (result == null)
            {
                throw ErrorDefinition.NotFound().AsException();
            }

            // require to be either administrator or owner
            AccessGuard.Require(() => Roles.ADMINS.Contains(principal.Role) || result.OwnerId == principal.Subject);

            // the result
            return result;
        }

        /// <summary>
        /// Resolves the registry based on the build specification
        /// </summary>
        /// <param name="principal">The authenticated principal</param>
        /// <param name="spec">The specification</param>
        /// <returns></returns>
        private async Task<DockerRegistry> ResolveRegistry(ShocPrincipal principal, BuildSpec spec)
        {
            // name is given
            var nameGiven = spec.RegistryName.IsNotBlank();

            // try get all registries with given name 
            var registries = (await this.dockerRegistryRepository.GetBy(new DockerRegistryQuery
            {
                Name = nameGiven ? spec.RegistryName : null
            })).ToList();

            // in case if name given but not found any with name raise an error
            if (nameGiven && registries.Count == 0)
            {
                throw ErrorDefinition.Validation(BuilderErrors.INVALID_REGISTRY_NAME).AsException();
            }

            // get owned instance with name if any
            var ownedWithName = registries.FirstOrDefault(reg => reg.Name == spec.RegistryName &&  reg.OwnerId == principal.Subject);

            // return owned instance with name
            if (ownedWithName != null)
            {
                return ownedWithName;
            }

            // get named registry that is shared
            var sharedWithName = registries.FirstOrDefault(reg => reg.Name == spec.RegistryName && reg.Shared);

            // return shared instance with matching name
            if (sharedWithName != null)
            {
                return sharedWithName;
            }

            // try get any shared without considering the name
            var anyShared = registries.FirstOrDefault(c => c.Shared);

            // no registry available
            if (anyShared == null)
            {
                throw ErrorDefinition.Validation(BuilderErrors.NO_REGISTRY).AsException();
            }

            return anyShared;
        }
    }
}