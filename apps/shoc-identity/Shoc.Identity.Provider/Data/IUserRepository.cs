using System.Collections.Generic;
using System.Threading.Tasks;
using Shoc.Identity.Model.User;
using Shoc.Identity.Model.UserGroup;

namespace Shoc.Identity.Provider.Data;

/// <summary>
/// The users repository
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Gets all the users
    /// </summary>
    /// <param name="filter">The filter to apply</param>
    /// <returns></returns>
    Task<IEnumerable<UserModel>> GetAll(UserFilterModel filter);

    /// <summary>
    /// Gets page of users by filter
    /// </summary>
    /// <param name="filter">The filter to apply</param>
    /// <param name="page">The page number</param>
    /// <param name="size">The page size</param>
    /// <returns></returns>
    Task<UserPageResult> GetPageBy(UserFilterModel filter, int page, int size);

    /// <summary>
    /// Gets the referential values by the given filter
    /// </summary>
    /// <param name="filter">The filter to apply</param>
    /// <param name="page">The page number</param>
    /// <param name="size">The size</param>
    /// <returns></returns>
    Task<UserReferentialValuePageResult> GetReferentialValuesPageBy(UserFilterModel filter, int page, int size);

    /// <summary>
    /// Gets the user by id
    /// </summary>
    /// <param name="id">The user id</param>
    /// <returns></returns>
    Task<UserModel> GetById(string id);
    
    /// <summary>
    /// Gets the user by email
    /// </summary>
    /// <param name="email">The user email</param>
    /// <returns></returns>
    Task<UserModel> GetByEmail(string email);
    
    /// <summary>
    /// Gets the user profile by id
    /// </summary>
    /// <param name="id">The user id</param>
    /// <returns></returns>
    Task<UserProfileModel> GetProfileById(string id);

    /// <summary>
    /// Gets the user picture by id
    /// </summary>
    /// <param name="id">The user id</param>
    /// <returns></returns>
    Task<UserPictureModel> GetPictureById(string id);

    /// <summary>
    /// Gets the references to the groups the user is member of
    /// </summary>
    /// <param name="id">The id of user to request</param>
    /// <returns></returns>
    public Task<IEnumerable<UserGroupReferentialValueModel>> GetGroupsById(string id);

    /// <summary>
    /// Gets the references to the roles the user is member of.
    /// </summary>
    /// <param name="id">The id of user to request.</param>
    /// <returns></returns>
    public Task<IEnumerable<UserRoleReferentialValueModel>> GetRolesById(string id);

    /// <summary>
    /// Creates entity based on input
    /// </summary>
    /// <param name="input">The creation input</param>
    /// <returns></returns>
    Task<UserUpdateResultModel> Create(UserCreateModel input);
    
    /// <summary>
    /// Updates the user model by id
    /// </summary>
    /// <param name="id">The id of user</param>
    /// <param name="input">The user update input</param>
    /// <returns></returns>
    Task<UserUpdateResultModel> UpdateById(string id, UserUpdateModel input);

    /// <summary>
    /// Updates the user type by id
    /// </summary>
    /// <param name="id">The id of user</param>
    /// <param name="userType">The type of user to assign</param>
    /// <returns></returns>
    Task<UserUpdateResultModel> UpdateTypeById(string id, string userType);

    /// <summary>
    /// Updates the user state by id
    /// </summary>
    /// <param name="id">The id of user</param>
    /// <param name="userState">The state of user to assign</param>
    /// <returns></returns>
    Task<UserUpdateResultModel> UpdateUserStateById(string id, string userState);

    /// <summary>
    /// Updates the user profile picture
    /// </summary>
    /// <param name="id">The id of user</param>
    /// <param name="input">The update input</param>
    /// <returns></returns>
    Task<UserPictureModel> UpdatePictureById(string id, UserPictureUpdateModel input);

    /// <summary>
    /// Updates the user profile info
    /// </summary>
    /// <param name="id">The id of user</param>
    /// <param name="input">The update input</param>
    /// <returns></returns>
    Task<UserProfileModel> UpdateProfileById(string id, UserProfileUpdateModel input);

    /// <summary>
    /// Deletes the user by id
    /// </summary>
    /// <param name="id">The user id</param>
    /// <returns></returns>
    Task<UserModel> DeleteById(string id);
}
