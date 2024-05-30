using System.Collections.Generic;
using System.Threading.Tasks;
using Shoc.Identity.Model.UserGroup;
using Shoc.Identity.Provider.Data;

namespace Shoc.Identity.Services;

/// <summary>
/// The user group members access service
/// </summary>
public class UserGroupAccessService : UserGroupServiceBase
{
    /// <summary>
    /// The access repository
    /// </summary>
    private readonly IUserGroupAccessRepository userGroupAccessRepository;

    /// <summary>
    /// Creates new instance of user group access service
    /// </summary>
    /// <param name="userGroupRepository"></param>
    /// <param name="userGroupAccessRepository">The access repository</param>
    public UserGroupAccessService(IUserGroupRepository userGroupRepository, IUserGroupAccessRepository userGroupAccessRepository) : base(userGroupRepository)
    {
        this.userGroupAccessRepository = userGroupAccessRepository;
    }

    /// <summary>
    /// Gets all the accesses granted to the group by the group id
    /// </summary>
    /// <param name="groupId">The group id</param>
    /// <returns></returns>
    public async Task<IEnumerable<UserGroupAccessModel>> Get(string groupId)
    {
        // require the group to be exist
        await this.RequireById(groupId);
        
        // get all objects
        return await this.userGroupAccessRepository.GetByGroupId(groupId);
    }

    /// <summary>
    /// Updates the accesses assigned to user group.
    /// </summary>
    /// <param name="groupId">The group id</param>
    /// <param name="input">The creation input</param>
    /// <returns></returns>
    public async Task<UserGroupAccessUpdateResultModel> Update(string groupId, UserGroupAccessUpdateModel input)
    {
        // make sure group exists
        await this.RequireById(groupId);
        
        // perform the operation
        return await this.userGroupAccessRepository.UpdateByGroupId(groupId, input);
    }
}