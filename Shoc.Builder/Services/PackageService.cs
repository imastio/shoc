using System.Collections.Generic;
using System.Threading.Tasks;
using Shoc.ApiCore;
using Shoc.Builder.Data;
using Shoc.Builder.Model.Package;
using Shoc.Builder.Model.Project;
using Shoc.Core;
using Shoc.Identity.Model;
using Shoc.ModelCore;

namespace Shoc.Builder.Services
{
    /// <summary>
    /// The package service implementation
    /// </summary>
    public class PackageService
    {
        /// <summary>
        /// The package repository
        /// </summary>
        private readonly IPackageRepository packageRepository;

        /// <summary>
        /// The project repository
        /// </summary>
        private readonly IProjectRepository projectRepository;

        /// <summary>
        /// Creates new instance of package service
        /// </summary>
        /// <param name="packageRepository">The package repository</param>
        /// <param name="projectRepository">The project repository</param>
        public PackageService(IPackageRepository packageRepository, IProjectRepository projectRepository)
        {
            this.packageRepository = packageRepository;
            this.projectRepository = projectRepository;
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
        /// <param name="input">The package input</param>
        /// <returns></returns>
        public async Task<ShocPackage> Create(ShocPrincipal principal, string projectId, CreatePackageInput input)
        {
            // gets the project with access 
            var _ = await this.RequireProject(principal, projectId);

            // make sure default parameters are set
            input.ProjectId = projectId;
            input.Status = PackageStatuses.INIT;
            

            throw new System.NotImplementedException();
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
    }
}