using System.Collections.Generic;
using System.Threading.Tasks;
using Imast.DataOps.Api;
using Shoc.Core;
using Shoc.Identity.Model;
using Shoc.Identity.Model.Roles;

namespace Shoc.Identity.Provider.Data.Mysql;

/// <summary>
/// The role repository.
/// </summary>
public class RoleRepository : IRoleRepository
{
    /// <summary>
    /// The data operations instance.
    /// </summary>
    private readonly DataOperations dataOps;

    /// <summary>
    /// Creates new instance of roles repository implementation.
    /// </summary>
    /// <param name="dataOps">A DataOps instance.</param>
    public RoleRepository(DataOperations dataOps)
    {
        this.dataOps = dataOps;
    }

    /// <summary>
    /// Get all roles.
    /// </summary>
    /// <returns></returns>
    public Task<IEnumerable<RoleModel>> GetAll()
    {
        return this.dataOps.Connect().Query("Identity.Role", "GetAll").ExecuteAsync<RoleModel>();
    }

    /// <summary>
    /// Get role by id.
    /// </summary>
    /// <param name="id">The role id.</param>
    /// <returns>A task containing the role.</returns>
    public Task<RoleModel> GetById(string id)
    {
        return this.dataOps.Connect().QueryFirst("Identity.Role", "GetById").ExecuteAsync<RoleModel>(new
        {
           Id = id?.ToLowerInvariant()
        });
    }

    /// <summary>
    /// Gets all role referential values.
    /// </summary>
    /// <returns>A task containing all role referential values.</returns>
    public Task<IEnumerable<RoleReferentialValueModel>> GetAllReferentialValues()
    {
        return this.dataOps.Connect().Query("Identity.Role", "GetAllReferentialValues").ExecuteAsync<RoleReferentialValueModel>();
    }

    /// <summary>
    /// Get the role by given name.
    /// </summary>
    /// <param name="name">The role name.</param>
    /// <returns>A task containing the role.</returns>
    public Task<RoleModel> GetByName(string name)
    {
        return this.dataOps.Connect().QueryFirst("Identity.Role", "GetByName").ExecuteAsync<RoleModel>(new
        {
            Name = name
        });
    }

    /// <summary>
    /// Creates new role by given input.
    /// </summary>
    /// <param name="input">The role creation input.</param>
    /// <returns>A task containing the newly created role.</returns>
    public Task<RoleModel> Create(RoleCreateModel input)
    {
        // generate id if necessary
        input.Id ??= StdIdGenerator.Next(IdentityObjects.ROLE)?.ToLowerInvariant();

        // add role to the database
        return this.dataOps.Connect().QueryFirst("Identity.Role", "Create").ExecuteAsync<RoleModel>(input);
    }

    /// <summary>
    /// Update the role by given input.
    /// </summary>
    /// <param name="input">The role update input.</param>
    /// <returns>A task containing the updated role.</returns>
    public Task<RoleModel> UpdateById(RoleUpdateModel input)
    {
        return this.dataOps.Connect().QueryFirst("Identity.Role", "UpdateById").ExecuteAsync<RoleModel>(input);
    }

    /// <summary>
    /// Deletes the role by id.
    /// </summary>
    /// <param name="id">The role id.</param>
    /// <returns>A task containing the deleted role.</returns>
    public Task<RoleModel> DeleteById(string id)
    {
        return this.dataOps.Connect().QueryFirst("Identity.Role", "DeleteById").ExecuteAsync<RoleModel>(new
        {
            Id = id?.ToLowerInvariant()
        });
    }
}
