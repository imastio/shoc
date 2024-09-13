using Duende.IdentityServer.Services;
using Microsoft.Extensions.Logging;

namespace Shoc.Identity.Provider.Services;

/// <summary>
/// The CORS policy service
/// </summary>
public class CorsPolicyService : DefaultCorsPolicyService
{
    /// <summary>
    /// Creates policy service
    /// </summary>
    /// <param name="logger">The logger</param>
    public CorsPolicyService(ILogger<DefaultCorsPolicyService> logger) : base(logger)
    {
        this.AllowAll = true;
    }
}