using System;
using System.Threading.Tasks;
using Duende.IdentityServer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using Shoc.Identity.Model.Flow;
using Shoc.Identity.Provider.Data;

namespace Shoc.Identity.Services;

/// <summary>
/// The sign-in handler implementation
/// </summary>
public class SigninHandler
{
    /// <summary>
    /// The number of maximum failed attempts
    /// </summary>
    private static readonly int MAX_FAILED_ATTEMPTS = 10;
        
    /// <summary>
    /// Lock the account for N minutes if maximum sign-in attempts are exceeded
    /// </summary>
    private static readonly TimeSpan MAX_ATTEMPTS_LOCKOUT_TIME = TimeSpan.FromMinutes(10);
    
    /// <summary>
    /// The internal user repository
    /// </summary>
    private readonly IUserInternalRepository userInternalRepository;

    /// <summary>
    /// The sign-in history repository
    /// </summary>
    private readonly ISigninHistoryRepository signinHistoryRepository;

    /// <summary>
    /// Creates new instance of sign-in handler
    /// </summary>
    /// <param name="userInternalRepository">The internal user repository</param>
    /// <param name="signinHistoryRepository">The sign-in history repository</param>
    public SigninHandler(IUserInternalRepository userInternalRepository, ISigninHistoryRepository signinHistoryRepository)
    {
        this.userInternalRepository = userInternalRepository;
        this.signinHistoryRepository = signinHistoryRepository;
    }

    /// <summary>
    /// The implementation of sign-in flow
    /// </summary>
    /// <param name="httpContext">The http context</param>
    /// <param name="principal">The principal to sign-in</param>
    public async Task Signin(HttpContext httpContext, SigninPrincipal principal)
    {
        // create a subject principal 
        var subject = new IdentityServerUser(principal.Subject)
        {
            DisplayName = principal.DisplayName,
            IdentityProvider = principal.Provider
        };
            
        // sign-in a persistent user with 14 days long session
        await httpContext.SignInAsync(subject, new AuthenticationProperties
        {
            IsPersistent = true,
            ExpiresUtc = DateTimeOffset.UtcNow.Add(TimeSpan.FromDays(14))
        });

        // gets the sign-in metadata
        var metadata = GetSignInMetadata(httpContext);
        
        // record successful attempt of user
        var user = await this.userInternalRepository.UpdateAttemptSuccessById(new SigninAttemptSuccessModel
        {
            Id = principal.Subject,
            Ip = metadata.Ip,
            Time = metadata.Time
        });

        // nothing to add, maybe no such user at all
        if (user == null)
        {
            return;
        }

        // record the history
        await this.signinHistoryRepository.Create(new SigninHistoryRecordModel
        {
            UserId = user.Id,
            SessionId = metadata.SessionId,
            Ip = metadata.Ip,
            Provider = principal.Provider,
            MethodType = principal.MethodType,
            MultiFactorType = principal.MultiFactorType,
            UserAgent = metadata.UserAgent,
            Time = metadata.Time,
            Refreshed = metadata.Time
        });
    }
    
    /// <summary>
    /// Record sign-in failure by given email
    /// </summary>
    /// <param name="email">The target email</param>
    /// <returns></returns>
    public async Task SigninFailed(string email)
    {
        // nothing to do
        if (string.IsNullOrWhiteSpace(email))
        {
            return;
        }

        // increment failed attempts
        var user = await this.userInternalRepository.UpdateAttemptFailureByEmail(email);

        // nothing to add, maybe no such user at all
        if (user == null)
        {
            return;
        }

        // more than maximum failed attempts
        if (user.FailedAttempts >= MAX_FAILED_ATTEMPTS)
        {
            await this.userInternalRepository.UpdateLockoutById(user.Id, DateTime.UtcNow.Add(MAX_ATTEMPTS_LOCKOUT_TIME));
        }
    }
    
    /// <summary>
    /// Gets the sign-in metadata
    /// </summary>
    /// <param name="httpContext">The HTTP context</param>
    /// <returns></returns>
    private static SignInMetadata GetSignInMetadata(HttpContext httpContext)
    {
        // try get user agent from request
        var userAgent = httpContext.Request.Headers.TryGetValue(HeaderNames.UserAgent, out var agent) ? agent.ToString() : "unknown";

        // get the IP address of the current connection
        var ip = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

        // build metadata
        return new SignInMetadata
        {
            Ip = ip,
            UserAgent = userAgent,
            Time = DateTime.UtcNow
        };
    }
}