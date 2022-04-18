using System.Collections.Generic;
using System.Threading.Tasks;
using Imast.DataOps.Api;
using Shoc.Builder.Model.Package;
using Shoc.Core;

namespace Shoc.Builder.Data.Sql
{
    /// <summary>
    /// The package repository implementation
    /// </summary>
    public class PackageRepository : IPackageRepository
    {
        /// <summary>
        /// The data operations instance
        /// </summary>
        private readonly DataOperations dataOps;

        /// <summary>
        /// Creates new instance of package repository implementation
        /// </summary>
        /// <param name="dataOps">A DataOps instance</param>
        public PackageRepository(DataOperations dataOps)
        {
            this.dataOps = dataOps;
        }

        /// <summary>
        /// Gets all the packages
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<ShocPackage>> GetAll()
        {
            return this.dataOps.Connect().Query("Package", "GetAll").ExecuteAsync<ShocPackage>();
        }

        /// <summary>
        /// Gets all the packages for the given query
        /// </summary>
        /// <param name="query">The query to filter with</param>
        /// <returns></returns>
        public Task<IEnumerable<ShocPackage>> GetBy(PackageQuery query)
        {
            return this.dataOps.Connect().Query("Package", "GetBy")
                .WithBinding("ByListingChecksum", query.ListingChecksum != null)
                .WithBinding("ByProject", query.ProjectId != null)
                .ExecuteAsync<ShocPackage>(query);
        }

        /// <summary>
        /// Gets the package by given id
        /// </summary>
        /// <param name="id">The package id</param>
        /// <returns></returns>
        public Task<ShocPackage> GetById(string id)
        {
            return this.dataOps.Connect().QueryFirst("Package", "GetById").ExecuteAsync<ShocPackage>(new
            {
                Id = id
            });
        }

        /// <summary>
        /// Creates a new package with given input
        /// </summary>
        /// <param name="input">The package creation input</param>
        /// <returns></returns>
        public Task<ShocPackage> Create(CreatePackageInput input)
        {
            // generate id if necessary
            input.Id ??= StdIdGenerator.Next(BuilderObjects.PACKAGE)?.ToLowerInvariant();

            // add object to the database
            return this.dataOps.Connect().QueryFirst("Package", "Create").ExecuteAsync<ShocPackage>(input);
        }

        /// <summary>
        /// Updates the package status based on input
        /// </summary>
        /// <param name="input">The package status update model</param>
        /// <returns></returns>
        public Task<ShocPackage> UpdateStatus(PackageStatusModel input)
        {
            // do update
            return this.dataOps.Connect().QueryFirst("Package", "UpdateStatus").ExecuteAsync<ShocPackage>(input);
        }

        /// <summary>
        /// Updates the package image references based on input
        /// </summary>
        /// <param name="input">The package image update model</param>
        /// <returns></returns>
        public Task<ShocPackage> UpdateImage(PackageImageModel input)
        {
            // do update
            return this.dataOps.Connect().QueryFirst("Package", "UpdateImage").ExecuteAsync<ShocPackage>(input);
        }

        /// <summary>
        /// Deletes the package by given id
        /// </summary>
        /// <param name="id">The package id</param>
        /// <returns></returns>
        public Task<ShocPackage> DeleteById(string id)
        {
            // delete object from the database
            return this.dataOps.Connect().QueryFirst("Package", "DeleteById").ExecuteAsync<ShocPackage>(new
            {
                Id = id
            });
        }

        /// <summary>
        /// Gets the bundle by id
        /// </summary>
        /// <param name="id">The id of bundle</param>
        /// <returns></returns>
        public Task<PackageBundleModel> GetBundleById(string id)
        {
            // gets object from the database
            return this.dataOps.Connect().QueryFirst("Package", "GetBundleById").ExecuteAsync<PackageBundleModel>(new
            {
                Id = id
            });
        }

        /// <summary>
        /// Create the bundle for a package
        /// </summary>
        /// <param name="input">The bundle input</param>
        /// <returns></returns>
        public Task<PackageBundleModel> CreateBundle(PackageBundleModel input)
        {
            // generate id if necessary
            input.Id ??= StdIdGenerator.Next(BuilderObjects.BUNDLE)?.ToLowerInvariant();

            // add object to the database
            return this.dataOps.Connect().QueryFirst("Package", "CreateBundle").ExecuteAsync<PackageBundleModel>(input);
        }

        /// <summary>
        /// Deletes the bundle by id
        /// </summary>
        /// <param name="id">The id of bundle</param>
        /// <returns></returns>
        public Task<PackageBundleModel> DeleteBundleById(string id)
        {
            // gets object from the database
            return this.dataOps.Connect().QueryFirst("Package", "DeleteBundleById").ExecuteAsync<PackageBundleModel>(new
            {
                Id = id
            });
        }
    }
}