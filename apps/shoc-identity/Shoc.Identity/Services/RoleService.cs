using System.Collections.Generic;
using System.Threading.Tasks;
using Shoc.Core;
using Shoc.Identity.Model;
using Shoc.Identity.Model.Roles;
using Shoc.Identity.Provider.Data;

namespace Shoc.Identity.Services;

/// <summary>
/// The role service.
/// </summary>
public class RoleService : RoleServiceBase
{
    /// <summary>
    /// Creates new instance of role service.
    /// </summary>
    /// <param name="roleRepository">The role repository.</param>
    public RoleService(IRoleRepository roleRepository) : base(roleRepository)
    {
    }

    /// <summary>
    /// Gets all roles.
    /// </summary>
    /// <returns>A task containing the roles.</returns>
    public Task<IEnumerable<RoleModel>> GetAll()
    {
        return this.roleRepository.GetAll();
    }

    /// <summary>
    /// Get the role by id.
    /// </summary>
    /// <param name="id">The role id.</param>
    /// <returns>A task containing the role.</returns>
    public Task<RoleModel> GetById(string id)
    {
        return this.RequireById(id);
    }

    /// <summary>
    /// Gets all role referential values.
    /// </summary>
    /// <returns>A task containing all role referential values.</returns>
    public Task<IEnumerable<RoleReferentialValueModel>> GetAllReferentialValues()
    {
        return this.roleRepository.GetAllReferentialValues();
    }

    /// <summary>
    /// Creates a role by given input.
    /// </summary>
    /// <param name="input">The role creation input.</param>
    /// <returns>A task containing a newly created role.</returns>
    public async Task<RoleModel> Create(RoleCreateModel input)
    {
        // validate the role name
        ValidateName(input.Name);

        // validate the role description
        ValidateDescription(input.Description);

        // try get the existing by name
        var existing = await this.roleRepository.GetByName(input.Name);

        // throw error if there is already a role with the given name
        if (existing != null)
        {
            throw ErrorDefinition.Validation(IdentityErrors.EXISTING_ROLE_NAME).AsException();
        }

        return await this.roleRepository.Create(input);
    }


    public async Task<RoleModel> UpdateById(string id, RoleUpdateModel input)
    {
        // make sure the role exists.
        await this.RequireById(id);

        // validate the role name
        ValidateName(input.Name);

        // validate the role description
        ValidateDescription(input.Description);

        // try get the existing by name
        var existing = await this.roleRepository.GetByName(input.Name);

        // throw error if there is already a role with the given name other then the existing
        if (existing != null && existing.Id != id)
        {
            throw ErrorDefinition.Validation(IdentityErrors.EXISTING_ROLE_NAME).AsException();
        }

        // add existing id
        input.Id = id;

        // update the role
        return await this.roleRepository.UpdateById(input);
    }

    /// <summary>
    /// Deletes the role by id.
    /// </summary>
    /// <param name="id">The role id.</param>
    /// <returns>A task containing the deleted role.</returns>
    public async Task<RoleModel> DeleteById(string id)
    {
        // try to delete the role by id
        var result = await this.roleRepository.DeleteById(id);

        // throw error if not found
        if (result == null)
        {
            throw ErrorDefinition.NotFound().AsException();
        }

        return result;
    }
}
