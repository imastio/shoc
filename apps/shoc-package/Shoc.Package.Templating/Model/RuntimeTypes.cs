using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Shoc.Package.Templating.Model;

/// <summary>
/// The runtime types
/// </summary>
public class RuntimeTypes
{
    /// <summary>
    /// The function runtime
    /// </summary>
    public const string FUNCTION = "function";
    
    /// <summary>
    /// The MPI runtime
    /// </summary>
    public const string MPI = "mpi";
    
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
        return typeof(RuntimeTypes)
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(f => !f.IsInitOnly && f.IsLiteral && f.FieldType == typeof(string))
            .Select(f => f.GetRawConstantValue() as string)
            .ToHashSet();
    }
}