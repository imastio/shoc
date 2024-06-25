using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Shoc.ObjectAccess.Model;

/// <summary>
/// The known evaluation result codes
/// </summary>
public class EvaluationResultCodes
{
    /// <summary>
    /// Access granted
    /// </summary>
    public const string ACCESS_GRANTED = "access_granted";
    
    /// <summary>
    /// Access denied
    /// </summary>
    public const string ACCESS_DENIED = "access_denied";
    
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
        return typeof(EvaluationResultCodes)
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(f => !f.IsInitOnly && f.IsLiteral && f.FieldType == typeof(string))
            .Select(f => f.GetRawConstantValue() as string)
            .ToHashSet();
    }
}