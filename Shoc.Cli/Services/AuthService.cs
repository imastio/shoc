using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using IdentityModel.OidcClient;
using IdentityModel.OidcClient.Browser;
using Shoc.Cli.Model;
using Shoc.Cli.OpenId;
using Shoc.Cli.Utility;
using Shoc.Core;

namespace Shoc.Cli.Services
{
    /// <summary>
    /// The authentication service
    /// </summary>
    public class AuthService
    {
        /// <summary>
        /// The refresh token key
        /// </summary>
        private const string REFRESH_TOKEN_KEY = "rt";

        /// <summary>
        /// The access token key
        /// </summary>
        private const string ACCESS_TOKEN_KEY = "at";

        /// <summary>
        /// The identity token key
        /// </summary>
        private const string IDENTITY_TOKEN_KEY = "it";

        /// <summary>
        /// The default scopes
        /// </summary>
        private const string DEFAULT_SCOPES = "openid profile email offline_access";

        /// <summary>
        /// The default client id
        /// </summary>
        private const string DEFAULT_CLIENT_ID = "native";

        /// <summary>
        /// The loopback prefix
        /// </summary>
        private const string LOOPBACK_PREFIX = "http://127.0.0.1";

        /// <summary>
        /// The token expiration threshold
        /// </summary>
        private static readonly TimeSpan TOKEN_EXPIRATION_THRESHOLD = TimeSpan.FromMinutes(2);

        /// <summary>
        /// The encrypted storage
        /// </summary>
        private readonly EncryptedStorage encryptedStorage;

        /// <summary>
        /// The configuration service
        /// </summary>
        private readonly ConfigurationService configurationService;
        
        /// <summary>
        /// Creates new instance of authentication service
        /// </summary>
        /// <param name="encryptedStorage">The encrypted storage</param>
        /// <param name="configurationService">The configuration service</param>
        public AuthService(EncryptedStorage encryptedStorage, ConfigurationService configurationService)
        {
            this.encryptedStorage = encryptedStorage;
            this.configurationService = configurationService;
        }

        /// <summary>
        /// Sign-in with given profile
        /// </summary>
        /// <param name="profileName">The name of profile</param>
        public async Task SignIn(string profileName)
        {
            // try load the profile
            var profile = await this.configurationService.GetProfile(profileName);

            // get authority
            var authority = await this.GetAuthority(profileName);

            // create new browser
            var browser = new SystemBrowser(Network.GetNextAvailablePort());

            // build redirect uri
            var redirectUri = $"{LOOPBACK_PREFIX}:{browser.Port}";

            // init OIDC options
            var options = new OidcClientOptions
            {
                Authority = authority,
                ClientId = DEFAULT_CLIENT_ID,
                RedirectUri = redirectUri,
                Browser = browser,
                Scope = DEFAULT_SCOPES,
                FilterClaims = false
            };

            // build new client
            var client = new OidcClient(options);

            // try login and get result back
            var result = await client.LoginAsync(new LoginRequest
            {
                BrowserTimeout = (int)TimeSpan.FromMinutes(5).TotalSeconds,
                BrowserDisplayMode = DisplayMode.Visible
            });

            // login error
            if (result.IsError)
            {
                await this.SignOut(profileName);
                throw ErrorDefinition.Access(CliErrors.LOGIN_FAILED, "Could not sign-in").AsException();
            }

            // save tokens in encrypted form
            await this.encryptedStorage.Set(profile.Name, ACCESS_TOKEN_KEY, result.AccessToken ?? string.Empty);
            await this.encryptedStorage.Set(profile.Name, REFRESH_TOKEN_KEY, result.RefreshToken ?? string.Empty);
            await this.encryptedStorage.Set(profile.Name, IDENTITY_TOKEN_KEY, result.IdentityToken ?? string.Empty);
        }

        /// <summary>
        /// Sign-in with given profile (silently)
        /// </summary>
        /// <param name="profileName">The name of profile</param>
        public async Task SignInSilent(string profileName)
        {
            // try load the profile
            var profile = await this.configurationService.GetProfile(profileName);

            // get authority
            var authority = await this.GetAuthority(profileName);
            
            // init OIDC options
            var options = new OidcClientOptions
            {
                Authority = authority,
                ClientId = DEFAULT_CLIENT_ID,
                Scope = DEFAULT_SCOPES,
                FilterClaims = false
            };

            // try get refresh token
            var refreshToken = await this.encryptedStorage.Get(profile.Name, REFRESH_TOKEN_KEY);

            // check if refresh token is valid and available
            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                await this.SignOut(profileName);
                throw ErrorDefinition.Access(CliErrors.LOGIN_FAILED, "The session is expired. Sign-in again!").AsException();
            }

            // build new client
            var client = new OidcClient(options);

            // try login and get result back
            var result = await client.RefreshTokenAsync(refreshToken);

            // login error
            if (result.IsError)
            {
                await this.SignOut(profileName);
                throw ErrorDefinition.Access(CliErrors.LOGIN_FAILED, "Could not extend the session. Please sign-in again!").AsException();
            }

            // save tokens in encrypted form
            await this.encryptedStorage.Set(profile.Name, ACCESS_TOKEN_KEY, result.AccessToken ?? string.Empty);
            await this.encryptedStorage.Set(profile.Name, REFRESH_TOKEN_KEY, result.RefreshToken ?? string.Empty);
            await this.encryptedStorage.Set(profile.Name, IDENTITY_TOKEN_KEY, result.IdentityToken ?? string.Empty);
        }

        /// <summary>
        /// Get current signed-in user
        /// </summary>
        /// <param name="profileName">The name of profile</param>
        public async Task<AuthStatus> GetStatus(string profileName)
        {
            // try load the profile
            var profile = await this.configurationService.GetProfile(profileName);
            
            // get access and identity tokens
            var accessToken = await this.encryptedStorage.Get(profile.Name, ACCESS_TOKEN_KEY);
            var identityToken = await this.encryptedStorage.Get(profile.Name, IDENTITY_TOKEN_KEY);

            // make sure we have token
            if (string.IsNullOrWhiteSpace(accessToken) || string.IsNullOrWhiteSpace(identityToken))
            {
                throw ErrorDefinition.Access(CliErrors.NOT_LOGGED_IN, "The user is not logged in").AsException();
            }

            // the jwt token handler
            var handler = new JwtSecurityTokenHandler();

            // the parsed access token
            var parsedAccessToken = handler.ReadJwtToken(accessToken);

            // the parsed identity token
            var parsedIdentityToken = handler.ReadJwtToken(identityToken);

            // return the current signed-in user
            return new AuthStatus
            {
                Id = parsedIdentityToken.Subject,
                Email = parsedIdentityToken.Claims.FirstOrDefault(c => c.Type == "email")?.Value,
                Name = parsedIdentityToken.Claims.FirstOrDefault(c => c.Type == "name")?.Value,
                Username = parsedIdentityToken.Claims.FirstOrDefault(c => c.Type== "preferred_username")?.Value,
                AccessToken = accessToken,
                SessionExpiration = parsedAccessToken.ValidTo
            };
        }

        /// <summary>
        /// Sign-in with given profile
        /// </summary>
        /// <param name="profileName">The name of profile</param>
        public async Task SignOut(string profileName)
        {
            // try load the profile
            var profile = await this.configurationService.GetProfile(profileName);

            // delete tokens in encrypted form
            await this.encryptedStorage.Remove(profile.Name, ACCESS_TOKEN_KEY);
            await this.encryptedStorage.Remove(profile.Name, REFRESH_TOKEN_KEY);
            await this.encryptedStorage.Remove(profile.Name, IDENTITY_TOKEN_KEY);
        }

        /// <summary>
        /// Do the given action with authorization of profile context
        /// </summary>
        /// <typeparam name="T">The result type</typeparam>
        /// <param name="profileName">The profile name</param>
        /// <param name="action">The action to execute</param>
        /// <returns></returns>
        public async Task<T> DoAuthorized<T>(string profileName, Func<ShocProfile, AuthStatus, Task<T>> action)
        {
            // get the profile
            var profile = await this.configurationService.GetProfile(profileName);

            // get current authenticated user
            var status = await this.GetStatus(profile.Name);

            // check if token has been expired or is about to expire
            if (DateTime.UtcNow.Add(TOKEN_EXPIRATION_THRESHOLD) > status.SessionExpiration)
            {
                // try silently re-login
                await this.SignInSilent(profile.Name);
            }

            // after re-login get the current session again
            status = await this.GetStatus(profile.Name);

            // invoke required protected action
            return await action(profile, status);
        }

        /// <summary>
        /// Gets the authority for the profile
        /// </summary>
        /// <param name="profileName">The profile name</param>
        /// <returns></returns>
        private async Task<string> GetAuthority(string profileName)
        {
            // try load the profile
            var profile = await this.configurationService.GetProfile(profileName);

            // check if authority is missing
            if (string.IsNullOrWhiteSpace(profile.Authority))
            {
                throw ErrorDefinition.Validation(CliErrors.INVALID_AUTHORITY, $"The authority is missing for the profile {profileName}").AsException();
            }

            return profile.Authority;
        }
    }
}