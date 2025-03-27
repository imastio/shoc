using System;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Options;

namespace Shoc.Identity.Config.Oidc;

/// <summary>
/// The dynamic implementation for getting oidc options over monitor
/// </summary>
public class DynamicOidcOptionsMonitor : IOptionsMonitor<OpenIdConnectOptions>
{
    /// <summary>
    /// The options factory
    /// </summary>
    private readonly IOptionsFactory<OpenIdConnectOptions> optionsFactory;

    /// <summary>
    /// Creates new instance of the options monitor
    /// </summary>
    /// <param name="optionsFactory">The options factory</param>
    public DynamicOidcOptionsMonitor(IOptionsFactory<OpenIdConnectOptions> optionsFactory)
    {
        this.optionsFactory = optionsFactory;
    }

    /// <summary>
    /// Gets the configuration
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public OpenIdConnectOptions Get(string name)
    {
        return this.optionsFactory.Create(name ?? string.Empty);
    }

    /// <summary>
    /// Handle the options change
    /// </summary>
    /// <param name="listener">The change listener</param>
    /// <returns></returns>
    public IDisposable OnChange(Action<OpenIdConnectOptions, string> listener)
    {
        return null;
    }
    
    /// <summary>
    /// Gets the current value of the options
    /// </summary>
    public OpenIdConnectOptions CurrentValue => this.Get(string.Empty);
}
