using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Imast.DataOps.Api;
using Shoc.Core;
using Shoc.Identity.Model;
using Shoc.Identity.Model.User;
using Shoc.Identity.Model.UserGroup;
using Shoc.Identity.Provider.Utility;

namespace Shoc.Identity.Provider.Data.Mysql;

/// <summary>
/// The users repository implementation
/// </summary>
public class UserRepository : IUserRepository
{
    /// <summary>
    /// The data operations instance
    /// </summary>
    private readonly DataOperations dataOps;

    /// <summary>
    /// Creates new instance of users repository implementation
    /// </summary>
    /// <param name="dataOps">A DataOps instance</param>
    public UserRepository(DataOperations dataOps)
    {
        this.dataOps = dataOps;
    }

    /// <summary>
    /// Gets all the users
    /// </summary>
    /// <param name="filter">The filter to apply</param>
    /// <returns></returns>
    public Task<IEnumerable<UserModel>> GetAll(UserFilterModel filter)
    {
        // the operation argument
        var arg = new
        {
            Search = SqlUtility.AnyMatchLikeValue(filter.Search),
            filter.Type
        };

        return this.dataOps.Connect().Query("Identity.User", "GetAll")
            .WithBinding("WithSearch", !string.IsNullOrWhiteSpace(arg.Search))
            .WithBinding("ByType", !string.IsNullOrWhiteSpace(arg.Type))
            .ExecuteAsync<UserModel>(arg);
    }

    /// <summary>
    /// Gets page of users by filter
    /// </summary>
    /// <param name="filter">The filter to apply</param>
    /// <param name="page">The page number</param>
    /// <param name="size">The page size</param>
    /// <returns></returns>
    public async Task<UserPageResult> GetPageBy(UserFilterModel filter, int page, int size)
    {
        // the operation argument
        var arg = new
        {
            Search = SqlUtility.AnyMatchLikeValue(filter.Search),
            filter.Type,
            Offset = page * size,
            Count = size
        };

        // load page of items based on filter
        var items = await this.dataOps.Connect().Query("Identity.User", "GetPageBy")
            .WithBinding("WithSearch", !string.IsNullOrWhiteSpace(arg.Search))
            .WithBinding("ByType", !string.IsNullOrWhiteSpace(arg.Type))
            .ExecuteAsync<UserModel>(arg);

        // count total count separately by now
        // fix the multi-query with binding
        var totalCount = await this.dataOps.Connect().QueryFirst("Identity.User", "CountBy")
            .WithBinding("WithSearch", !string.IsNullOrWhiteSpace(arg.Search))
            .WithBinding("ByType", !string.IsNullOrWhiteSpace(arg.Type))
            .ExecuteAsync<long>(arg);

        return new UserPageResult
        {
            Items = items,
            TotalCount = totalCount
        };
    }

    /// <summary>
    /// Gets the referential values by the given filter
    /// </summary>
    /// <param name="filter">The filter to apply</param>
    /// <param name="page">The page number</param>
    /// <param name="size">The size</param>
    /// <returns></returns>
    public async Task<UserReferentialValuePageResult> GetReferentialValuesPageBy(UserFilterModel filter, int page, int size)
    {
        // the operation argument
        var arg = new
        {
            Search = SqlUtility.AnyMatchLikeValue(filter.Search),
            filter.Type,
            Offset = page * size,
            Count = size
        };

        // load page of items based on filter
        var items = await this.dataOps.Connect().Query("Identity.User", "GetReferentialValuesPageBy")
            .WithBinding("WithSearch", !string.IsNullOrWhiteSpace(arg.Search))
            .WithBinding("ByType", !string.IsNullOrWhiteSpace(arg.Type))
            .ExecuteAsync<UserReferentialValueModel>(arg);

        // count total count separately by now
        // fix the multi-query with binding
        var totalCount = await this.dataOps.Connect().QueryFirst("Identity.User", "CountBy")
            .WithBinding("WithSearch", !string.IsNullOrWhiteSpace(arg.Search))
            .WithBinding("ByType", !string.IsNullOrWhiteSpace(arg.Type))
            .ExecuteAsync<long>(arg);

        return new UserReferentialValuePageResult
        {
            Items = items,
            TotalCount = totalCount
        };
    }

    /// <summary>
    /// Gets the user by id
    /// </summary>
    /// <param name="id">The user id</param>
    /// <returns></returns>
    public Task<UserModel> GetById(string id)
    {
        // no user do not even try
        if (string.IsNullOrWhiteSpace(id))
        {
            return Task.FromResult(default(UserModel));
        }

        // try load from database
        return this.dataOps.Connect().QueryFirst("Identity.User", "GetById").ExecuteAsync<UserModel>(new
        {
            Id = id.ToLowerInvariant()
        });
    }

    /// <summary>
    /// Gets the user by email
    /// </summary>
    /// <param name="email">The user email</param>
    /// <returns></returns>
    public Task<UserModel> GetByEmail(string email)
    {
        // no user do not even try
        if (string.IsNullOrWhiteSpace(email))
        {
            return Task.FromResult(default(UserModel));
        }

        // try load from database
        return this.dataOps.Connect().QueryFirst("Identity.User", "GetByEmail").ExecuteAsync<UserModel>(new
        {
            Email = email.ToLowerInvariant()
        });
    }

    /// <summary>
    /// Gets the user profile by id
    /// </summary>
    /// <param name="id">The user id</param>
    /// <returns></returns>
    public Task<UserProfileModel> GetProfileById(string id)
    {
        // try load from database
        return this.dataOps.Connect().QueryFirst("Identity.User", "GetProfileById").ExecuteAsync<UserProfileModel>(new
        {
            Id = id
        });
    }

    /// <summary>
    /// Gets the user picture by id
    /// </summary>
    /// <param name="id">The user id</param>
    /// <returns></returns>
    public Task<UserPictureModel> GetPictureById(string id)
    {
        // try load from database
        return this.dataOps.Connect().QueryFirst("Identity.User", "GetPictureById").ExecuteAsync<UserPictureModel>(new
        {
            Id = id
        });
    }

    /// <summary>
    /// Gets the references to the groups the user is member of
    /// </summary>
    /// <param name="id">The id of user to request</param>
    /// <returns></returns>
    public Task<IEnumerable<UserGroupReferentialValueModel>> GetGroupsById(string id)
    {
        // try load from database
        return this.dataOps.Connect().Query("Identity.User", "GetGroupsById").ExecuteAsync<UserGroupReferentialValueModel>(new
        {
            Id = id
        });
    }

    /// <summary>
    /// Gets the references to the roles the user is member of.
    /// </summary>
    /// <param name="id">The id of user to request.</param>
    /// <returns></returns>
    public Task<IEnumerable<UserRoleReferentialValueModel>> GetRolesById(string id)
    {
        // try load from database
        return this.dataOps.Connect().Query("Identity.User", "GetRolesById").ExecuteAsync<UserRoleReferentialValueModel>(new
        {
            Id = id
        });
    }

    /// <summary>
    /// Creates entity based on input
    /// </summary>
    /// <param name="input">The creation input</param>
    /// <returns></returns>
    public async Task<UserUpdateResultModel> Create(UserCreateModel input)
    {
        // generate id if necessary
        input.Id ??= StdIdGenerator.Next(IdentityObjects.USER)?.ToLowerInvariant();

        // handle case sensitivity of identifiers
        input.Email = input.Email?.ToLowerInvariant();
        input.Username = input.Username?.ToLowerInvariant();

        // add user to the database
        return await this.dataOps.Connect().QueryFirst("Identity.User", "Create").ExecuteAsync<UserUpdateResultModel>(input);
    }

    /// <summary>
    /// Updates the user model by id
    /// </summary>
    /// <param name="id">The id of user</param>
    /// <param name="input">The user update input</param>
    /// <returns></returns>
    public Task<UserUpdateResultModel> UpdateById(string id, UserUpdateModel input)
    {
        input.Id = id;

        // handle case sensitivity of the identifiers
        input.Email = input.Email?.ToLowerInvariant();
        
        return this.dataOps.Connect().QueryFirst("Identity.User", "UpdateById").ExecuteAsync<UserUpdateResultModel>(input);
    }

    /// <summary>
    /// Updates the user type by id
    /// </summary>
    /// <param name="id">The id of user</param>
    /// <param name="userType">The type of user to assign</param>
    /// <returns></returns>
    public Task<UserUpdateResultModel> UpdateTypeById(string id, string userType)
    {
        return this.dataOps.Connect().QueryFirst("Identity.User", "UpdateTypeById").ExecuteAsync<UserUpdateResultModel>(
            new
            {
                Id = id?.ToLowerInvariant(),
                UserType = userType
            });
    }

    /// <summary>
    /// Updates the user state by id
    /// </summary>
    /// <param name="id">The id of user</param>
    /// <param name="userState">The state of user to assign</param>
    /// <returns></returns>
    public Task<UserUpdateResultModel> UpdateUserStateById(string id, string userState)
    {
        return this.dataOps.Connect().QueryFirst("Identity.User", "UpdateUserStateById")
            .ExecuteAsync<UserUpdateResultModel>(new
            {
                Id = id?.ToLowerInvariant(),
                UserState = userState
            });
    }

    /// <summary>
    /// Updates the user profile picture
    /// </summary>
    /// <param name="id">The id of user</param>
    /// <param name="input">The update input</param>
    /// <returns></returns>
    public Task<UserPictureModel> UpdatePictureById(string id, UserPictureUpdateModel input)
    {
        // assign id if not assigned
        input.Id = id;

        return this.dataOps.Connect().QueryFirst("Identity.User", "UpdatePictureById").ExecuteAsync<UserPictureModel>(input);
    }

    /// <summary>
    /// Updates the user profile info
    /// </summary>
    /// <param name="id">The id of user</param>
    /// <param name="input">The update input</param>
    /// <returns></returns>
    public Task<UserProfileModel> UpdateProfileById(string id, UserProfileUpdateModel input)
    {
        // assign id if not assigned
        input.Id = id;

        return this.dataOps.Connect().QueryFirst("Identity.User", "UpdateProfileById").ExecuteAsync<UserProfileModel>(input);
    }

    /// <summary>
    /// Deletes the user by id
    /// </summary>
    /// <param name="id">The user id</param>
    /// <returns></returns>
    public Task<UserModel> DeleteById(string id)
    {
        // the anonymous indicator
        var anon = $"deleted-{Guid.NewGuid():N}";

        return this.dataOps.Connect().QueryFirst("Identity.User", "DeleteById").ExecuteAsync<UserModel>(new
        {
            Id = id?.ToLowerInvariant(),
            Email = $"{anon}@example.com",
            Username = anon
        });
    }
}