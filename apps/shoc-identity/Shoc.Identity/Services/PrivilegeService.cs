using System.Collections.Generic;
using System.Threading.Tasks;
using Shoc.Core;
using Shoc.Identity.Model;
using Shoc.Identity.Model.Privileges;
using Shoc.Identity.Provider.Data;

namespace Shoc.Identity.Services;

/// <summary>
/// The privilege service.
/// </summary>
public class PrivilegeService : PrivilegeServiceBase
{
    /// <summary>
    /// Creates new instance of privilege service.
    /// </summary>
    /// <param name="privilegeRepository">The privilege repository.</param>
    public PrivilegeService(IPrivilegeRepository privilegeRepository) : base(privilegeRepository)
    {
    }

    /// <summary>
    /// Get all privileges.
    /// </summary>
    /// <returns></returns>
    public Task<IEnumerable<PrivilegeModel>> GetAll()
    {
        return this.privilegeRepository.GetAll();
    }

    /// <summary>
    /// Get all privilege references.
    /// </summary>
    /// <returns>A task containing all privilege references.</returns>
    public Task<IEnumerable<PrivilegeReferentialValueModel>> GetAllReferentialValues()
    {
        return this.privilegeRepository.GetAllReferentialValues();
    }

    /// <summary>
    /// Get the privilege by id.
    /// </summary>
    /// <param name="id">The privilege id.</param>
    /// <returns>A task containing the privilege.</returns>
    public Task<PrivilegeModel> GetById(string id)
    {
        return this.RequireById(id);
    }

    /// <summary>
    /// Creates a privilege by given input.
    /// </summary>
    /// <param name="input">The privilege creation input.</param>
    /// <returns>A task containing a newly created privilege.</returns>
    public async Task<PrivilegeModel> Create(PrivilegeCreateModel input)
    {
        // validate the privilege name
        ValidateName(input.Name);

        //validate the privilege category
        ValidateCategory(input.Category);

        // validate the privilege description
        ValidateDescription(input.Description);

        // try get the existing by name
        var existing = await this.privilegeRepository.GetByName(input.Name);

        // throw error if there is already a privilege with the given name
        if (existing != null)
        {
            throw ErrorDefinition.Validation(IdentityErrors.EXISTING_PRIVILEGE_NAME).AsException();
        }

        return await this.privilegeRepository.Create(input);
    }

    /// <summary>
    /// Updates the privilege by given input.
    /// </summary>
    /// <param name="id">The privilege id.</param>
    /// <param name="input">The privilege update input.</param>
    /// <returns>A task containing the updated privilege.</returns>
    public async Task<PrivilegeModel> UpdateById(string id, PrivilegeUpdateModel input)
    {
        // make sure the privilege exists
        await this.RequireById(id);

        // validate the privilege name
        ValidateName(input.Name);

        //validate the privilege category
        ValidateCategory(input.Category);

        // validate the privilege description
        ValidateDescription(input.Description);

        // try get the existing by name
        var existing = await this.privilegeRepository.GetByName(input.Name);

        // throw error if there is already a privilege with the given name other than the existing
        if (existing != null && existing.Id != id)
        {
            throw ErrorDefinition.Validation(IdentityErrors.EXISTING_PRIVILEGE_NAME).AsException();
        }

        // make sure referring to same object
        input.Id = id;

        return await this.privilegeRepository.UpdateById(input);
    }

    /// <summary>
    /// Deletes the privilege by id.
    /// </summary>
    /// <param name="id">The privilege id.</param>
    /// <returns>A task containing the deleted privilege.</returns>
    public async Task<PrivilegeModel> DeleteById(string id)
    {
        // make sure the privilege exists
        await this.RequireById(id);

        // try to delete the privilege by id
        var result = await this.privilegeRepository.DeleteById(id);

        // throw error if not found
        if (result == null)
        {
            throw ErrorDefinition.NotFound().AsException();
        }

        return result;
    }
}

