using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shoc.ApiCore;
using Shoc.ApiCore.Protection;
using Shoc.Builder.Model.Package;
using Shoc.Builder.Services;
using Shoc.Identity.Model;

namespace Shoc.Builder.Controllers
{
    /// <summary>
    /// The packages controller
    /// </summary>
    [Route("api/projects/{projectId}/packages")]
    [ApiController]
    [ShocExceptionHandler]
    [AuthorizedSubject]
    public class PackagesController : ControllerBase
    {
        /// <summary>
        /// The projects service
        /// </summary>
        private readonly PackageService packageService;

        /// <summary>
        /// Creates new instance of packages
        /// </summary>
        /// <param name="packageService">The packages service</param>
        public PackagesController(PackageService packageService)
        {
            this.packageService = packageService;
        }
        
        /// <summary>
        /// Gets the package entries for the given project with filters
        /// </summary>
        /// <param name="projectId">The project id</param>
        /// <param name="listingChecksum">The listing checksum</param>
        /// <returns></returns>
        [HttpGet]
        public Task<IEnumerable<ShocPackage>> GetBy(string projectId, [FromQuery] string listingChecksum = null)
        {
            // the request principal
            var principal = this.HttpContext.GetShocPrincipal();

            // get filtered
            return this.packageService.GetBy(principal, new PackageQuery
            {
                ProjectId = projectId,
                ListingChecksum = listingChecksum
            });
        }

        /// <summary>
        /// Gets the package by id
        /// </summary>
        /// <param name="projectId">The project id</param>
        /// <param name="id">The id of package</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public Task<ShocPackage> GetById(string projectId, string id)
        {
            var principal = this.HttpContext.GetShocPrincipal();

            // try get result
            return this.packageService.GetById(principal.Subject, projectId, id);
        }

        /// <summary>
        /// Gets the package by id
        /// </summary>
        /// <param name="projectId">The project id</param>
        /// <param name="id">The id of package</param>
        /// <param name="ownerId">The owner id</param>
        /// <returns></returns>
        [HttpGet("{id}/by-owner/{ownerId}")]
        [AuthorizeMinUserType(UserTypes.ADMIN, AllowInsiders = true)]
        public Task<ShocPackage> GetInternalById(string projectId, string id, string ownerId)
        {
            // try get result
            return this.packageService.GetById(ownerId, projectId, id);
        }

        /// <summary>
        /// Creates a package with the given input
        /// </summary>
        /// <param name="projectId">The project id</param>
        /// <param name="input">The package creation input</param>
        /// <returns></returns>
        [HttpPost]
        public Task<ShocPackage> Create(string projectId, [FromBody] CreatePackageInput input)
        {
            return this.packageService.Create(this.HttpContext.GetShocPrincipal(), projectId, input);
        }   

        /// <summary>
        /// Creates a package with the given input
        /// </summary>
        /// <param name="projectId">The project id</param>
        /// <param name="id">The id of package</param>
        /// <returns></returns>
        [HttpPut("{id}/bundle")]
        [DisableFormValueModelBinding]
        [DisableRequestSizeLimit]
        public async Task<PackageBundleReference> UploadBundle(string projectId, string id)
        {
            // the bundle reference to return
            var bundleReference = default(PackageBundleReference);

            // stream request files
            await this.Request.StreamFiles(async file => 
            {
                // open stream to read
                await using var stream = file.OpenReadStream();

                // upload the bundle 
                bundleReference = await this.packageService.UploadBundle(this.HttpContext.GetShocPrincipal(), projectId, id, stream);
            });

            return bundleReference;
        }

        /// <summary>
        /// Build the package with given id
        /// </summary>
        /// <param name="projectId">The project id</param>
        /// <param name="id">The id of package</param>
        /// <param name="version">The version of package</param>
        /// <returns></returns>
        [HttpPost("{id}/build/{version}")]
        public async Task<PackageBundleReference> BuildBundle(string projectId, string id, string version)
        {
            return await this.packageService.BuildBundle(this.HttpContext.GetShocPrincipal(), projectId, id, version);
        }

        /// <summary>
        /// Deletes a package with the given id
        /// </summary>
        /// <param name="projectId">The project id</param>
        /// <param name="id">The id of package to delete</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public Task<ShocPackage> DeleteById(string projectId, string id)
        {
            return this.packageService.DeleteById(this.HttpContext.GetShocPrincipal(), projectId, id);
        }
    }
}
