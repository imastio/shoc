using System.Collections.Generic;
using System.Threading.Tasks;
using Shoc.Identity.Model.Roles;

namespace Shoc.Identity.Provider.Data;

/// <summary>
/// The role repository interface.
/// </summary>
public interface IRoleRepository
{
    /// <summary>
    /// Get all roles.
    /// </summary>
    /// <returns>A task containing all roles.</returns>
    Task<IEnumerable<RoleModel>> GetAll();

    /// <summary>
    /// Gets the role by id.
    /// </summary>
    /// <param name="id">The role id.</param>
    /// <returns>A task containing the specified role.</returns>
    Task<RoleModel> GetById(string id);

    /// <summary>
    /// Gets all role referential values.
    /// </summary>
    /// <returns>A task containing all role referential values.</returns>
    Task<IEnumerable<RoleReferentialValueModel>> GetAllReferentialValues();

    /// <summary>
    /// Gets the role by name.
    /// </summary>
    /// <param name="name">The role name.</param>
    /// <returns>A task containing the role.</returns>
    Task<RoleModel> GetByName(string name);

    /// <summary>
    /// Creates role entity based on input.
    /// </summary>
    /// <param name="input">The role creation input.</param>
    /// <returns>A task containing newly created role.</returns>
    Task<RoleModel> Create(RoleCreateModel input);

    /// <summary>
    /// Update the role by given input.
    /// </summary>
    /// <param name="input">The role update input.</param>
    /// <returns>A task containing the updated the role.</returns>
    Task<RoleModel> UpdateById(RoleUpdateModel input);

    /// <summary>
    /// Deletes the role by id.
    /// </summary>
    /// <param name="id">The role id.</param>
    /// <returns>A task containing the deleted role.</returns>
    Task<RoleModel> DeleteById(string id);
}
