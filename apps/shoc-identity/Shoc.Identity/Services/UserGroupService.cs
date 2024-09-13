using System.Collections.Generic;
using System.Threading.Tasks;
using Shoc.Core;
using Shoc.Identity.Model;
using Shoc.Identity.Model.UserGroup;
using Shoc.Identity.Provider.Data;

namespace Shoc.Identity.Services;

/// <summary>
/// The user group service
/// </summary>
public class UserGroupService : UserGroupServiceBase
{
    /// <summary>
    /// Creates new instance of user group service
    /// </summary>
    /// <param name="userGroupRepository">The user group repository</param>
    public UserGroupService(IUserGroupRepository userGroupRepository) : base(userGroupRepository)
    {
    }

    /// <summary>
    /// Gets all the user groups
    /// </summary>
    /// <returns></returns>
    public Task<IEnumerable<UserGroupModel>> GetAll()
    {
        return this.userGroupRepository.GetAll();
    }
    
    /// <summary>
    /// Gets the user group by id
    /// </summary>
    /// <param name="id">The user group id</param>
    /// <returns></returns>
    public Task<UserGroupModel> GetById(string id)
    {
        return this.RequireById(id);
    }

    /// <summary>
    /// Creates entity based on input
    /// </summary>
    /// <param name="input">The creation input</param>
    /// <returns></returns>
    public async Task<UserGroupModel> Create(UserGroupCreateModel input)
    {
        // validate the name
        this.ValidateName(input.Name);

        // try get by existing name
        var existing = await this.userGroupRepository.GetByName(input.Name);

        // if there is already a group with the given name
        if (existing != null)
        {
            throw ErrorDefinition.Validation(IdentityErrors.EXISTING_GROUP_NAME).AsException();
        }

        return await this.userGroupRepository.Create(input);
    }

    /// <summary>
    /// Update the user group by given input.
    /// </summary>
    /// <param name="id">The user group id.</param>
    /// <param name="input">The user group update input.</param>
    /// <returns></returns>
    public async Task<UserGroupModel> UpdateById(string id, UserGroupUpdateModel input)
    {
        // make sure the user group exists
        await this.RequireById(id);

        // validate the name
        this.ValidateName(input.Name);

        // try get by existing name
        var existing = await this.userGroupRepository.GetByName(input.Name);

        // throw error if there is already a user group with the given name other than the existing
        if (existing != null && existing.Id != id)
        {
            throw ErrorDefinition.Validation(IdentityErrors.EXISTING_GROUP_NAME).AsException();
        }

        // make sure referring to same object
        input.Id = id;

        // update the user group
        return await this.userGroupRepository.UpdateById(input);
    }
    
    /// <summary>
    /// Deletes the user group by id
    /// </summary>
    /// <param name="id">The user group id</param>
    /// <returns></returns>
    public async Task<UserGroupModel> DeleteById(string id)
    {
        // try delete by id
        var result = await this.userGroupRepository.DeleteById(id);

        // handle if not found
        if (result == null)
        {
            throw ErrorDefinition.NotFound().AsException();
        }

        return result;
    }
}    

