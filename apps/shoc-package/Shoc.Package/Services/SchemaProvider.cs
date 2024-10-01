using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Shoc.Core;

namespace Shoc.Package.Services;

/// <summary>
/// The schema provider service
/// </summary>
public class SchemaProvider
{
    /// <summary>
    /// Gets the schema or null if not found
    /// </summary>
    /// <param name="name">The name of the schema</param>
    /// <returns></returns>
    public async Task<string> GetSchemaOrNull(string name)
    {
        // get the file path
        var path = GetSchemaPath(name);

        // file does not exist
        if (!Path.Exists(path))
        {
            return null;
        }

        // read the file contents
        return await File.ReadAllTextAsync(path);
    }
    
    /// <summary>
    /// Gets the schema or null if not found
    /// </summary>
    /// <param name="name">The name of the schema</param>
    /// <returns></returns>
    public async Task<string> GetSchema(string name)
    {
        // try getting the schema contents
        var result = await this.GetSchemaOrNull(name);

        // ensure schema exists
        if (result == null)
        {
            throw ErrorDefinition.NotFound().AsException();
        }

        return result;
    }
    
    /// <summary>
    /// Gets the path of the schema file based on the name
    /// </summary>
    /// <param name="name">The schema name</param>
    /// <returns></returns>
    private static string GetSchemaPath(string name)
    {
        // get the execution directory
        var sourceDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty;

        return Path.Combine(sourceDirectory, "Schemas", $"{name}.json");
    }
}