using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Shoc.Core;
using Imast.Ext.Core;
using Shoc.Core.Security;
using Shoc.Identity.Data;
using Shoc.Identity.Model;

namespace Shoc.Identity.Services
{
    /// <summary>
    /// The user service 
    /// </summary>
    public class UserService
    {
        /// <summary>
        /// The regex to validate email
        /// </summary>
        private static readonly Regex EMAIL_REGEX = new("^(?(\")(\".+?(?<!\\\\)\"@)|(([0-9a-z]((\\.(?!\\.))|[-!#\\$%&'\\*\\+/=\\?\\^`\\{\\}\\|~\\w])*)(?<=[0-9a-z])@))(?(\\[)(\\[(\\d{1,3}\\.){3}\\d{1,3}\\])|(([0-9a-z][-\\w]*[0-9a-z]*\\.)+[a-z0-9][\\-a-z0-9]{0,22}[a-z0-9]))$");

        /// <summary>
        /// The minimum length of password
        /// </summary>
        private static readonly int MIN_PASSWORD_LENGTH = 6;

        /// <summary>
        /// The number of maximum failed attempts
        /// </summary>
        private static readonly int MAX_FAILED_ATTEMPTS = 10;

        /// <summary>
        /// Number of tries to generate unique username
        /// </summary>
        private static readonly int MAX_USERNAME_TRIES = 10;

        /// <summary>
        /// Lock the account for N minutes if maximum sign-in attempts are exceeded
        /// </summary>
        private static readonly TimeSpan MAX_ATTEMPTS_LOCKOUT_TIME = TimeSpan.FromMinutes(10);

        /// <summary>
        /// The user repository
        /// </summary>
        private readonly IUserRepository userRepository;

        /// <summary>
        /// The confirmation code repository
        /// </summary>
        private readonly IConfirmationCodeRepository confirmationCodeRepository;

        /// <summary>
        /// The password hasher
        /// </summary>
        private readonly IPasswordHasher passwordHasher;

        /// <summary>
        /// Creates new instance of user service
        /// </summary>
        /// <param name="userRepository">The user repository</param>
        /// <param name="confirmationCodeRepository">The confirmation code repository</param>
        /// <param name="passwordHasher">The password hasher</param>
        public UserService(IUserRepository userRepository, IConfirmationCodeRepository confirmationCodeRepository, IPasswordHasher passwordHasher)
        {
            this.userRepository = userRepository;
            this.confirmationCodeRepository = confirmationCodeRepository;
            this.passwordHasher = passwordHasher;
        }

        /// <summary>
        /// Gets all the users
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<UserModel>> GetAll()
        {
            return this.userRepository.GetAll();
        }

        /// <summary>
        /// Gets the user by id
        /// </summary>
        /// <param name="id">The user id</param>
        /// <returns></returns>
        public Task<UserModel> GetById(string id)
        {
            return this.userRepository.GetById(id);
        }

        /// <summary>
        /// Gets the user by email
        /// </summary>
        /// <param name="email">The user email</param>
        /// <returns></returns>
        public Task<UserModel> GetByEmail(string email)
        {
            return this.userRepository.GetByEmail(email);
        }

        /// <summary>
        /// Gets the root user
        /// </summary>
        /// <returns></returns>
        public Task<UserModel> GetRootUser()
        {
            return this.userRepository.GetRootUser();
        }

        /// <summary>
        /// Creates entity based on input
        /// </summary>
        /// <param name="input">The creation input</param>
        /// <returns></returns>
        public async Task<UserModel> Create(CreateUserModel input)
        {
            // validate email
            var validate = RequireEmail(input.Email);

            // make sure passed email validation
            if (validate.Count > 0)
            {
                throw new ShocException(validate);
            }

            // make sure full name is set
            input.FullName = input.FullName.IsNotBlank() ? input.FullName : "New User";

            // name parts
            var nameParts = input.FullName.Split(" ");

            // make sure first name is set
            input.FirstName = input.FirstName.IsNotBlank() ? input.FirstName : nameParts.First();

            // make sure last name is set
            input.LastName = input.LastName.IsNotBlank() ? input.LastName : nameParts.Last();

            // default role if not given
            input.Type ??= UserTypes.USER;

            // no need to have duplicate name
            if (input.FirstName == input.LastName)
            {
                input.LastName = null;
            }

            // password hash is not given but plain password is given
            if (input.PasswordHash.IsBlank() && input.Password.IsNotBlank())
            {
                input.PasswordHash = this.passwordHasher.Hash(input.Password).AsHash();
            }

            // checks if user already exists by the email
            var emailExists = await this.userRepository.CheckEmail(input.Email);

            // email is taken
            if (emailExists)
            {
                throw ErrorDefinition.Validation(IdentityErrors.EXISTING_EMAIL).AsException();
            }

            // make sure we have username even if not given
            if (input.Username.IsBlank())
            {
                // try pick a username 
                input.Username = await this.PickUsername(input.Email);
            }

            // if username is given but used
            if (input.Username.IsNotBlank() && await this.userRepository.CheckUsername(input.Username))
            {
                throw ErrorDefinition.Validation(IdentityErrors.TAKEN_USERNAME).AsException();
            }

            // create the user
            return await this.userRepository.Create(input);
        }

        /// <summary>
        /// Record sign-in failure by given email
        /// </summary>
        /// <param name="email">The target email</param>
        /// <returns></returns>
        public async Task<UserModel> SignInFailed(string email)
        {
            // nothing to do
            if (string.IsNullOrWhiteSpace(email))
            {
                return null;
            }

            // increment failed attempts
            var user = await this.userRepository.AttemptFailed(email);

            // nothing to add, maybe no such user at all
            if (user == null)
            {
                return null;
            }

            // more than maximum failed attempts
            if (user.FailedAttempts >= MAX_FAILED_ATTEMPTS)
            {
                user = await this.userRepository.Lockout(user.Id, DateTime.UtcNow.Add(MAX_ATTEMPTS_LOCKOUT_TIME));
            }

            return user;
        }

        /// <summary>
        /// Record sign-in success for given principal
        /// </summary>
        /// <param name="principal">The target principal</param>
        /// <param name="metadata">The sign-in metadata</param>
        /// <returns></returns>
        public Task<UserModel> SignInSuccess(SignInPrincipal principal, SignInMetadata metadata)
        {
            // record successful attempt of user
            return this.userRepository.AttemptSuccess(new AttemptSuccess
            {
                Id = principal.Subject,
                Ip = metadata.Ip,
                Time = metadata.Time
            });
        }

        /// <summary>
        /// Evaluates user credentials
        /// </summary>
        /// <param name="email">The email</param>
        /// <param name="password">The password</param>
        /// <returns></returns>
        public async Task<ValidationResult<UserModel>> EvaluateCredentials(string email, string password)
        {
            // validation result
            var errors = new List<ErrorDefinition>();

            // email is not given
            if (email.IsBlank())
            {
                errors.Add(ErrorDefinition.Validation(IdentityErrors.EMPTY_EMAIL));
            }

            // password is not given
            if (password.IsBlank())
            {
                errors.Add(ErrorDefinition.Validation(IdentityErrors.EMPTY_PASSWORD));
            }

            // break early if no valid credentials
            if (errors.Count > 0)
            {
                return new ValidationResult<UserModel> { Errors = errors };
            }

            // get the user by email
            var user = await this.userRepository.GetByEmail(email);

            // no user by email
            if (user == null)
            {
                errors.Add(ErrorDefinition.Validation(IdentityErrors.INVALID_CREDENTIALS));
                return new ValidationResult<UserModel> { Errors = errors };
            }

            // the user is locked
            if (user.LockedUntil.HasValue && user.LockedUntil.Value > DateTime.UtcNow)
            {
                errors.Add(ErrorDefinition.Validation(IdentityErrors.USER_LOCKED));
                return new ValidationResult<UserModel> { Errors = errors };
            }

            // get the password hash
            var hash = await this.userRepository.GetPasswordHash(email);

            // checks the hash against password
            var validation = this.passwordHasher.Check(hash ?? string.Empty, password);
            
            // does not match anyways
            if (!validation.Verified)
            {
                errors.Add(ErrorDefinition.Validation(IdentityErrors.INVALID_CREDENTIALS));
            }

            // return validated result
            return new ValidationResult<UserModel>
            {
                Errors = errors,
                Value = errors.Count > 0 ? null : user
            };
        }

        /// <summary>
        /// Validates the email
        /// </summary>
        /// <param name="email">The email</param>
        /// <returns></returns>
        private static List<ErrorDefinition> RequireEmail(string email)
        {
            // the validation result
            var validation = new List<ErrorDefinition>();

            // checks if email is empty
            if (string.IsNullOrWhiteSpace(email))
            {
                validation.Add(ErrorDefinition.Validation(IdentityErrors.EMPTY_EMAIL));
            }

            // checks if email matches required pattern or not
            if (!EMAIL_REGEX.IsMatch(email ?? string.Empty))
            {
                validation.Add(ErrorDefinition.Validation(IdentityErrors.INVALID_EMAIL));
            }

            return validation;
        }

        /// <summary>
        /// Validates the email and password
        /// </summary>
        /// <param name="email">The email</param>
        /// <param name="password">The password</param>
        /// <returns></returns>
        public List<ErrorDefinition> ValidateEmailAndPassword(string email, string password)
        {
            // the validation result
            var validation = new List<ErrorDefinition>();

            // checks if password is empty
            if (password.IsBlank())
            {
                validation.Add(ErrorDefinition.Validation(IdentityErrors.EMPTY_PASSWORD));
            }

            // checks if password is too short
            if (password.Length < MIN_PASSWORD_LENGTH)
            {
                validation.Add(ErrorDefinition.Validation(IdentityErrors.WEAK_PASSWORD));
            }

            validation.AddRange(RequireEmail(email));

            return validation;
        }

        /// <summary>
        /// Gets active confirmations
        /// </summary>
        /// <param name="email">The email</param>
        /// <returns></returns>
        public Task<IEnumerable<ConfirmationCode>> GetConfirmations(string email)
        {
            return this.confirmationCodeRepository.GetByEmail(email);
        }

        /// <summary>
        /// Gets the confirmation code by link
        /// </summary>
        /// <param name="link">The link fragment</param>
        /// <returns></returns>
        public Task<ConfirmationCode> GetConfirmationByLink(string link)
        {
            return this.confirmationCodeRepository.GetByLink(link);
        }

        /// <summary>
        /// Create a confirmation code
        /// </summary>
        /// <param name="input">The code input</param>
        /// <returns></returns>
        public Task<ConfirmationCode> CreateConfirmation(ConfirmationCode input)
        {
            return this.confirmationCodeRepository.Create(input);
        }

        /// <summary>
        /// Delete confirmation code by id
        /// </summary>
        /// <param name="id">The code id</param>
        /// <returns></returns>
        public Task<int> DeleteConfirmation(string id)
        {
            return this.confirmationCodeRepository.DeleteById(id);
        }

        /// <summary>
        /// Delete confirmation codes for the email
        /// </summary>
        /// <param name="email">The target to confirm</param>
        /// <returns></returns>
        public Task<int> DeleteConfirmations(string email)
        {
            return this.confirmationCodeRepository.DeleteByEmail(email);
        }

        /// <summary>
        /// Confirms the email address for the given user
        /// </summary>
        /// <param name="userId">The user id</param>
        /// <returns></returns>
        public async Task<UserModel> ConfirmUserEmail(string userId)
        {
            // mark confirmed
            var user = await this.userRepository.ConfirmUserEmail(userId);

            // delete confirmations 
            var _ = await this.DeleteConfirmations(user.Email);

            return user;
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
                if (!await this.userRepository.CheckUsername(candidate))
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
        /// Deletes a entity with the given id
        /// </summary>
        /// <param name="id">The user id</param>
        /// <returns></returns>
        public async Task<UserModel> DeleteById(string id)
        {
            // delete from repository
            return await this.userRepository.DeleteById(id);
        }
    }
}