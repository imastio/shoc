using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shoc.Core;
using Shoc.Core.Security;
using Shoc.Identity.Model;
using Shoc.Identity.Model.Flow;
using Shoc.Identity.Provider.Data;

namespace Shoc.Identity.Services;

/// <summary>
/// The credential evaluator service
/// </summary>
public class CredentialEvaluator
{
    /// <summary>
    /// The user internal repository
    /// </summary>
    private readonly IUserInternalRepository userInternalRepository;

    /// <summary>
    /// The otp repository
    /// </summary>
    private readonly IOtpRepository otpRepository;

    /// <summary>
    /// The password hasher
    /// </summary>
    private readonly IPasswordHasher passwordHasher;

    /// <summary>
    /// Creates new instance of credential evaluator
    /// </summary>
    /// <param name="userInternalRepository">The internal user repository</param>
    /// <param name="otpRepository">The otp repository</param>
    /// <param name="passwordHasher">The password hasher</param>
    public CredentialEvaluator(IUserInternalRepository userInternalRepository, IOtpRepository otpRepository, IPasswordHasher passwordHasher)
    {
        this.userInternalRepository = userInternalRepository;
        this.otpRepository = otpRepository;
        this.passwordHasher = passwordHasher;
    }

    /// <summary>
    /// Evaluates the given email and password for validity
    /// </summary>
    /// <param name="email">The email address</param>
    /// <param name="password">The password to validate</param>
    /// <returns></returns>
    public async Task<CredentialsEvaluationResult> Evaluate(string email, string password)
    {
        // validation result
        var errors = new List<ErrorDefinition>();

        // email is not given
        if (string.IsNullOrWhiteSpace(email))
        {
            errors.Add(ErrorDefinition.Validation(IdentityErrors.EMPTY_EMAIL));
        }

        // password is not given
        if (string.IsNullOrWhiteSpace(password))
        {
            errors.Add(ErrorDefinition.Validation(IdentityErrors.EMPTY_PASSWORD));
        }

        // break early if no valid credentials
        if (errors.Count > 0)
        {
            return new CredentialsEvaluationResult { Errors = errors };
        }

        // get the user by email
        var user = await this.userInternalRepository.GetByEmail(email);

        // no user by email
        if (user == null)
        {
            errors.Add(ErrorDefinition.Validation(IdentityErrors.INVALID_CREDENTIALS));
            return new CredentialsEvaluationResult { Errors = errors };
        }

        // the user is locked
        if (user.LockedUntil.HasValue && user.LockedUntil.Value > DateTime.UtcNow)
        {
            errors.Add(ErrorDefinition.Validation(IdentityErrors.USER_LOCKED));
            return new CredentialsEvaluationResult { Errors = errors };
        }

        // get the password hash
        var hash = await this.userInternalRepository.GetPasswordHashByEmail(email);

        // checks the hash against password
        var validation = this.passwordHasher.Check(hash ?? string.Empty, password);

        // indicates if one of OTPs matches
        var validOtp = default(OneTimePassModel);

        // the direct password does not match
        if (!validation.Verified)
        {
            // get user's one-time passwords that are still valid
            var otps = (await this.otpRepository.GetAll(user.Id)).Where(otp => otp.ValidUntil >= DateTime.UtcNow);

            // try find an OTP that matches to the given password
            validOtp = otps.FirstOrDefault(otp => this.passwordHasher.Check(otp.PasswordHash, password).Verified);
        }

        // does not match anyways
        if (!validation.Verified && validOtp == null)
        {
            errors.Add(ErrorDefinition.Validation(IdentityErrors.INVALID_CREDENTIALS));
        }

        // return validated result
        return new CredentialsEvaluationResult
        {
            Errors = errors,
            User = errors.Count > 0 ? null : user,
            Otp = validOtp
        };
    }
}