using System;
using Duende.IdentityServer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Options;
using Shoc.Identity.Services;

namespace Shoc.Identity.Config.Oidc;

/// <summary>
/// The dynamic implementation for getting oidc options
/// </summary>
public class DynamicOidcOptionsConfigurator : IConfigureNamedOptions<OpenIdConnectOptions>
{
    /// <summary>
    /// The provider accessor
    /// </summary>
    private readonly DynamicOidcProviderAccessor providerAccessor;
    
    /// <summary>
    /// The oidc provider service
    /// </summary>
    private readonly OidcProviderService oidcProviderService;
    
    /// <summary>
    /// The protection provider
    /// </summary>
    private readonly IdentityProviderProtectionProvider protectionProvider;
    
    /// <summary>
    /// Creates new instance of the options monitor
    /// </summary>
    /// <param name="providerAccessor">The provider accessor</param>
    /// <param name="oidcProviderService">The oidc provider service</param>
    /// <param name="protectionProvider">The protection provider for oidc providers</param>
    public DynamicOidcOptionsConfigurator(DynamicOidcProviderAccessor providerAccessor, OidcProviderService oidcProviderService, IdentityProviderProtectionProvider protectionProvider)
    {
        this.providerAccessor = providerAccessor;
        this.oidcProviderService = oidcProviderService;
        this.protectionProvider = protectionProvider;
    }

    /// <summary>
    /// The named configuration handler
    /// </summary>
    /// <param name="name">The name</param>
    /// <param name="options">The options</param>
    public void Configure(string name, OpenIdConnectOptions options)
    {
        // for the default handler do nothing
        if (!string.Equals(name, OidcProviderConstants.DYNAMIC_OIDC_SCHEME, StringComparison.Ordinal))
        {
            return;
        }
        
        // extract provider code from the request
        var providerCode = this.providerAccessor.Get();

        // ensure provider exists otherwise do nothing
        if (string.IsNullOrWhiteSpace(providerCode))
        {
            return;
        }

        // try to fetch provider dynamically
        var provider = this.oidcProviderService.GetByCodeOrNull(providerCode).GetAwaiter().GetResult();

        // provider is unknown 
        if (provider == null)
        {
            return;
        }

        // create a protector
        var protector = this.protectionProvider.Create();
        
        // build options
        options.Authority = provider.Authority;
        options.ClientId = provider.ClientId;
        options.ClientSecret = protector.Unprotect(provider.ClientSecretEncrypted);
        options.ResponseType = provider.ResponseType;
        options.GetClaimsFromUserInfoEndpoint = provider.FetchUserInfo;
        options.UsePkce = provider.Pkce;
        options.CallbackPath = $"{OidcProviderConstants.CALLBACK_PATH}/{providerCode}";
        options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
        options.SaveTokens = false;
        options.MapInboundClaims = false;
        options.DisableTelemetry = true;
        options.SkipUnrecognizedRequests = true;
        
        // clear scopes by default
        options.Scope.Clear();
        
        // add scopes
        foreach (var scope in provider.Scope.Split(' ', StringSplitOptions.RemoveEmptyEntries))
        {
            options.Scope.Add(scope);
        }
    }
    
    /// <summary>
    /// The configuring method for options without name
    /// </summary>
    /// <param name="options">The options to configure</param>
    public void Configure(OpenIdConnectOptions options)
    {
        // do nothing
    }
}
