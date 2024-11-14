using System.Collections.Generic;
using System.Threading.Tasks;
using Imast.DataOps.Api;
using Shoc.Core;
using Shoc.Package.Model;
using Shoc.Package.Model.Package;

namespace Shoc.Package.Data.Sql;

/// <summary>
/// The repository interface implementation
/// </summary>
public class PackageRepository : IPackageRepository
{
    /// <summary>
    /// The data operations instance
    /// </summary>
    private readonly DataOperations dataOps;

    /// <summary>
    /// Creates new instance of repository implementation
    /// </summary>
    /// <param name="dataOps">A DataOps instance</param>
    public PackageRepository(DataOperations dataOps)
    {
        this.dataOps = dataOps;
    }

    /// <summary>
    /// Gets all the objects
    /// </summary>
    /// <returns></returns>
    public Task<IEnumerable<PackageModel>> GetAll(string workspaceId)
    {
        return this.dataOps.Connect().Query("Package", "GetAll").ExecuteAsync<PackageModel>(new
        {
            WorkspaceId = workspaceId
        });
    }

    /// <summary>
    /// Gets page of the extended objects
    /// </summary>
    /// <returns></returns>
    public async Task<PackagePageResult<PackageExtendedModel>> GetExtendedPageBy(string workspaceId, PackageFilter filter, int page, int size)
    {
        // the query argument
        var arg = new
        {
            WorkspaceId = workspaceId,
            filter.UserId,
            filter.Scope,
            Offset = page * size,
            Count = size
        };
        
        // load page of objects
        var items = await this.dataOps.Connect().Query("Package", "GetExtendedPageBy")
            .WithBinding("ByUser", !string.IsNullOrWhiteSpace(arg.UserId))
            .WithBinding("ByScope", !string.IsNullOrWhiteSpace(arg.Scope))
            .ExecuteAsync<PackageExtendedModel>(arg);
        
        // count total count separately by now
        // fix the multi-query with binding
        var totalCount = await this.dataOps.Connect().QueryFirst("Package", "CountBy")
            .WithBinding("ByUser", !string.IsNullOrWhiteSpace(arg.UserId))
            .WithBinding("ByScope", !string.IsNullOrWhiteSpace(arg.Scope))
            .ExecuteAsync<long>(arg);
        
        return new PackagePageResult<PackageExtendedModel>
        {
            Items = items,
            TotalCount = totalCount
        };
    }

    /// <summary>
    /// Gets all the objects by listing checksum
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="listingChecksum">The listing checksum</param>
    /// <returns></returns>
    public Task<IEnumerable<PackageModel>> GetAllByListingChecksum(string workspaceId, string listingChecksum)
    {
        return this.dataOps.Connect().Query("Package", "GetAllByListingChecksum").ExecuteAsync<PackageModel>(new
        {
            WorkspaceId = workspaceId,
            ListingChecksum = listingChecksum
        });
    }

    /// <summary>
    /// Gets the object by id
    /// </summary>
    /// <returns></returns>
    public Task<PackageModel> GetById(string workspaceId, string id)
    {
        return this.dataOps.Connect().QueryFirst("Package", "GetById").ExecuteAsync<PackageModel>(new
        {
            WorkspaceId = workspaceId,
            Id = id
        });
    }

    /// <summary>
    /// Creates the object with given input
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="input">The creation input</param>
    /// <returns></returns>
    public Task<PackageModel> Create(string workspaceId, PackageCreateModel input)
    {
        // assign the id
        input.WorkspaceId = workspaceId;
        input.Id ??= StdIdGenerator.Next(PackageObjects.PACKAGE).ToLowerInvariant();

        return this.dataOps.Connect().QueryFirst("Package", "Create").ExecuteAsync<PackageModel>(input);
    }
    
    /// <summary>
    /// Deletes the object by id
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="id">The id of the object</param>
    /// <returns></returns>
    public Task<PackageModel> DeleteById(string workspaceId, string id)
    {
        return this.dataOps.Connect().QueryFirst("Package", "DeleteById").ExecuteAsync<PackageModel>(new
        {
            WorkspaceId = workspaceId,
            Id = id
        });
    }
}