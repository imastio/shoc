using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shoc.ApiCore;
using Shoc.ApiCore.Access;
using Shoc.Core.OpenId;
using Shoc.Identity.Model;
using Shoc.Identity.Model.User;
using Shoc.Identity.Model.UserGroup;
using Shoc.Identity.Services;

namespace Shoc.Identity.Controllers;

/// <summary>
/// The users controller
/// </summary>
[Route("api/users")]
[ApiController]
[ShocExceptionHandler]
[BearerOnly]
public class UsersController : ControllerBase
{
    /// <summary>
    /// The user service
    /// </summary>
    private readonly UserService userService;

    /// <summary>
    /// Creates new instance of users controller
    /// </summary>
    /// <param name="userService">The users service</param>
    public UsersController(UserService userService)
    {
        this.userService = userService;
    }

    /// <summary>
    /// Gets users by filter
    /// </summary>
    /// <param name="search">The search term to apply</param>
    /// <param name="type">The user type</param>
    /// <param name="page">The page number</param>
    /// <param name="size">The page size</param>
    /// <returns></returns>
    [AuthorizeAnyAccess(IdentityAccesses.CONNECT_USERS_LIST)]
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string search, [FromQuery] string type, [FromQuery] int? page, [FromQuery] int? size)
    {
        // build the filter
        var filter = new UserFilterModel
        {
            Search = search,
            Type = type
        };

        return page == null ? Ok(await this.userService.GetAll(filter)) : Ok(await this.userService.GetPageBy(filter, page.Value, size));
    }
    
    /// <summary>
    /// Gets users by filter
    /// </summary>
    /// <param name="search">The search term to apply</param>
    /// <param name="type">The user type</param>
    /// <param name="page">The page number</param>
    /// <param name="size">The page size</param>
    /// <returns></returns>
    [AuthorizeAnyAccess(IdentityAccesses.CONNECT_USERS_LIST_REFERENCES)]
    [HttpGet("referential-values")]
    public async Task<UserReferentialValuePageResult> GetReferentialValuesPageBy([FromQuery] string search, [FromQuery] string type, [FromQuery] int page, [FromQuery] int? size)
    {
        // build the filter
        var filter = new UserFilterModel
        {
            Search = search,
            Type = type
        };

        return await this.userService.GetReferentialValuesPageBy(filter, page, size);
    }
    
    /// <summary>
    /// Gets the user by email
    /// </summary>
    /// <param name="email">The email of the user</param>
    /// <returns></returns>
    [AuthorizeAnyAccess(IdentityAccesses.CONNECT_USERS_READ, AllowedScopes = new []{ KnownScopes.SVC })]
    [HttpGet("by-email/{email}")]
    public Task<UserModel> GetByEmail(string email)
    {
        return this.userService.GetByEmail(email);
    }

    /// <summary>
    /// Gets the user by id
    /// </summary>
    /// <param name="id">The id of user to request</param>
    /// <returns></returns>
    [AuthorizeAnyAccess(IdentityAccesses.CONNECT_USERS_READ)]
    [HttpGet("{id}")]
    public Task<UserModel> GetById(string id)
    {
        return this.userService.GetById(id);
    }

    /// <summary>
    /// Gets the user profile by id
    /// </summary>
    /// <param name="id">The of user to request</param>
    /// <returns></returns>
    [AuthorizeAnyAccess(IdentityAccesses.CONNECT_USERS_READ)]
    [HttpGet("{id}/profile")]
    public Task<UserProfileModel> GetProfileById(string id)
    {
        return this.userService.GetProfileById(id);
    }

    /// <summary>
    /// Gets the user picture by id
    /// </summary>
    /// <param name="id">The of user to request</param>
    /// <returns></returns>
    [AuthorizeAnyAccess(IdentityAccesses.CONNECT_USERS_READ)]
    [HttpGet("{id}/picture")]
    public Task<UserPictureModel> GetPictureById(string id)
    {
        return this.userService.GetPictureById(id);
    }

    /// <summary>
    /// Gets the sign-in history of user by id
    /// </summary>
    /// <param name="id">The id of user to request</param>
    /// <param name="page">The page number</param>
    /// <param name="size">The page size</param>
    /// <returns></returns>
    [AuthorizeAnyAccess(IdentityAccesses.CONNECT_USERS_READ)]
    [HttpGet("{id}/sign-in-history")]
    public async Task<IActionResult> GetSigninHistoryById(string id, [FromQuery] int page, int? size)
    {
        return Ok(await this.userService.GetSignInHistoryPage(id, page, size));
    }
    
    /// <summary>
    /// Gets the references to the groups the user is member of
    /// </summary>
    /// <param name="id">The id of user to request</param>
    /// <returns></returns>
    [AuthorizeAnyAccess(IdentityAccesses.CONNECT_USERS_READ)]
    [HttpGet("{id}/groups")]
    public Task<IEnumerable<UserGroupReferentialValueModel>> GetGroupsById(string id)
    {
        return this.userService.GetGroupsById(id);
    }

    /// <summary>
    /// Gets the references to the roles the user is member of.
    /// </summary>
    /// <param name="id">The id of user to request.</param>
    /// <returns></returns>
    [AuthorizeAnyAccess(IdentityAccesses.CONNECT_USERS_READ)]
    [HttpGet("{id}/roles")]
    public Task<IEnumerable<UserRoleReferentialValueModel>> GetRolesById(string id)
    {
        return this.userService.GetRolesById(id);
    }

    /// <summary>
    /// Creates new user based on input
    /// </summary>
    /// <param name="input">The user input</param>
    /// <returns></returns>
    [AuthorizeAnyAccess(IdentityAccesses.CONNECT_USERS_CREATE, AllowedScopes = new []{ KnownScopes.SVC })]
    [HttpPost]
    public Task<UserUpdateResultModel> Create([FromBody] UserCreateModel input)
    {
        return this.userService.Create(input);
    }

    /// <summary>
    /// Update the profile of the given user
    /// </summary>
    /// <param name="id">The id of user to request</param>
    /// <param name="input">The profile update model</param>
    /// <returns></returns>
    [AuthorizeAnyAccess(IdentityAccesses.CONNECT_USERS_EDIT_IDENTITY)]
    [HttpPut("{id}")]
    public Task<UserUpdateResultModel> UpdateById(string id, UserUpdateModel input)
    {
        return this.userService.UpdateById(id, input);
    }

    /// <summary>
    /// Update the profile picture of the given user
    /// </summary>
    /// <param name="id">The id of user to request</param>
    /// <param name="input">The picture update model</param>
    /// <returns></returns>
    [AuthorizeAnyAccess(IdentityAccesses.CONNECT_USERS_EDIT_PROFILE)]
    [HttpPut("{id}/picture")]
    public Task<UserPictureModel> UpdatePictureById(string id, UserPictureUpdateModel input)
    {
        return this.userService.UpdatePictureById(id, input);
    }

    /// <summary>
    /// Update the profile of the given user
    /// </summary>
    /// <param name="id">The id of user to request</param>
    /// <param name="input">The profile update model</param>
    /// <returns></returns>
    [AuthorizeAnyAccess(IdentityAccesses.CONNECT_USERS_EDIT_PROFILE)]
    [HttpPut("{id}/profile")]
    public Task<UserProfileModel> UpdateProfileById(string id, UserProfileUpdateModel input)
    {
        return this.userService.UpdateProfileById(id, input);
    }

    /// <summary>
    /// Change the password of the given user
    /// </summary>
    /// <param name="id">The id of user to request</param>
    /// <param name="input">The password update model</param>
    /// <returns></returns>
    [AuthorizeAnyAccess(IdentityAccesses.CONNECT_USERS_MANAGE)]
    [HttpPut("{id}/password")]
    public Task<UserUpdateResultModel> UpdatePasswordById(string id, UserPasswordUpdateModel input)
    {
        return this.userService.UpdatePasswordById(id, input);
    }

    /// <summary>
    /// Update the type of the given user
    /// </summary>
    /// <param name="id">The id of user to request</param>
    /// <param name="input">The type update model</param>
    /// <returns></returns>
    [AuthorizeAnyAccess(IdentityAccesses.CONNECT_USERS_MANAGE_ACCESS)]
    [HttpPut("{id}/type")]
    public Task<UserUpdateResultModel> UpdateTypeById(string id, UserTypeUpdateModel input)
    {
        return this.userService.UpdateTypeById(id, input);
    }

    /// <summary>
    /// Update the state of the given user
    /// </summary>
    /// <param name="id">The id of user to request</param>
    /// <param name="input">The state update model</param>
    /// <returns></returns>
    [AuthorizeAnyAccess(IdentityAccesses.CONNECT_USERS_MANAGE)]
    [HttpPut("{id}/state")]
    public Task<UserUpdateResultModel> UpdateUserStateById(string id, UserStateUpdateModel input)
    {
        return this.userService.UpdateUserStateById(id, input);
    }

    /// <summary>
    /// Update the lockout of the given user
    /// </summary>
    /// <param name="id">The id of user to request</param>
    /// <param name="input">The lockout update model</param>
    /// <returns></returns>
    [AuthorizeAnyAccess(IdentityAccesses.CONNECT_USERS_MANAGE)]
    [HttpPut("{id}/lockout")]
    public Task<UserUpdateResultModel> UpdateLockoutById(string id, UserLockoutUpdateModel input)
    {
        return this.userService.UpdateLockoutById(id, input);
    }
    
    /// <summary>
    /// Deletes the given user
    /// </summary>
    /// <param name="id">The id of user to delete</param>
    /// <returns></returns>
    [AuthorizeAnyAccess(IdentityAccesses.CONNECT_USERS_DELETE)]
    [HttpDelete("{id}")]
    public Task<UserModel> DeleteById(string id)
    {
        return this.userService.DeleteById(id);
    }
}