using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Shoc.Core;
using Shoc.Identity.Model.Application;
using Shoc.Identity.Provider.Data;

namespace Shoc.Identity.Services;

/// <summary>
/// The application service
/// </summary>
public class ApplicationService : ApplicationServiceBase
{
    /// <summary>
    /// Creates new instance of application service
    /// </summary>
    /// <param name="applicationRepository">The application repository</param>
    public ApplicationService(IApplicationRepository applicationRepository) : base(applicationRepository)
    {
    }

    /// <summary>
    /// Gets the applications
    /// </summary>
    /// <returns></returns>
    public Task<IEnumerable<ApplicationModel>> GetAll()
    {
        return this.applicationRepository.GetAll();
    }

    /// <summary>
    /// Gets the application by id
    /// </summary>
    /// <returns></returns>
    public Task<ApplicationModel> GetById(string id)
    {
        return this.RequireById(id);
    }

    /// <summary>
    /// Creates the object with given input
    /// </summary>
    /// <param name="input">The creation input</param>
    /// <returns></returns>
    public async Task<ApplicationModel> Create(ApplicationCreateModel input)
    {
        // assign client id if not given
        input.ApplicationClientId ??= Guid.NewGuid().ToString("D");
        
        // try get by client id
        var existsByClientId = await this.applicationRepository.GetByClientId(input.ApplicationClientId);
        
        // check if application id is free
        if (existsByClientId != null)
        {
            throw ErrorDefinition.Validation().AsException();
        }

        // create in the storage
        return await this.applicationRepository.Create(MapCreate(input));
    }

    /// <summary>
    /// Updates the object by id
    /// </summary>
    /// <param name="id">The object id</param>
    /// <param name="input">The update input</param>
    /// <returns></returns>
    public async Task<ApplicationModel> UpdateById(string id, ApplicationModel input)
    {
        // make sure referring to the proper object
        input.Id = id;

        // make sure object exists
        await this.RequireById(input.Id);
        
        // assign client id if not given
        input.ApplicationClientId ??= Guid.NewGuid().ToString("D");
        
        // try get by client id
        var existsByClientId = await this.applicationRepository.GetByClientId(input.ApplicationClientId);
        
        // check if application id is free
        if (existsByClientId != null && existsByClientId.Id != input.Id)
        {
            throw ErrorDefinition.Validation().AsException();
        }

        // update in the storage
        return await this.applicationRepository.UpdateById(id, input);
    }

    /// <summary>
    /// Deletes the object by id
    /// </summary>
    /// <param name="id">The id of the object</param>
    /// <returns></returns>
    public async Task<ApplicationModel> DeleteById(string id)
    {
        // delete the object
        var existing = await this.applicationRepository.DeleteById(id);

        // check if exists
        if (existing == null)
        {
            throw ErrorDefinition.NotFound().AsException();
        }
        
        return existing;
    }

    /// <summary>
    /// Maps the application create model into the application model
    /// </summary>
    /// <param name="input">The input to map</param>
    /// <returns></returns>
    private static ApplicationModel MapCreate(ApplicationCreateModel input)
    {
        return new ApplicationModel
        {
            Id = input.Id,
            Enabled = input.Enabled,
            ApplicationClientId = input.ApplicationClientId,
            ProtocolType = ApplicationDefaults.DEFAULT_PROTOCOL_TYPE,
            Name = input.Name ?? input.ApplicationClientId,
            Description = input.Description ?? string.Empty,
            SecretRequired = ApplicationDefaults.DEFAULT_SECRET_REQUIRED,
            ApplicationUri = string.Empty,
            LogoUri = string.Empty,
            ConsentRequired = ApplicationDefaults.DEFAULT_CONSENT_REQUIRED,
            AllowRememberConsent = ApplicationDefaults.DEFAULT_ALLOW_REMEMBER_CONSENT,
            AllowedGrantTypes = ApplicationDefaults.DEFAULT_ALLOWED_GRANT_TYPES,
            PkceRequired = ApplicationDefaults.DEFAULT_PKCE_REQUIRED,
            AllowPlainTextPkce = ApplicationDefaults.DEFAULT_ALLOW_PLAIN_TEXT_PKCE,
            RequireRequestObject = ApplicationDefaults.DEFAULT_REQUIRE_REQUEST_OBJECT,
            AllowAccessTokensViaBrowser = ApplicationDefaults.DEFAULT_ALLOW_ACCESS_TOKENS_VIA_BROWSER,
            DpopRequired = ApplicationDefaults.DEFAULT_DPOP_REQUIRED,
            DpopValidationMode = ApplicationDefaults.DEFAULT_DPOP_VALIDATION_MODE,
            DpopClockSkewSeconds = ApplicationDefaults.DEFAULT_DPOP_CLOCK_SKEW_SECONDS,
            FrontChannelLogoutUri = string.Empty,
            FrontChannelLogoutSessionRequired = ApplicationDefaults.DEFAULT_FRONT_CHANNEL_LOGOUT_SESSION_REQUIRED,
            BackChannelLogoutUri = string.Empty,
            BackChannelLogoutSessionRequired = ApplicationDefaults.DEFAULT_BACK_CHANNEL_LOGOUT_SESSION_REQUIRED,
            AllowOfflineAccess = ApplicationDefaults.DEFAULT_ALLOW_OFFLINE_ACCESS,
            AllowedScopes = ApplicationDefaults.DEFAULT_ALLOWED_SCOPES,
            AlwaysIncludeUserClaimsInIdToken = ApplicationDefaults.DEFAULT_INCLUDE_USER_CLAIMS_IN_ID_TOKEN,
            IdentityTokenLifetime = ApplicationDefaults.DEFAULT_IDENTITY_TOKEN_LIFETIME,
            AllowedIdentityTokenSigningAlgorithms = ApplicationDefaults.DEFAULT_ALLOWED_IDENTITY_TOKEN_SIGNING_ALGORITHMS,
            AccessTokenLifetime = ApplicationDefaults.DEFAULT_ACCESS_TOKEN_LIFETIME,
            AuthorizationCodeLifetime = ApplicationDefaults.DEFAULT_AUTHORIZATION_CODE_LIFETIME,
            AbsoluteRefreshTokenLifetime = ApplicationDefaults.DEFAULT_ABSOLUTE_REFRESH_TOKEN_LIFETIME,
            SlidingRefreshTokenLifetime = ApplicationDefaults.DEFAULT_SLIDING_REFRESH_TOKEN_LIFETIME,
            ConsentLifetime = ApplicationDefaults.DEFAULT_CONSENT_LIFETIME,
            RefreshTokenUsage = ApplicationDefaults.DEFAULT_REFRESH_TOKEN_USAGE,
            UpdateAccessTokenClaimsOnRefresh = ApplicationDefaults.DEFAULT_UPDATE_ACCESS_TOKEN_CLAIMS_ON_REFRESH,
            RefreshTokenExpiration = ApplicationDefaults.DEFAULT_REFRESH_TOKEN_EXPIRATION,
            AccessTokenType = ApplicationDefaults.DEFAULT_ACCESS_TOKEN_TYPE,
            EnableLocalLogin = ApplicationDefaults.DEFAULT_ENABLE_LOCAL_LOGIN,
            IdentityProviderRestrictions = string.Empty,
            IncludeJwtId = ApplicationDefaults.DEFAULT_INCLUDE_JWT_ID,
            AlwaysSendClientClaims = ApplicationDefaults.DEFAULT_ALWAYS_SEND_CLIENT_CLAIMS,
            ClientClaimsPrefix = string.Empty,
            PairWiseSubjectSalt = string.Empty,
            UserSsoLifetime = ApplicationDefaults.DEFAULT_USER_SSO_LIFETIME,
            UserCodeType = string.Empty,
            DeviceCodeLifetime = ApplicationDefaults.DEFAULT_DEVICE_CODE_LIFETIME,
            CibaLifetime = ApplicationDefaults.DEFAULT_CIBA_LIFETIME,
            PollingInterval = ApplicationDefaults.DEFAULT_POLLING_INTERVAL,
            CoordinateLifetimeWithUserSession = ApplicationDefaults.DEFAULT_COORDINATE_LIFETIME_WITH_USER_SESSION,
            InitiateLoginUri = string.Empty,
            Created = DateTime.UtcNow,
            Updated = DateTime.UtcNow
        };
    }
}