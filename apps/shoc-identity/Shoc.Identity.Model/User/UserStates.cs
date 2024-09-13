using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Shoc.Identity.Model.User;

/// <summary>
/// The known user states
/// </summary>
public static class UserStates
{
    /// <summary>
    /// The user is in active state
    /// </summary>
    public const string ACTIVE = "active";

    /// <summary>
    /// The user is in inactive state
    /// </summary>
    public const string INACTIVE = "inactive";

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
        return typeof(UserTypes)
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(f => !f.IsInitOnly && f.IsLiteral && f.FieldType == typeof(string))
            .Select(f => f.GetRawConstantValue() as string)
            .ToHashSet();
    }
}