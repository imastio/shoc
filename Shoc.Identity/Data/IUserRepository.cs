using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Shoc.Identity.Model;

namespace Shoc.Identity.Data
{
    /// <summary>
    /// The users repository
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Gets all the users
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<UserModel>> GetAll();

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
        /// Gets the password hash
        /// </summary>
        /// <param name="email">The email</param>
        /// <returns></returns>
        Task<string> GetPasswordHash(string email);

        /// <summary>
        /// Gets the root user
        /// </summary>
        /// <returns></returns>
        Task<UserModel> GetRootUser();

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
        /// Creates entity based on input
        /// </summary>
        /// <param name="input">The creation input</param>
        /// <returns></returns>
        Task<UserModel> Create(CreateUserModel input);

        /// <summary>
        /// Record failed attempt for the email
        /// </summary>
        /// <param name="email">The email</param>
        /// <returns></returns>
        Task<UserModel> AttemptFailed(string email);

        /// <summary>
        /// Record successful sign-in attempt
        /// </summary>
        /// <param name="input">The attempt input</param>
        /// <returns></returns>
        Task<UserModel> AttemptSuccess(AttemptSuccess input);

        /// <summary>
        /// Updates the password of the user
        /// </summary>
        /// <param name="id">The user id</param>
        /// <param name="passwordHash">The password hash</param>
        /// <returns></returns>
        Task<UserModel> ChangePassword(string id, string passwordHash);

        /// <summary>
        /// Updates the role of the user
        /// </summary>
        /// <param name="id">The user id</param>
        /// <param name="type">The type of user</param>
        /// <returns></returns>
        Task<UserModel> ChangeType(string id, string type);

        /// <summary>
        /// Locks the user until given time
        /// </summary>
        /// <param name="id">The user id</param>
        /// <param name="lockUntil">The lockout time</param>
        /// <returns></returns>
        Task<UserModel> Lockout(string id, DateTime lockUntil);

        /// <summary>
        /// Release the lockout of the user
        /// </summary>
        /// <param name="id">The user id</param>
        /// <returns></returns>
        Task<UserModel> ReleaseLockout(string id);

        /// <summary>
        /// Deletes the user by id
        /// </summary>
        /// <param name="id">The user id</param>
        /// <returns></returns>
        Task<UserModel> DeleteById(string id);
    }
}