using System.Collections.Generic;
using System.Threading.Tasks;
using Shoc.Builder.Model.Package;

namespace Shoc.Builder.Data
{
    /// <summary>
    /// The package repository interface
    /// </summary>
    public interface IPackageRepository
    {
        /// <summary>
        /// Gets all the packages
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<ShocPackage>> GetAll();

        /// <summary>
        /// Gets all the packages for the given query
        /// </summary>
        /// <param name="query">The query to filter with</param>
        /// <returns></returns>
        Task<IEnumerable<ShocPackage>> GetBy(PackageQuery query);

        /// <summary>
        /// Gets the package by given id
        /// </summary>
        /// <param name="id">The package id</param>
        /// <returns></returns>
        Task<ShocPackage> GetById(string id);

        /// <summary>
        /// Creates a new package with given input
        /// </summary>
        /// <param name="input">The package creation input</param>
        /// <returns></returns>
        Task<ShocPackage> Create(CreatePackageInput input);

        /// <summary>
        /// Updates the package status based on input
        /// </summary>
        /// <param name="input">The package status update model</param>
        /// <returns></returns>
        Task<ShocPackage> UpdateStatus(PackageStatusModel input);

        /// <summary>
        /// Updates the package image references based on input
        /// </summary>
        /// <param name="input">The package image update model</param>
        /// <returns></returns>
        Task<ShocPackage> UpdateImage(PackageImageModel input);

        /// <summary>
        /// Deletes the package by given id
        /// </summary>
        /// <param name="id">The package id</param>
        /// <returns></returns>
        Task<ShocPackage> DeleteById(string id);

        /// <summary>
        /// Gets the bundle by id
        /// </summary>
        /// <param name="id">The id of bundle</param>
        /// <returns></returns>
        Task<PackageBundleModel> GetBundleById(string id);

        /// <summary>
        /// Create the bundle for a package
        /// </summary>
        /// <param name="input">The bundle input</param>
        /// <returns></returns>
        Task<PackageBundleModel> CreateBundle(PackageBundleModel input);

        /// <summary>
        /// Deletes the bundle by id
        /// </summary>
        /// <param name="id">The id of bundle</param>
        /// <returns></returns>
        Task<PackageBundleModel> DeleteBundleById(string id);
    }
}