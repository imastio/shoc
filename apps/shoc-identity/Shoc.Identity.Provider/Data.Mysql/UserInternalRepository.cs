using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Imast.DataOps.Api;
using Shoc.Identity.Model.Flow;
using Shoc.Identity.Model.User;

namespace Shoc.Identity.Provider.Data.Mysql;

/// <summary>
/// The user internal repository
/// </summary>
public class UserInternalRepository : IUserInternalRepository
{
    /// <summary>
    /// The data operations instance
    /// </summary>
    private readonly DataOperations dataOps;

    /// <summary>
    /// Creates new instance of repository implementation
    /// </summary>
    /// <param name="dataOps">A DataOps instance</param>
    public UserInternalRepository(DataOperations dataOps)
    {
        this.dataOps = dataOps;
    }
    
    /// <summary>
    /// Gets all the internal users
    /// </summary>
    /// <returns></returns>
    public Task<IEnumerable<UserInternalModel>> GetAll()
    {
        return this.dataOps.Connect().Query("Identity.User.Internal", "GetAll").ExecuteAsync<UserInternalModel>();
    }
    
    /// <summary>
    /// Gets the internal user by id
    /// </summary>
    /// <param name="id">The id of the user</param>
    /// <returns></returns>
    public Task<UserInternalModel> GetById(string id)
    {
        return this.dataOps.Connect().QueryFirst("Identity.User.Internal", "GetById").ExecuteAsync<UserInternalModel>(new
        {
            Id = id
        });
    }
    
    /// <summary>
    /// Gets the internal user by email
    /// </summary>
    /// <param name="email">The email of the user</param>
    /// <returns></returns>
    public Task<UserInternalModel> GetByEmail(string email)
    {
        return this.dataOps.Connect().QueryFirst("Identity.User.Internal", "GetByEmail").ExecuteAsync<UserInternalModel>(new
        {
            Email = email?.ToLowerInvariant()
        });
    }
    
    /// <summary>
    /// Gets the password hash by email
    /// </summary>
    /// <param name="email">The email</param>
    /// <returns></returns>
    public Task<string> GetPasswordHashByEmail(string email)
    {
        return this.dataOps.Connect().QueryFirst("Identity.User.Internal", "GetPasswordHashByEmail").ExecuteAsync<string>(new
        {
            Email = email?.ToLowerInvariant()
        });
    }
    
    /// <summary>
    /// Gets the root user
    /// </summary>
    /// <returns></returns>
    public Task<UserInternalModel> GetRoot()
    {
        return this.dataOps.Connect().QueryFirst("Identity.User.Internal", "GetRoot").ExecuteAsync<UserInternalModel>();
    }
    
    /// <summary>
    /// Checks if the email exists
    /// </summary>
    /// <param name="email">The email to check</param>
    /// <returns></returns>
    public Task<bool> CheckEmail(string email)
    {
        return this.dataOps.Connect().QueryFirst("Identity.User.Internal", "CheckEmail").ExecuteAsync<bool>(new
        {
            Email = email.ToLowerInvariant()
        });
    }

    /// <summary>
    /// Checks if the username exists
    /// </summary>
    /// <param name="username">The username to check</param>
    /// <returns></returns>
    public Task<bool> CheckUsername(string username)
    {
        return this.dataOps.Connect().QueryFirst("Identity.User.Internal", "CheckUsername").ExecuteAsync<bool>(new
        {
            Username = username.ToLowerInvariant()
        });
    }

    /// <summary>
    /// Updates the password of the user
    /// </summary>
    /// <param name="id">The user id</param>
    /// <param name="passwordHash">The password hash</param>
    /// <returns></returns>
    public Task<UserInternalModel> UpdatePasswordById(string id, string passwordHash)
    {
        return this.dataOps.Connect().QueryFirst("Identity.User.Internal", "UpdatePasswordById").ExecuteAsync<UserInternalModel>(new
        {
            Id = id,
            PasswordHash = passwordHash
        });
    }

    /// <summary>
    /// Updates the lockout time by id 
    /// </summary>
    /// <param name="id">The id of the user</param>
    /// <param name="lockedUntil">The lockout time</param>
    /// <returns></returns>
    public Task<UserInternalModel> UpdateLockoutById(string id, DateTime? lockedUntil)
    {
        return this.dataOps.Connect().QueryFirst("Identity.User.Internal", "UpdateLockoutById").ExecuteAsync<UserInternalModel>(new
        {
            Id = id,
            LockedUntil = lockedUntil
        });
    }

    /// <summary>
    /// Confirms the user's emails
    /// </summary>
    /// <param name="id">The user id</param>
    /// <param name="emailVerified">The flag indicating if the email is verified</param>
    /// <returns></returns>
    public Task<UserInternalModel> UpdateEmailVerifiedById(string id, bool emailVerified)
    {
        return this.dataOps.Connect().QueryFirst("Identity.User.Internal", "UpdateEmailVerifiedById").ExecuteAsync<UserInternalModel>(new
        {
            Id = id,
            EmailVerified = emailVerified
        });
    }

    /// <summary>
    /// Confirms the user's phone
    /// </summary>
    /// <param name="id">The user id</param>
    /// <param name="phoneVerified">The flag indicating if the phone is verified</param>
    /// <returns></returns>
    public Task<UserInternalModel> UpdatePhoneVerifiedById(string id, bool phoneVerified)
    {
        return this.dataOps.Connect().QueryFirst("Identity.User.Internal", "UpdatePhoneVerifiedById").ExecuteAsync<UserInternalModel>(new
        {
            Id = id,
            PhoneVerified = phoneVerified
        });
    }
    
    /// <summary>
    /// Record successful sign-in attempt
    /// </summary>
    /// <param name="input">The attempt input</param>
    /// <returns></returns>
    public Task<UserInternalModel> UpdateAttemptSuccessById(SigninAttemptSuccessModel input)
    {
        // id lowercase
        input.Id = input.Id?.ToLowerInvariant();

        return this.dataOps.Connect().QueryFirst("Identity.User.Internal", "UpdateAttemptSuccessById").ExecuteAsync<UserInternalModel>(input);
    }

    /// <summary>
    /// Record failed attempt for the email
    /// </summary>
    /// <param name="email">The email</param>
    /// <returns></returns>
    public Task<UserInternalModel> UpdateAttemptFailureByEmail(string email)
    {
        return this.dataOps.Connect().QueryFirst("Identity.User.Internal", "UpdateAttemptFailureByEmail").ExecuteAsync<UserInternalModel>(new
        {
            Email = email.ToLowerInvariant()
        });
    }

    
}