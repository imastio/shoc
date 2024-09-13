using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Shoc.Settings.Model;

/// <summary>
/// The definitions for accesses of infrastructure service
/// </summary>
public class SettingsAccesses
{
    /// <summary>
    /// A read access to mailing profiles objects
    /// </summary>
    public const string SETTINGS_MAILING_PROFILES_READ = "settings:mailing_profiles:read";

    /// <summary>
    /// The list access to mailing profiles objects
    /// </summary>
    public const string SETTINGS_MAILING_PROFILES_LIST = "settings:mailing_profiles:list";

    /// <summary>
    /// A create access to mailing profiles objects
    /// </summary>
    public const string SETTINGS_MAILING_PROFILES_CREATE = "settings:mailing_profiles:create";
    
    /// <summary>
    /// An edit access to mailing profiles objects
    /// </summary>
    public const string SETTINGS_MAILING_PROFILES_EDIT = "settings:mailing_profiles:edit";
    
    /// <summary>
    /// A delete access to mailing profiles objects
    /// </summary>
    public const string SETTINGS_MAILING_PROFILES_DELETE = "settings:mailing_profiles:delete";
    
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
        return typeof(SettingsAccesses)
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(f => !f.IsInitOnly && f.IsLiteral && f.FieldType == typeof(string))
            .Select(f => f.GetRawConstantValue() as string)
            .ToHashSet();
    }
}