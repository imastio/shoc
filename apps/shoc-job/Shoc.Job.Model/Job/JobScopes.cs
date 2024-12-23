using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Shoc.Job.Model.Job;

/// <summary>
/// The job scopes
/// </summary>
public static class JobScopes
{
    /// <summary>
    /// The workspace scope 
    /// </summary>
    public const string WORKSPACE = "workspace";

    /// <summary>
    /// The user scope 
    /// </summary>
    public const string USER = "user";
    
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
        return typeof(JobScopes)
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(f => !f.IsInitOnly && f.IsLiteral && f.FieldType == typeof(string))
            .Select(f => f.GetRawConstantValue() as string)
            .ToHashSet();
    }
}