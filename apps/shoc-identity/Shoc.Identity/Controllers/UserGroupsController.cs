using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shoc.ApiCore;
using Shoc.ApiCore.Access;
using Shoc.Identity.Model;
using Shoc.Identity.Model.UserGroup;
using Shoc.Identity.Services;

namespace Shoc.Identity.Controllers;

/// <summary>
/// The user groups controller
/// </summary>
[Route("api/user-groups")]
[ApiController]
[ShocExceptionHandler]
[BearerOnly]
public class UserGroupsController : ControllerBase
{
    /// <summary>
    /// The user group service
    /// </summary>
    private readonly UserGroupService userGroupService;

    /// <summary>
    /// Creates new instance of user groups controller
    /// </summary>
    /// <param name="userGroupService">The user groups service</param>
    public UserGroupsController(UserGroupService userGroupService)
    {
        this.userGroupService = userGroupService;
    }

    /// <summary>
    /// Gets all user groups
    /// </summary>
    /// <returns></returns>
    [AuthorizeAnyAccess(IdentityAccesses.CONNECT_USER_GROUPS_LIST)]
    [HttpGet]
    public Task<IEnumerable<UserGroupModel>> GetAll()
    {
        return this.userGroupService.GetAll();
    }

    /// <summary>
    /// Gets the user group by id
    /// </summary>
    /// <param name="id">The of user group to request</param>
    /// <returns></returns>
    [AuthorizeAnyAccess(IdentityAccesses.CONNECT_USER_GROUPS_READ)]
    [HttpGet("{id}")]
    public Task<UserGroupModel> GetById(string id)
    {
        return this.userGroupService.GetById(id);
    }
    
    /// <summary>
    /// Creates new user group based on input
    /// </summary>
    /// <param name="input">The user group input</param>
    /// <returns></returns>
    [AuthorizeAnyAccess(IdentityAccesses.CONNECT_USER_GROUPS_CREATE)]
    [HttpPost]
    public Task<UserGroupModel> Create([FromBody] UserGroupCreateModel input)
    {
        return this.userGroupService.Create(input);
    }

    /// <summary>
    /// Updates the user group by given input.
    /// </summary>
    /// <param name="id">The user group id.</param>
    /// <param name="input">The user group update input.</param>
    /// <returns></returns>
    [AuthorizeAnyAccess(IdentityAccesses.CONNECT_USER_GROUPS_EDIT)]
    [HttpPut]
    public Task<UserGroupModel> UpdateById(string id, UserGroupUpdateModel input)
    {
        return this.userGroupService.UpdateById(id, input);
    }
    
    /// <summary>
    /// Deletes the given user group
    /// </summary>
    /// <param name="id">The id of user group to delete</param>
    /// <returns></returns>
    [AuthorizeAnyAccess(IdentityAccesses.CONNECT_USER_GROUPS_DELETE)]
    [HttpDelete("{id}")]
    public Task<UserGroupModel> DeleteById(string id)
    {
        return this.userGroupService.DeleteById(id);
    }
}
