using System.Collections.Generic;
using System.Threading.Tasks;
using Shoc.Core;
using Shoc.Identity.Model;
using Shoc.Identity.Model.User;
using Shoc.Identity.Model.UserGroup;
using Shoc.Identity.Provider.Data;

namespace Shoc.Identity.Services;

/// <summary>
/// The user group members service
/// </summary>
public class UserGroupMembersService : UserGroupServiceBase
{

    /// <summary>
    /// The user group member repository
    /// </summary>
    private readonly IUserGroupMemberRepository userGroupMemberRepository;

    /// <summary>
    /// The user internal repository
    /// </summary>
    private readonly IUserInternalRepository userInternalRepository;

    /// <summary>
    /// Creates new instance of user group member service
    /// </summary>
    /// <param name="userGroupRepository">The user group repository</param>
    /// <param name="userGroupMemberRepository">The user group member repository</param>
    /// <param name="userInternalRepository">The user internal repository</param>
    public UserGroupMembersService(IUserGroupRepository userGroupRepository, IUserGroupMemberRepository userGroupMemberRepository, IUserInternalRepository userInternalRepository) : base(userGroupRepository)
    {
        this.userGroupMemberRepository = userGroupMemberRepository;
        this.userInternalRepository = userInternalRepository;
    }

    /// <summary>
    /// Gets all the member users by the group id
    /// </summary>
    /// <param name="groupId">The group id</param>
    /// <returns></returns>
    public async Task<IEnumerable<UserReferentialValueModel>> GetAll(string groupId)
    {
        // require the group to be exist
        await this.RequireById(groupId);
        
        // get all objects
        return await this.userGroupMemberRepository.GetAll(groupId);
    }

    /// <summary>
    /// Gets the particular user in the group
    /// </summary>
    /// <param name="groupId">The group id</param>
    /// <param name="userId">The user id</param>
    /// <returns></returns>
    public async Task<UserReferentialValueModel> GetById(string groupId, string userId)
    {
        // make sure both requirements are fulfilled 
        await Task.WhenAll(this.RequireById(groupId), this.RequireUserById(userId));

        // return the result 
        return await this.userGroupMemberRepository.GetById(groupId, userId);
    }

    /// <summary>
    /// Creates new membership record for the given
    /// </summary>
    /// <param name="groupId">The group id</param>
    /// <param name="input">The creation input</param>
    /// <returns></returns>
    public async Task<UserReferentialValueModel> Create(string groupId, UserGroupMembership input)
    {
        // make sure referring to the correct object
        input.GroupId = groupId;

        // try get the specified user in the group
        var result = await this.GetById(groupId, input.UserId);

        // throw if the user already exists in the group
        if (result != null)
        {
            throw ErrorDefinition.Validation(IdentityErrors.EXISTING_GROUP_USER).AsException();
        }

        // perform the operation
        return await this.userGroupMemberRepository.Create(input);
    }

    /// <summary>
    /// Deletes the member user from the group
    /// </summary>
    /// <param name="groupId">The group id</param>
    /// <param name="userId">The user id</param>
    /// <returns></returns>
    public async Task<UserReferentialValueModel> DeleteById(string groupId, string userId)
    {
        // make sure both requirements are fulfilled 
        await Task.WhenAll(this.RequireById(groupId), this.RequireUserById(userId));

        // perform and return the result 
        return await this.userGroupMemberRepository.DeleteById(groupId, userId);
    }

    /// <summary>
    /// Requires the user by the given key to exist
    /// </summary>
    /// <param name="userId">The user id</param>
    /// <returns></returns>
    private async Task<UserInternalModel> RequireUserById(string userId)
    {
        // try get the object by id
        var result = await this.userInternalRepository.GetById(userId);

        // check if exists
        if (result == null)
        {
            throw ErrorDefinition.Validation(IdentityErrors.NO_USER).AsException();
        }

        return result;
    }
}