using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Shoc.Access.Data;
using Shoc.Core.OpenId;

namespace Shoc.ApiCore.Access;

/// <summary>
/// The access enrichment middleware
/// </summary>
public class AccessEnrichmentMiddleware
{
    /// <summary>
    /// The next request delegate
    /// </summary>
    private readonly RequestDelegate next;

    /// <summary>
    /// The access repository
    /// </summary>
    private readonly IAccessRepository accessRepository;

    /// <summary>
    /// Creates new instance of middleware
    /// </summary>
    /// <param name="next">The next delegate</param>
    /// <param name="accessRepository">The access repository</param>
    public AccessEnrichmentMiddleware(RequestDelegate next, IAccessRepository accessRepository)
    {
        this.next = next;
        this.accessRepository = accessRepository;
    }

    /// <summary>
    /// The implementation of middleware logic
    /// </summary>
    /// <param name="context">The current http context</param>
    public async Task InvokeAsync(HttpContext context)
    {
        // indicate if authenticated subject
        var subject = context.User.FindFirst(KnownClaims.SUBJECT)?.Value ?? string.Empty;
        
        // if user is authenticated and there is not yet access set assigned
        if (!string.IsNullOrWhiteSpace(subject) && !context.Items.ContainsKey("Accesses"))
        {
            // assign accesses in the context
            context.Items["Accesses"] = await this.GetAccesses(subject);
        }

        // Call the next delegate/middleware in the pipeline.
        await this.next(context);
    }

    /// <summary>
    /// Load accesses assigned to the user effectively
    /// </summary>
    /// <param name="userId">The user id</param>
    /// <returns></returns>
    private async Task<ISet<string>> GetAccesses(string userId)
    {
        // get all granted accesses
        var granted = await this.accessRepository.GetEffectiveByUser(userId);

        // make hash set of granted access modifiers
        return granted.Select(item => item.Access).ToHashSet();
    }
}