using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.Tar;
using Imast.Ext.Core;
using Microsoft.AspNetCore.DataProtection;
using Shoc.ApiCore;
using Shoc.Builder.Data;
using Shoc.Builder.Model;
using Shoc.Builder.Model.Package;
using Shoc.Builder.Model.Project;
using Shoc.Builder.Model.Registry;
using Shoc.Builder.Services.Interfaces;
using Shoc.Core;
using Shoc.Engine.Model;
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
        /// The package building progress
        /// </summary>
        private const int PACKAGE_BUILDING_PROGRESS = 50;

        /// <summary>
        /// The package building progress
        /// </summary>
        private const int PACKAGE_BUILT_PROGRESS = 100;

        /// <summary>
        /// The package failed to build progress
        /// </summary>
        private const int PACKAGE_FAILED_PROGRESS = 100;

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
        /// The containerizer provider
        /// </summary>
        private readonly IContainerizeProvider containerizeProvider;

        /// <summary>
        /// The container engine service
        /// </summary>
        private readonly EngineService engineService;

        /// <summary>
        /// The data protection provider 
        /// </summary>
        private readonly IDataProtectionProvider dataProtectionProvider;

        /// <summary>
        /// Creates new instance of package service
        /// </summary>
        /// <param name="packageRepository">The package repository</param>
        /// <param name="projectRepository">The project repository</param>
        /// <param name="dockerRegistryRepository">The registry repository</param>
        /// <param name="builderSettings">The builder settings</param>
        /// <param name="containerizeProvider">The containerizer provider</param>
        /// <param name="engineService">The container engine service</param>
        /// <param name="dataProtectionProvider">The data protection provider</param>
        public PackageService(IPackageRepository packageRepository, IProjectRepository projectRepository,
            IDockerRegistryRepository dockerRegistryRepository, BuilderSettings builderSettings,
            IContainerizeProvider containerizeProvider, EngineService engineService, IDataProtectionProvider dataProtectionProvider)
        {
            this.packageRepository = packageRepository;
            this.projectRepository = projectRepository;
            this.dockerRegistryRepository = dockerRegistryRepository;
            this.builderSettings = builderSettings;
            this.containerizeProvider = containerizeProvider;
            this.engineService = engineService;
            this.dataProtectionProvider = dataProtectionProvider;
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
        /// Build package with id
        /// </summary>
        /// <param name="principal">The authenticated principal</param>
        /// <param name="projectId">The project id</param>
        /// <param name="id">The id of package</param>
        /// <param name="version">The version of package</param>
        /// <returns></returns>
        public async Task<PackageBundleReference> BuildBundle(ShocPrincipal principal, string projectId, string id, string version)
        {
            try
            {
                return await this.BuildBundleExecute(principal, projectId, id, version);
            }
            catch (Exception ex)
            {
                // update package status
                _ = await this.packageRepository.UpdateStatus(new PackageStatusModel
                {
                    Id = id,
                    Status = PackageStatuses.FAILED,
                    Progress = PACKAGE_FAILED_PROGRESS,
                    ProgressMessage = $"Failed to build the package: {ex.Message}"
                });

                throw;
            }
        }

        /// <summary>
        /// Build package with id
        /// </summary>
        /// <param name="principal">The authenticated principal</param>
        /// <param name="projectId">The project id</param>
        /// <param name="id">The id of package</param>
        /// <param name="version">The version of package</param>
        /// <returns></returns>
        public async Task<PackageBundleReference> BuildBundleExecute(ShocPrincipal principal, string projectId, string id, string version)
        {
            // get package with access check
            var project = await this.RequireProject(principal, projectId);

            // get package with access check
            var package = await this.GetById(principal, projectId, id);

            // update package status
            package = await this.packageRepository.UpdateStatus(new PackageStatusModel
            {
                Id = package.Id,
                Status = PackageStatuses.BUILDING,
                Progress = PACKAGE_BUILDING_PROGRESS,
                ProgressMessage = "Building the package"
            });

            // get bundle
            var bundle = await this.packageRepository.GetBundleByPackageId(package.Id);

            // make sure bundle exists
            if (bundle == null)
            {
                throw ErrorDefinition.NotFound().AsException();
            }

            // makre sure directory is present
            if (!Directory.Exists(bundle.BundleRoot))
            {
                ErrorDefinition.Validation().Throw();
            }

            // prepare yaml deserializer
            var deserializer = new DeserializerBuilder()
                .IgnoreUnmatchedProperties()
                .WithNamingConvention(HyphenatedNamingConvention.Instance)
                .Build();

            // deserialize build spec
            var spec = deserializer.Deserialize<BuildSpec>(package.BuildSpec);

            // get containerizer
            var containerizer = this.containerizeProvider.Create(spec.Base);

            // make sure containerizer exists
            if (containerizer == null)
            {
                throw ErrorDefinition.NotFound().AsException();
            }

            // get docker file contents
            var fileContent = containerizer.GetFileContents(spec);

            try
            {
                // write content to the file
                await File.WriteAllTextAsync(Path.Combine(bundle.BundleRoot, "Dockerfile"), fileContent);
            }
            catch
            {
                ErrorDefinition.Validation(BuilderErrors.DOCKERFILE_ERROR).Throw();
            }

            // get registry of the package
            var registry = await this.dockerRegistryRepository.GetById(package.RegistryId);

            // make sure registry exists
            if (registry == null)
            {
                throw ErrorDefinition.NotFound().AsException();
            }

            // create tarball archive from the bundle
            var stream = await this.MakeTarStream(bundle.BundleRoot);

            // create image uri based on nesting
            var imageUri =
                registry.AllowNesting ?
                $"{registry.RegistryUri.Replace("http://", "").Replace("https://", "").TrimEnd('/')}/{registry.Repository.TrimEnd('/')}/{principal.Subject}/{project.Id}"
                : $"{registry.RegistryUri.Replace("http://", "").Replace("https://", "").TrimEnd('/')}/{registry.Repository.TrimEnd('/')}";

            // create the image in dind
            await this.engineService.CreateImage(new ImageBuildInput
            {
                Name = project.Name,
                Version = package.Id,
                ImageUri = imageUri,
                Payload = stream,
            });

            // get protector
            var protector = this.dataProtectionProvider.CreateProtector(BuilderProtection.REGISTRY_CREDENTIALS);

            // push the image from dind to repository
            await this.engineService.PushImage(new ImagePushInput
            {
                Version = package.Id,
                ImageUri = imageUri,
                Username = registry.Username,
                Password = protector.Unprotect(registry.EncryptedPassword)
            });

            // update latest version to newest package
            await this.projectRepository.AssignVersion(projectId, package.Id, "latest");

            // if version is not latest assign also that version
            if (version != null && !string.Equals(version, "latest"))
            {
                await this.projectRepository.AssignVersion(projectId, package.Id, version);
            }

            // update package status
            package = await this.packageRepository.UpdateStatus(new PackageStatusModel
            {
                Id = package.Id,
                Status = PackageStatuses.BUILT,
                Progress = PACKAGE_BUILT_PROGRESS,
                ProgressMessage = "The package has been built"
            });

            // return package
            return new PackageBundleReference
            {
                Id = package.Id
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
            _ = await this.RequireProject(principal, projectId);

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
            var registry = await this.ResolveRegistry(spec);

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
            AccessGuard.Require(() => UserTypes.ESCALATED.Contains(principal.Type) || result.OwnerId == principal.Subject);

            // the result
            return result;
        }

        /// <summary>
        /// Resolves the registry based on the build specification
        /// </summary>
        /// <param name="spec">The specification</param>
        /// <returns></returns>
        private async Task<DockerRegistry> ResolveRegistry(BuildSpec spec)
        {
            // name is given
            var nameGiven = spec.RegistryName.IsNotBlank();

            // try get all registries with given name 
            var registry = (await this.dockerRegistryRepository.GetBy(new DockerRegistryQuery
            {
                Name = nameGiven ? spec.RegistryName : null
            })).FirstOrDefault();

            // in case if name given but not found any with name raise an error
            if (registry == null)
            {
                throw ErrorDefinition.Validation(BuilderErrors.INVALID_REGISTRY_NAME).AsException();
            }

            return registry;
        }

        /// <summary>
        /// Create tar.gz archive from directory
        /// </summary>
        /// <param name="directory">The directory to stream archive</param>
        private async Task<Stream> MakeTarStream(string directory)
        {
            var tarball = new MemoryStream();
            var files = Directory.GetFiles(directory, "*.*", SearchOption.AllDirectories);

            await using var archive = new TarOutputStream(tarball, Encoding.UTF8)
            {
                // prevent the TarOutputStream from closing the underlying memory stream when done
                IsStreamOwner = false
            };

            foreach (var file in files)
            {
                // replacing slashes
                var tarName = file.Substring(directory.Length).Replace('\\', '/').TrimStart('/');

                // create the entry header
                var entry = TarEntry.CreateTarEntry(tarName);

                await using var fileStream = File.OpenRead(file);
                entry.Size = fileStream.Length;
                archive.PutNextEntry(entry);

                // now write the bytes of data
                var localBuffer = new byte[32 * 1024];
                while (true)
                {
                    var numRead = fileStream.Read(localBuffer, 0, localBuffer.Length);

                    if (numRead <= 0)
                    {
                        break;
                    }

                    archive.Write(localBuffer, 0, numRead);
                }

                // nothing more to do with this entry
                archive.CloseEntry();
            }
            archive.Close();

            // reset the stream and return it, so it can be used by the caller
            tarball.Position = 0;

            return tarball;
        }
    }
}