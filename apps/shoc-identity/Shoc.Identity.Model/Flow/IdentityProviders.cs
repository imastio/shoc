using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Shoc.Identity.Model.Flow;

/// <summary>
/// The known identity provider types
/// </summary>
public static class IdentityProviders
{
    /// <summary>
    /// The local identity provider
    /// </summary>
    public const string LOCAL = "local";

    /// <summary>
    /// The Google identity provider
    /// </summary>
    public const string GOOGLE = "google";

    /// <summary>
    /// The Facebook identity provider
    /// </summary>
    public const string FACEBOOK = "facebook";

    /// <summary>
    /// The Okta identity provider
    /// </summary>
    public const string OKTA = "okta";

    /// <summary>
    /// The other identity provider
    /// </summary>
    public const string OTHER = "other";
    
    /// <summary>
    /// Get and initialize all the constants
    /// </summary>
    public static readonly ISet<string> ALL = GetAll();
    
    /// <summary>
    /// Gets all the constant values
    /// </summary>
    /// <returns></returns>
    private static ISet<string> GetAll()
    {
        return typeof(IdentityProviders)
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(f => !f.IsInitOnly && f.IsLiteral && f.FieldType == typeof(string))
            .Select(f => f.GetRawConstantValue() as string)
            .ToHashSet();
    }
}