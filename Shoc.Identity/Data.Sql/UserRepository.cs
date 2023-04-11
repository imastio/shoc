using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Imast.DataOps.Api;
using Shoc.Core;
using Shoc.Identity.Model;

namespace Shoc.Identity.Data.Sql
{
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
        /// <returns></returns>
        public Task<IEnumerable<UserModel>> GetAll()
        {
            return this.dataOps.Connect().Query("User", "GetAll").ExecuteAsync<UserModel>();
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
            return this.dataOps.Connect().QueryFirst("User", "GetById").ExecuteAsync<UserModel>(new
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
            return this.dataOps.Connect().QueryFirst("User", "GetByEmail").ExecuteAsync<UserModel>(new
            {
                Email = email?.ToLowerInvariant()
            });
        }

        /// <summary>
        /// Gets the password hash
        /// </summary>
        /// <param name="email">The email</param>
        /// <returns></returns>
        public Task<string> GetPasswordHash(string email)
        {
            return this.dataOps.Connect().QueryFirst("User", "GetPasswordHash").ExecuteAsync<string>(new
            {
                Email = email?.ToLowerInvariant()
            });
        }

        /// <summary>
        /// Gets the root user
        /// </summary>
        /// <returns></returns>
        public Task<UserModel> GetRootUser()
        {
            return this.dataOps.Connect().QueryFirst("User", "GetRootUser").ExecuteAsync<UserModel>();
        }

        /// <summary>
        /// Checks if the email exists
        /// </summary>
        /// <param name="email">The email to check</param>
        /// <returns></returns>
        public Task<bool> CheckEmail(string email)
        {
            return this.dataOps.Connect().QueryFirst("User", "CheckEmail").ExecuteAsync<bool>(new { Email = email.ToLowerInvariant() });
        }

        /// <summary>
        /// Checks if the username exists
        /// </summary>
        /// <param name="username">The username to check</param>
        /// <returns></returns>
        public Task<bool> CheckUsername(string username)
        {
            return this.dataOps.Connect().QueryFirst("User", "CheckUsername").ExecuteAsync<bool>(new { Username = username.ToLowerInvariant() });
        }

        /// <summary>
        /// Creates entity based on input
        /// </summary>
        /// <param name="input">The creation input</param>
        /// <returns></returns>
        public Task<UserModel> Create(CreateUserModel input)
        {
            // generate id if necessary
            input.Id ??= StdIdGenerator.Next(IdentityObjects.USER)?.ToLowerInvariant();

            // handle case sensitivity of identifiers
            input.Email = input.Email?.ToLowerInvariant();
            input.Username = input.Username?.ToLowerInvariant();

            // add user to the database
            return this.dataOps.Connect().QueryFirst("User", "Create").ExecuteAsync<UserModel>(input);
        }

        /// <summary>
        /// Record failed attempt for the email
        /// </summary>
        /// <param name="email">The email</param>
        /// <returns></returns>
        public Task<UserModel> AttemptFailed(string email)
        {
            return this.dataOps.Connect().QueryFirst("User", "AttemptFailed").ExecuteAsync<UserModel>(new
            {
                Email = email.ToLowerInvariant()
            });
        }

        /// <summary>
        /// Record successful sign-in attempt
        /// </summary>
        /// <param name="input">The attempt input</param>
        /// <returns></returns>
        public Task<UserModel> AttemptSuccess(AttemptSuccess input)
        {
            // id lowercase
            input.Id = input.Id?.ToLowerInvariant();

            return this.dataOps.Connect().QueryFirst("User", "AttemptSuccess").ExecuteAsync<UserModel>(input);
        }

        /// <summary>
        /// Updates the password of the user
        /// </summary>
        /// <param name="id">The user id</param>
        /// <param name="passwordHash">The password hash</param>
        /// <returns></returns>
        public Task<UserModel> ChangePassword(string id, string passwordHash)
        {
            return this.dataOps.Connect().QueryFirst("User", "ChangePassword").ExecuteAsync<UserModel>(new
            {
                Id = id?.ToLowerInvariant(),
                PasswordHash = passwordHash
            });
        }

        /// <summary>
        /// Updates the role of the user
        /// </summary>
        /// <param name="id">The user id</param>
        /// <param name="type">The type of user</param>
        /// <returns></returns>
        public Task<UserModel> ChangeType(string id, string type)
        {
            return this.dataOps.Connect().QueryFirst("User", "ChangeType").ExecuteAsync<UserModel>(new
            {
                Id = id?.ToLowerInvariant(),
                Type = type
            });
        }

        /// <summary>
        /// Locks the user until given time
        /// </summary>
        /// <param name="id">The user id</param>
        /// <param name="lockUntil">The lockout time</param>
        /// <returns></returns>
        public Task<UserModel> Lockout(string id, DateTime lockUntil)
        {
            return this.dataOps.Connect().QueryFirst("User", "ChangeLockout").ExecuteAsync<UserModel>(new
            {
                Id = id?.ToLowerInvariant(),
                LockedUntil = lockUntil
            });
        }

        /// <summary>
        /// Release the lockout of the user
        /// </summary>
        /// <param name="id">The user id</param>
        /// <returns></returns>
        public Task<UserModel> ReleaseLockout(string id)
        {
            return this.dataOps.Connect().QueryFirst("User", "ChangeLockout").ExecuteAsync<UserModel>(new
            {
                Id = id?.ToLowerInvariant(),
                LockedUntil = default(DateTime?)
            });
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

            return this.dataOps.Connect().QueryFirst("User", "DeleteById").ExecuteAsync<UserModel>(new
            {
                Id = id?.ToLowerInvariant(),
                Email = $"{anon}@example.com",
                Username = anon
            });
        }
    }
}