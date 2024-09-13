using System.Collections.Generic;
using System.Threading.Tasks;
using Imast.DataOps.Api;
using Shoc.Core;
using Shoc.Identity.Model;
using Shoc.Identity.Model.Privileges;

namespace Shoc.Identity.Provider.Data.Mysql;

/// <summary>
/// The privilege repository.
/// </summary>
public class PrivilegeRepository : IPrivilegeRepository
{
    /// <summary>
    /// The data operations instance.
    /// </summary>
    private readonly DataOperations dataOps;

    /// <summary>
    /// Creates new instance of privileges repository implementation.
    /// </summary>
    /// <param name="dataOps">A DataOps instance.</param>
    public PrivilegeRepository(DataOperations dataOps)
    {
        this.dataOps = dataOps;
    }

    /// <summary>
    /// Get all privileges.
    /// </summary>
    /// <returns>A task containing all privileges.</returns>
    public Task<IEnumerable<PrivilegeModel>> GetAll()
    {
        return this.dataOps.Connect().Query("Identity.Privilege", "GetAll").ExecuteAsync<PrivilegeModel>();
    }

    /// <summary>
    /// Get all privilege references.
    /// </summary>
    /// <returns>A task containing all privilege references.</returns>
    public Task<IEnumerable<PrivilegeReferentialValueModel>> GetAllReferentialValues()
    {
        return this.dataOps.Connect().Query("Identity.Privilege", "GetAllReferentialValues").ExecuteAsync<PrivilegeReferentialValueModel>();
    }

    /// <summary>
    /// Get the privilege by the id.
    /// </summary>
    /// <param name="id">The privilege id.</param>
    /// <returns>A task containing the privilege.</returns>
    public Task<PrivilegeModel> GetById(string id)
    {
        // try load from database
        return this.dataOps.Connect().QueryFirst("Identity.Privilege", "GetById").ExecuteAsync<PrivilegeModel>(new
        {
            Id = id.ToLowerInvariant()
        });
    }

    /// <summary>
    /// Get the privilege by the name.
    /// </summary>
    /// <param name="name">The privilege name.</param>
    /// <returns>A task containing the privilege.</returns>
    public Task<PrivilegeModel> GetByName(string name)
    { 
        // empty names are not allowed
        if (string.IsNullOrWhiteSpace(name))
        {
            return Task.FromResult(default(PrivilegeModel));
        }

        // try load from database
        return this.dataOps.Connect().QueryFirst("Identity.Privilege", "GetByName").ExecuteAsync<PrivilegeModel>(new
        {
            Name = name
        });
    }

    /// <summary>
    /// Creates privilege by given input.
    /// </summary>
    /// <param name="input">The privilege creation input.</param>
    /// <returns>A task containing the newly created privilege.</returns>
    public Task<PrivilegeModel> Create(PrivilegeCreateModel input)
    {
        // generate id if necessary
        input.Id ??= StdIdGenerator.Next(IdentityObjects.PRIVILEGE)?.ToLowerInvariant();

        // add privilege to the database
        return this.dataOps.Connect().QueryFirst("Identity.Privilege", "Create").ExecuteAsync<PrivilegeModel>(input);
    }

    /// <summary>
    /// Update the privilege by given input.
    /// </summary>
    /// <param name="input">The privilege update input.</param>
    /// <returns>A task containing the updated privilege.</returns>
    public Task<PrivilegeModel> UpdateById(PrivilegeUpdateModel input)
    {
        return this.dataOps.Connect().QueryFirst("Identity.Privilege", "UpdateById").ExecuteAsync<PrivilegeModel>(input);
    }

    /// <summary>
    /// Delete the privilege by id.
    /// </summary>
    /// <param name="id">The privilege id.</param>
    /// <returns>A task containing the deleted privilege.</returns>
    public Task<PrivilegeModel> DeleteById(string id)
    {
        return this.dataOps.Connect().QueryFirst("Identity.Privilege", "DeleteById").ExecuteAsync<PrivilegeModel>(new
        {
            Id = id?.ToLowerInvariant()
        });
    }
}
