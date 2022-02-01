using System;
using System.Threading.Tasks;
using IdentityModel.OidcClient;
using IdentityModel.OidcClient.Browser;
using Shoc.Cli.OpenId;
using Shoc.Cli.System;
using Shoc.Core;

namespace Shoc.Cli.Services
{
    /// <summary>
    /// The authentication service
    /// </summary>
    public class AuthService
    {
        /// <summary>
        /// The encrypted storage
        /// </summary>
        private readonly EncryptedStorage encryptedStorage;

        /// <summary>
        /// The configuration service
        /// </summary>
        private readonly ConfigurationService configurationService;

        /// <summary>
        /// The network service
        /// </summary>
        private readonly NetworkService networkService;

        /// <summary>
        /// Creates new instance of authentication service
        /// </summary>
        /// <param name="encryptedStorage">The encrypted storage</param>
        /// <param name="configurationService">The configuration service</param>
        /// <param name="networkService">The network service</param>
        public AuthService(EncryptedStorage encryptedStorage, ConfigurationService configurationService, NetworkService networkService)
        {
            this.encryptedStorage = encryptedStorage;
            this.configurationService = configurationService;
            this.networkService = networkService;
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
            var browser = new SystemBrowser(this.networkService.GetNextAvailablePort());

            // build redirect uri
            var redirectUri = $"http://127.0.0.1:{browser.Port}";

            // init OIDC options
            var options = new OidcClientOptions
            {
                Authority = authority,
                ClientId = "native",
                RedirectUri = redirectUri,
                Browser = browser,
                Scope = "openid profile email",
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
                throw ErrorDefinition.Access(CliErrors.LOGIN_FAILED, "Could not sign-in").AsException();
            }

            // save tokens in encrypted form
            await this.encryptedStorage.Set(profile.Name, "at", result.AccessToken);
            await this.encryptedStorage.Set(profile.Name, "rt", result.RefreshToken);
        }

        /// <summary>
        /// Sign-in with given profile
        /// </summary>
        /// <param name="profileName">The name of profile</param>
        public async Task SignOut(string profileName)
        {
            // save tokens in encrypted form
            await this.encryptedStorage.Set(profileName, "at", string.Empty);
            await this.encryptedStorage.Set(profileName, "rt", string.Empty);
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