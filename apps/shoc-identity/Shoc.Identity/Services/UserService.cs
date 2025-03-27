using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Shoc.Core;
using Shoc.Core.Security;
using Shoc.Identity.Model;
using Shoc.Identity.Model.Flow;
using Shoc.Identity.Model.User;
using Shoc.Identity.Model.UserGroup;
using Shoc.Identity.Provider.Data;

namespace Shoc.Identity.Services;

/// <summary>
/// The user service 
/// </summary>
public class UserService : UserServiceBase
{
    /// <summary>
    /// The default page size
    /// </summary>
    private const int DEFAULT_PAGE_SIZE = 20;

    /// <summary>
    /// The regex to validate email
    /// </summary>
    private static readonly Regex EMAIL_REGEX = new("^(?(\")(\".+?(?<!\\\\)\"@)|(([0-9a-z]((\\.(?!\\.))|[-!#\\$%&'\\*\\+/=\\?\\^`\\{\\}\\|~\\w])*)(?<=[0-9a-z])@))(?(\\[)(\\[(\\d{1,3}\\.){3}\\d{1,3}\\])|(([0-9a-z][-\\w]*[0-9a-z]*\\.)+[a-z0-9][\\-a-z0-9]{0,22}[a-z0-9]))$");

    /// <summary>
    /// Number of tries to generate unique username
    /// </summary>
    private static readonly int MAX_USERNAME_TRIES = 10;
    
    /// <summary>
    /// The user internal repository
    /// </summary>
    private readonly IUserInternalRepository userInternalRepository;

    /// <summary>
    /// The sign-in history repository
    /// </summary>
    private readonly ISigninHistoryRepository signinHistoryRepository;

    /// <summary>
    /// The user password service
    /// </summary>
    private readonly UserPasswordService userPasswordService;

    /// <summary>
    /// The password hasher
    /// </summary>
    private readonly IPasswordHasher passwordHasher;

    /// <summary>
    /// Creates new instance of user service
    /// </summary>
    /// <param name="userRepository">The user repository</param>
    /// <param name="userInternalRepository">The user internal repository</param>
    /// <param name="signinHistoryRepository">The sign-in history repository</param>
    /// <param name="userPasswordService">The user password service</param>
    /// <param name="passwordHasher">The password hasher</param>
    public UserService(IUserRepository userRepository, IUserInternalRepository userInternalRepository,
        ISigninHistoryRepository signinHistoryRepository, UserPasswordService userPasswordService,
        IPasswordHasher passwordHasher) : base(userRepository)
    {
        this.userInternalRepository = userInternalRepository;
        this.signinHistoryRepository = signinHistoryRepository;
        this.userPasswordService = userPasswordService;
        this.passwordHasher = passwordHasher;
    }

    /// <summary>
    /// Gets all the users by filter
    /// </summary>
    /// <param name="filter">The filter</param>
    /// <returns></returns>
    public Task<IEnumerable<UserModel>> GetAll(UserFilterModel filter)
    {
        return this.userRepository.GetAll(filter);
    }

    /// <summary>
    /// Gets page of users by filter
    /// </summary>
    /// <param name="filter">The filter to apply</param>
    /// <param name="page">The page number</param>
    /// <param name="size">The page size</param>
    /// <returns></returns>
    public Task<UserPageResult> GetPageBy(UserFilterModel filter, int page, int? size)
    {
        return this.userRepository.GetPageBy(filter, Math.Abs(page), Math.Abs(size ?? DEFAULT_PAGE_SIZE));
    }

    /// <summary>
    /// Gets the referential values by the given filter
    /// </summary>
    /// <param name="filter">The filter to apply</param>
    /// <param name="page">The page number</param>
    /// <param name="size">The size</param>
    /// <returns></returns>
    public Task<UserReferentialValuePageResult> GetReferentialValuesPageBy(UserFilterModel filter, int page, int? size)
    {
        return this.userRepository.GetReferentialValuesPageBy(filter, page, size ?? DEFAULT_PAGE_SIZE);
    }

    /// <summary>
    /// Gets the user by email
    /// </summary>
    /// <param name="email">The user email</param>
    /// <returns></returns>
    public async Task<UserModel> GetByEmail(string email)
    {
        var result = await this.userRepository.GetByEmail(email);

        if (result == null)
        {
            throw ErrorDefinition.NotFound().AsException();
        }

        return result;
    }

    /// <summary>
    /// Gets the user by id
    /// </summary>
    /// <param name="id">The user id</param>
    /// <returns></returns>
    public Task<UserModel> GetById(string id)
    {
        return this.RequireById(id);
    }

    /// <summary>
    /// Gets the user profile by id
    /// </summary>
    /// <param name="id">The of user to request</param>
    /// <returns></returns>
    public async Task<UserProfileModel> GetProfileById(string id)
    {
        var result = await this.userRepository.GetProfileById(id);

        if (result == null)
        {
            throw ErrorDefinition.NotFound().AsException();
        }

        return result;
    }

    /// <summary>
    /// Gets the user by id
    /// </summary>
    /// <param name="id">The user id</param>
    /// <returns></returns>
    public async Task<UserPictureModel> GetPictureById(string id)
    {
        var result = await this.userRepository.GetPictureById(id);

        if (result == null)
        {
            throw ErrorDefinition.NotFound().AsException();
        }

        return result;
    }

    /// <summary>
    /// Gets page of sign-in history for user
    /// </summary>
    /// <param name="userId">The target user id</param>
    /// <param name="page">The page number</param>
    /// <param name="size">The page size</param>
    /// <returns></returns>
    public Task<SigninHistoryPageResult> GetSignInHistoryPage(string userId, int page, int? size)
    {
        return this.signinHistoryRepository.GetAll(userId, Math.Abs(page), Math.Abs(size ?? DEFAULT_PAGE_SIZE));
    }

    /// <summary>
    /// Gets the references to the groups the user is member of
    /// </summary>
    /// <param name="id">The id of user to request</param>
    /// <returns></returns>
    public async Task<IEnumerable<UserGroupReferentialValueModel>> GetGroupsById(string id)
    {
        // require the user to exist
        await this.GetById(id);

        // get result
        return await this.userRepository.GetGroupsById(id);
    }

    /// <summary>
    /// Gets the references to the roles the user is member of.
    /// </summary>
    /// <param name="id">The id of user to request.</param>
    /// <returns></returns>
    public async Task<IEnumerable<UserRoleReferentialValueModel>> GetRolesById(string id)
    {
        // require the user to exist
        await this.GetById(id);

        // get result
        return await this.userRepository.GetRolesById(id);
    }

    /// <summary>
    /// Creates entity based on input
    /// </summary>
    /// <param name="input">The creation input</param>
    /// <returns></returns>
    public async Task<UserUpdateResultModel> Create(UserCreateModel input)
    {
        // validate email
        ValidateEmail(input.Email);
        
        // validate full name
        ValidateFullName(input.FullName);

        // validate the password
        await this.userPasswordService.Validate(input.Password);

        // make sure user has state by default
        input.UserState ??= UserStates.ACTIVE;

        // ensure email is lowercase
        input.Email = input.Email?.ToLowerInvariant();

        // password hash is not given but plain password is given
        if (string.IsNullOrWhiteSpace(input.PasswordHash) && !string.IsNullOrWhiteSpace(input.Password))
        {
            input.PasswordHash = this.passwordHasher.Hash(input.Password).AsHash();
        }

        // checks if user already exists by the email
        var emailExists = await this.userInternalRepository.CheckEmail(input.Email);

        // email is taken
        if (emailExists)
        {
            throw ErrorDefinition.Validation(IdentityErrors.EXISTING_EMAIL).AsException();
        }

        // make sure we have username even if not given
        if (string.IsNullOrWhiteSpace(input.Username))
        {
            // try pick a username 
            input.Username = await this.PickUsername(input.Email);
        }

        // if username is given but used
        if (!string.IsNullOrWhiteSpace(input.Username) && await this.userInternalRepository.CheckUsername(input.Username))
        {
            throw ErrorDefinition.Validation(IdentityErrors.TAKEN_USERNAME).AsException();
        }

        // create the user
        return await this.userRepository.Create(input);
    }

    /// <summary>
    /// Updates the user by id
    /// </summary>
    /// <param name="id">The id of the user</param>
    /// <param name="input">The user update input</param>
    /// <returns></returns>
    public async Task<UserUpdateResultModel> UpdateById(string id, UserUpdateModel input)
    {
        // refer to a correct object
        input.Id = id;

        // validate the email
        ValidateEmail(input.Email);

        // validates the full name
        ValidateFullName(input.FullName);
        
        // get the user by id
        var existing = await this.GetById(id);

        // in case if the new email is different from the current one make sure it does not exist 
        if (!string.Equals(existing.Email, input.Email))
        {
            // check if new email exists
            var newEmailExists = await this.userInternalRepository.CheckEmail(input.Email);

            // new email already exists
            if (newEmailExists)
            {
                throw ErrorDefinition.Validation(IdentityErrors.EXISTING_EMAIL).AsException();
            }
        }

        // update in the storage
        var result = await this.userRepository.UpdateById(id, input);

        // make sure object exists
        if (result == null)
        {
            throw ErrorDefinition.NotFound().AsException();
        }

        return result;
    }
    
    /// <summary>
    /// Update the type of the given user
    /// </summary>
    /// <param name="id">The id of user to request</param>
    /// <param name="input">The type update model</param>
    /// <returns></returns>
    public async Task<UserUpdateResultModel> UpdateTypeById(string id, UserTypeUpdateModel input)
    {
        // make sure the user exists
        var user = await this.GetById(id);

        // disallow root upgrade or downgrade
        if (UserTypes.ROOT.Equals(user.Type) || UserTypes.ROOT.Equals(input.Type))
        {
            throw ErrorDefinition.Validation(IdentityErrors.INVALID_ROOT_TYPE).AsException();
        }

        // make sure the user type is recognized
        if (!UserTypes.ALL.Contains(input.Type))
        {
            throw ErrorDefinition.Validation(IdentityErrors.INVALID_USER_TYPE).AsException();
        }

        // update the type in the database
        return await this.userRepository.UpdateTypeById(id, input.Type);
    }

    /// <summary>
    /// Update the state of the given user
    /// </summary>
    /// <param name="id">The id of user to request</param>
    /// <param name="input">The user state update model</param>
    /// <returns></returns>
    public async Task<UserUpdateResultModel> UpdateUserStateById(string id, UserStateUpdateModel input)
    {
        // get the user 
        var user = await this.GetById(id);

        // disallow state change of root
        if (UserTypes.ROOT.Equals(user.Type))
        {
            throw ErrorDefinition.Validation(IdentityErrors.INVALID_ROOT_TYPE).AsException();
        }

        // make sure the user state is recognized
        if (!UserStates.ALL.Contains(input.UserState))
        {
            throw ErrorDefinition.Validation(IdentityErrors.INVALID_USER_STATE).AsException();
        }

        // update the type in the database
        return await this.userRepository.UpdateUserStateById(id, input.UserState);
    }

    /// <summary>
    /// Updates the user lockout
    /// </summary>
    /// <param name="id">The id of user</param>
    /// <param name="input">The lockout update model</param>
    /// <returns></returns>
    public async Task<UserUpdateResultModel> UpdateLockoutById(string id, UserLockoutUpdateModel input)
    {
        // get the user by id
        var user = await this.GetById(id);

        // disallow state change of root
        if (UserTypes.ROOT.Equals(user.Type))
        {
            throw ErrorDefinition.Validation(IdentityErrors.INVALID_ROOT_TYPE).AsException();
        }

        // update the lockout
        var updated = await this.userInternalRepository.UpdateLockoutById(id, input.LockedUntil);

        // if nothing was updated then not found
        if (updated == null)
        {
            throw ErrorDefinition.NotFound().AsException();
        }

        // return update result
        return new UserUpdateResultModel
        {
            Id = updated.Id,
            Updated = updated.Updated
        };
    }

    /// <summary>
    /// Update the profile picture of the given user
    /// </summary>
    /// <param name="id">The id of user to request</param>
    /// <param name="input">The picture update model</param>
    /// <returns></returns>
    public async Task<UserPictureModel> UpdatePictureById(string id, UserPictureUpdateModel input)
    {
        // refer to the correct object
        input.Id = id;
        
        // try update the object
        var result = await this.userRepository.UpdatePictureById(id, input);

        // make sure object exists
        if (result == null)
        {
            throw ErrorDefinition.NotFound().AsException();
        }
        
        return result;
    }

    /// <summary>
    /// Update the profile of the given user
    /// </summary>
    /// <param name="id">The id of user to request</param>
    /// <param name="input">The profile update model</param>
    /// <returns></returns>
    public async Task<UserProfileModel> UpdateProfileById(string id, UserProfileUpdateModel input)
    {
        // refer to the correct object
        input.Id = id;
       
        // update profile data in database
        return await this.userRepository.UpdateProfileById(id, input);
    }

    /// <summary>
    /// Updates the password by id
    /// </summary>
    /// <param name="id">The id of the user</param>
    /// <param name="input">The password update input</param>
    /// <returns></returns>
    public async Task<UserUpdateResultModel> UpdatePasswordById(string id, UserPasswordUpdateModel input)
    {
        // update the password
        var result = await this.userPasswordService.UpdatePasswordById(id, input);

        return new UserUpdateResultModel
        {
            Id = result.Id,
            Updated = result.Updated
        };
    }
    
    /// <summary>
    /// Deletes the user by id
    /// </summary>
    /// <param name="id">The id of user</param>
    /// <returns></returns>
    public async Task<UserModel> DeleteById(string id)
    {
        // get the user 
        var user = await this.userRepository.GetById(id);

        // make sure user exists
        if (user == null)
        {
            throw ErrorDefinition.NotFound().AsException();
        }

        // root cannot be deleted
        if (string.Equals(user.Type, UserTypes.ROOT))
        {
            throw ErrorDefinition.Validation(IdentityErrors.INVALID_ROOT_REMOVAL).AsException();
        }

        // deletes the user by id
        var deleted = await this.userRepository.DeleteById(id);

        if (deleted == null)
        {
            throw ErrorDefinition.NotFound().AsException();
        }

        return deleted;
    }

    /// <summary>
    /// Pick a username with certain login
    /// </summary>
    /// <param name="email">The email of user</param>
    /// <returns></returns>
    private async Task<string> PickUsername(string email)
    {
        // get first part of email and try as username
        var username = email.Split("@").First();

        // use postfix to make unique username
        var postfix = string.Empty;

        // try next username candidate
        for (var i = 0; i < MAX_USERNAME_TRIES; ++i)
        {
            // next candidate
            var candidate = $"{username}{postfix}";

            // check if candidate is taken
            if (!await this.userInternalRepository.CheckUsername(candidate))
            {
                return candidate;
            }

            // generate new postfix as taken
            postfix = $"-{Rnd.GetString(3)}";
        }

        // use guid if no option worked
        return $"{username}-{Guid.NewGuid():N}";
    }

    /// <summary>
    /// Validates the email
    /// </summary>
    /// <param name="email">The email</param>
    /// <returns></returns>
    private static void ValidateEmail(string email)
    {
        // checks if email is empty
        if (string.IsNullOrWhiteSpace(email))
        {
            throw ErrorDefinition.Validation(IdentityErrors.EMPTY_EMAIL).AsException();
        }

        // checks if email matches required pattern or not
        if (!EMAIL_REGEX.IsMatch(email))
        {
            throw ErrorDefinition.Validation(IdentityErrors.INVALID_EMAIL).AsException();
        }
    }
    
    /// <summary>
    /// Validates the full name
    /// </summary>
    /// <param name="fullName">The full name to validate</param>
    /// <returns></returns>
    private static void ValidateFullName(string fullName)
    {
        // checks if email is empty
        if (string.IsNullOrWhiteSpace(fullName))
        {
            throw ErrorDefinition.Validation().AsException();
        }
    }
}