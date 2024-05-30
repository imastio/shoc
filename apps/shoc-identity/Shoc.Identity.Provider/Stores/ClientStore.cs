using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Stores;
using Shoc.Identity.Model;
using Shoc.Identity.Model.Application;
using Shoc.Identity.Model.OpenId;
using Shoc.Identity.Provider.Data;

namespace Shoc.Identity.Provider.Stores;

/// <summary>
/// The Client Store
/// </summary>
public class ClientStore : IClientStore
{
    /// <summary>
    /// The client repository
    /// </summary>
    private readonly IApplicationRepository applicationRepository;

    /// <summary>
    /// The in memory clients
    /// </summary>
    private readonly InMemoryClientsProvider inMemoryProvider;

    /// <summary>
    /// Creates new instance of hybrid client store
    /// </summary>
    /// <param name="applicationRepository">The application repository</param>
    /// <param name="inMemoryProvider">The set of built-in clients</param>
    public ClientStore(IApplicationRepository applicationRepository, InMemoryClientsProvider inMemoryProvider)
    {
        this.applicationRepository = applicationRepository;
        this.inMemoryProvider = inMemoryProvider;
    }

    /// <summary>
    /// Finds the client by id
    /// </summary>
    /// <param name="clientId">The client id</param>
    /// <returns></returns>
    public async Task<Client> FindClientByIdAsync(string clientId)
    {
        // try resolve from in memory store
        if (this.inMemoryProvider.TryGet(clientId, out var inMemoryClient))
        {
            return inMemoryClient;
        }
        
        // load from the repository
        var applicationClient = await this.applicationRepository.GetClientByClientId(clientId);

        // no client was found
        return applicationClient?.Application == null ? null : Build(applicationClient);
    }

    /// <summary>
    /// Maps the client model into the identity client
    /// </summary>
    /// <param name="client">The client model to map</param>
    /// <returns></returns>
    private static Client Build(ApplicationClientModel client)
    {
        // take the application
        var application = client.Application;
        
        // make uri list 
        var uriList = client.Uris.ToList();

        // compose the client
        return new Client
        {
            Enabled = application.Enabled,
            ClientId = application.ApplicationClientId,
            ProtocolType = application.ProtocolType,
            ClientSecrets = BuildSecrets(client.Secrets),
            RequireClientSecret = application.SecretRequired,
            ClientName = application.Name ?? application.ApplicationClientId,
            Description = application.Description,
            ClientUri = application.ApplicationUri,
            LogoUri = application.LogoUri,
            RequireConsent = application.ConsentRequired,
            AllowRememberConsent = application.AllowRememberConsent,
            AllowedGrantTypes = application.AllowedGrantTypes?.Split(' ', StringSplitOptions.RemoveEmptyEntries) ?? Array.Empty<string>(),
            RequirePkce = application.PkceRequired,
            AllowPlainTextPkce = application.AllowPlainTextPkce,
            RequireRequestObject = application.RequireRequestObject,
            AllowAccessTokensViaBrowser = application.AllowAccessTokensViaBrowser,
            RequireDPoP = application.DpopRequired,
            DPoPValidationMode = BuildDpopValidationMode(application.DpopValidationMode),
            DPoPClockSkew = TimeSpan.FromSeconds(application.DpopClockSkewSeconds),
            RedirectUris = BuildUris(uriList, ApplicationUriTypes.REDIRECT_URI),
            PostLogoutRedirectUris = BuildUris(uriList, ApplicationUriTypes.POST_LOGOUT_REDIRECT_URI),
            FrontChannelLogoutUri = application.FrontChannelLogoutUri,
            FrontChannelLogoutSessionRequired = application.FrontChannelLogoutSessionRequired,
            BackChannelLogoutUri = application.BackChannelLogoutUri,
            BackChannelLogoutSessionRequired = application.BackChannelLogoutSessionRequired,
            AllowOfflineAccess = application.AllowOfflineAccess,
            AllowedScopes = application.AllowedScopes?.Split(' ', StringSplitOptions.RemoveEmptyEntries) ?? Array.Empty<string>(),
            AlwaysIncludeUserClaimsInIdToken = application.AlwaysIncludeUserClaimsInIdToken,
            IdentityTokenLifetime = application.IdentityTokenLifetime,
            AllowedIdentityTokenSigningAlgorithms = application.AllowedIdentityTokenSigningAlgorithms?.Split(' ', StringSplitOptions.RemoveEmptyEntries) ?? Array.Empty<string>(),
            AccessTokenLifetime = application.AccessTokenLifetime,
            AuthorizationCodeLifetime = application.AuthorizationCodeLifetime,
            AbsoluteRefreshTokenLifetime = application.AbsoluteRefreshTokenLifetime,
            SlidingRefreshTokenLifetime = application.SlidingRefreshTokenLifetime,
            ConsentLifetime = application.ConsentLifetime == 0 ? null : application.ConsentLifetime,
            RefreshTokenUsage = BuildTokenUsage(application.RefreshTokenUsage),
            UpdateAccessTokenClaimsOnRefresh = application.UpdateAccessTokenClaimsOnRefresh,
            RefreshTokenExpiration = BuildTokenExpiration(application.RefreshTokenExpiration),
            AccessTokenType = BuildTokenType(application.AccessTokenType),
            EnableLocalLogin = application.EnableLocalLogin,
            IdentityProviderRestrictions = application.IdentityProviderRestrictions?.Split(' ', StringSplitOptions.RemoveEmptyEntries) ?? Array.Empty<string>(),
            IncludeJwtId = application.IncludeJwtId,
            Claims = BuildClaims(client.Claims),
            AlwaysSendClientClaims = application.AlwaysSendClientClaims,
            ClientClaimsPrefix = application.ClientClaimsPrefix,
            PairWiseSubjectSalt = application.PairWiseSubjectSalt,
            UserSsoLifetime = application.UserSsoLifetime == 0 ? null : application.UserSsoLifetime,
            UserCodeType = string.IsNullOrEmpty(application.UserCodeType) ? null : application.UserCodeType,
            DeviceCodeLifetime = application.DeviceCodeLifetime,
            CibaLifetime = application.CibaLifetime,
            PollingInterval = application.PollingInterval,
            CoordinateLifetimeWithUserSession = application.CoordinateLifetimeWithUserSession,
            AllowedCorsOrigins = BuildUris(uriList, ApplicationUriTypes.ORIGIN_URI),
            InitiateLoginUri = application.InitiateLoginUri,
            Properties = new Dictionary<string, string>()
        };
    }

    /// <summary>
    /// Build the set of claims
    /// </summary>
    /// <param name="claims">The claims to build</param>
    /// <returns></returns>
    private static ICollection<ClientClaim> BuildClaims(IEnumerable<ApplicationClaimModel> claims)
    {
        return claims.Select(claim => new ClientClaim
        {
            Type = claim.Type,
            Value = claim.Value,
            ValueType = string.IsNullOrWhiteSpace(claim.ValueType) ? ClaimValueTypes.String : claim.ValueType
        }).ToList();
    }
    
    /// <summary>
    /// Build URIs based on the type filter
    /// </summary>
    /// <param name="uris">The application uri models</param>
    /// <param name="type">The target type</param>
    /// <returns></returns>
    private static ICollection<string> BuildUris(IEnumerable<ApplicationUriModel> uris, string type)
    {
        return uris
            .Where(uri => string.Equals(uri.Type, type, StringComparison.InvariantCultureIgnoreCase))
            .Select(uri => uri.Uri)
            .ToList();
    }

    /// <summary>
    /// Build the secret collection for the client
    /// </summary>
    /// <param name="secrets">The list of application secrets</param>
    /// <returns></returns>
    private static ICollection<Secret> BuildSecrets(IEnumerable<ApplicationSecretModel> secrets)
    {
        return secrets.Select(appSecret => new Secret
        {
            Type = BuildSecretType(appSecret.Type),
            Description = appSecret.Description,
            Value = appSecret.Value,
            Expiration = appSecret.Expiration
        }).ToList();
    }

    /// <summary>
    /// Builds the DPoP validation mode
    /// </summary>
    /// <param name="mode">The mode to build</param>
    /// <returns></returns>
    private static DPoPTokenExpirationValidationMode BuildDpopValidationMode(string mode)
    {
        // on empty consider default
        if (string.IsNullOrWhiteSpace(mode))
        {
            return DPoPTokenExpirationValidationMode.Custom;
        }

        // parts
        var parts = mode.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        // the empty result at first
        var result = default(DPoPTokenExpirationValidationMode);

        // if contains "iat"
        if (parts.Contains(ApplicationDpopValidationModes.IAT))
        {
            result |= DPoPTokenExpirationValidationMode.Iat;
        }
        
        // if contains "nonce"
        if (parts.Contains(ApplicationDpopValidationModes.NONCE))
        {
            result |= DPoPTokenExpirationValidationMode.Nonce;
        }

        return result;
    }

    /// <summary>
    /// Builds the token usage
    /// </summary>
    /// <param name="usage">The usage string</param>
    /// <returns></returns>
    private static TokenUsage BuildTokenUsage(string usage)
    {
        return usage switch
        {
            TokenUsages.REUSE => TokenUsage.ReUse,
            TokenUsages.ONE_TIME => TokenUsage.OneTimeOnly,
            _ => TokenUsage.OneTimeOnly
        };
    }
    
    /// <summary>
    /// Builds the token expiration
    /// </summary>
    /// <param name="expiration">The expiration string</param>
    /// <returns></returns>
    private static TokenExpiration BuildTokenExpiration(string expiration)
    {
        return expiration switch
        {
            TokenExpirations.SLIDING => TokenExpiration.Sliding,
            TokenExpirations.ABSOLUTE => TokenExpiration.Absolute,
            _ => TokenExpiration.Absolute
        };
    }
    
    /// <summary>
    /// Builds the token type
    /// </summary>
    /// <param name="type">The type string</param>
    /// <returns></returns>
    private static AccessTokenType BuildTokenType(string type)
    {
        return type switch
        {
            AccessTokenTypes.JWT => AccessTokenType.Jwt,
            AccessTokenTypes.REFERENCE => AccessTokenType.Reference,
            _ => AccessTokenType.Jwt
        };
    }

    /// <summary>
    /// Converts the secret type
    /// </summary>
    /// <param name="type">The type of the secret</param>
    /// <returns></returns>
    private static string BuildSecretType(string type)
    {
        return type switch
        {
            KnownSecretTypes.CERT_NAME => IdentityServerConstants.SecretTypes.X509CertificateName,
            KnownSecretTypes.CERT_THUMBPRINT => IdentityServerConstants.SecretTypes.X509CertificateThumbprint,
            KnownSecretTypes.CERT_BASE64 => IdentityServerConstants.SecretTypes.X509CertificateBase64,
            KnownSecretTypes.JWK => IdentityServerConstants.SecretTypes.JsonWebKey,
            KnownSecretTypes.SHARED_SECRET => IdentityServerConstants.SecretTypes.SharedSecret,
            _ => IdentityServerConstants.SecretTypes.SharedSecret
        };
    }
}
