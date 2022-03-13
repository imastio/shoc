using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shoc.ApiCore;
using Shoc.ApiCore.Protection;
using Shoc.Builder.Model.Package;
using Shoc.Builder.Services;

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
            // try get result
            return this.packageService.GetById(this.HttpContext.GetShocPrincipal(), projectId, id);
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
