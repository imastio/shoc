using System.Collections.Generic;
using System.Threading.Tasks;
using Shoc.Identity.Model.Privileges;

namespace Shoc.Identity.Provider.Data;

/// <summary>
/// The privilege repository interface.
/// </summary>
public interface IPrivilegeRepository
{
    /// <summary>
    /// Get all privileges.
    /// </summary>
    /// <returns>A task containing all privileges.</returns>
    Task<IEnumerable<PrivilegeModel>> GetAll();

    /// <summary>
    /// Get all privilege references.
    /// </summary>
    /// <returns>A task containing all privilege references.</returns>
    Task<IEnumerable<PrivilegeReferentialValueModel>> GetAllReferentialValues();

    /// <summary>
    /// Gets the privilege by id.
    /// </summary>
    /// <param name="id">The privilege id.</param>
    /// <returns>A task containing the specified privilege.</returns>
    Task<PrivilegeModel> GetById(string id);

    /// <summary>
    /// Gets the privilege by name.
    /// </summary>
    /// <param name="name">The privilege name.</param>
    /// <returns></returns>
    Task<PrivilegeModel> GetByName(string name);

    /// <summary>
    /// Creates privilege entity based on input.
    /// </summary>
    /// <param name="input">The privilege creation input.</param>
    /// <returns>A task containing newly created privilege.</returns>
    Task<PrivilegeModel> Create(PrivilegeCreateModel input);

    /// <summary>
    /// Update the privilege by given input.
    /// </summary>
    /// <param name="input">The privilege update input.</param>
    /// <returns>A task containing the updated privilege.</returns>
    Task<PrivilegeModel> UpdateById(PrivilegeUpdateModel input);

    /// <summary>
    /// Deletes the privilege by id.
    /// </summary>
    /// <param name="id">The privilege id.</param>
    /// <returns>A task containing the deleted privilege.</returns>
    Task<PrivilegeModel> DeleteById(string id);
}

