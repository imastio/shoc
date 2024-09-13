using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Shoc.Identity.Model.Flow;
using Shoc.Identity.Model.User;

namespace Shoc.Identity.Provider.Data;

/// <summary>
/// The users internal processing repository
/// </summary>
public interface IUserInternalRepository
{
    /// <summary>
    /// Gets all the internal users
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<UserInternalModel>> GetAll();

    /// <summary>
    /// Gets the internal user by id
    /// </summary>
    /// <param name="id">The id of the user</param>
    /// <returns></returns>
    Task<UserInternalModel> GetById(string id);

    /// <summary>
    /// Gets the internal user by email
    /// </summary>
    /// <param name="email">The email of the user</param>
    /// <returns></returns>
    Task<UserInternalModel> GetByEmail(string email);

    /// <summary>
    /// Gets the password hash by email
    /// </summary>
    /// <param name="email">The email</param>
    /// <returns></returns>
    Task<string> GetPasswordHashByEmail(string email);

    /// <summary>
    /// Gets the root user
    /// </summary>
    /// <returns></returns>
    Task<UserInternalModel> GetRoot();

    /// <summary>
    /// Checks if the email exists
    /// </summary>
    /// <param name="email">The email to check</param>
    /// <returns></returns>
    Task<bool> CheckEmail(string email);

    /// <summary>
    /// Checks if the username exists
    /// </summary>
    /// <param name="username">The username to check</param>
    /// <returns></returns>
    Task<bool> CheckUsername(string username);

    /// <summary>
    /// Updates the password of the user
    /// </summary>
    /// <param name="id">The user id</param>
    /// <param name="passwordHash">The password hash</param>
    /// <returns></returns>
    Task<UserInternalModel> UpdatePasswordById(string id, string passwordHash);

    /// <summary>
    /// Updates the lockout time by id 
    /// </summary>
    /// <param name="id">The id of the user</param>
    /// <param name="lockedUntil">The lockout time</param>
    /// <returns></returns>
    Task<UserInternalModel> UpdateLockoutById(string id, DateTime? lockedUntil);

    /// <summary>
    /// Confirms the user's emails
    /// </summary>
    /// <param name="id">The user id</param>
    /// <param name="emailVerified">The flag indicating if the email is verified</param>
    /// <returns></returns>
    Task<UserInternalModel> UpdateEmailVerifiedById(string id, bool emailVerified);

    /// <summary>
    /// Confirms the user's phone
    /// </summary>
    /// <param name="id">The user id</param>
    /// <param name="phoneVerified">The flag indicating if the phone is verified</param>
    /// <returns></returns>
    Task<UserInternalModel> UpdatePhoneVerifiedById(string id, bool phoneVerified);

    /// <summary>
    /// Record failed attempt for the email
    /// </summary>
    /// <param name="email">The email</param>
    /// <returns></returns>
    Task<UserInternalModel> UpdateAttemptFailureByEmail(string email);

    /// <summary>
    /// Record successful sign-in attempt
    /// </summary>
    /// <param name="input">The attempt input</param>
    /// <returns></returns>
    Task<UserInternalModel> UpdateAttemptSuccessById(SigninAttemptSuccessModel input);
}
