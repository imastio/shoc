using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json.Schema;
using Shoc.Package.Templating.Model;

namespace Shoc.Package.Templating;

/// <summary>
/// The known schemas
/// </summary>
public class KnownSchemas
{
    /// <summary>
    /// The references fot all the schemas
    /// </summary>
    public static readonly Dictionary<Uri, JSchema> ALL = GetAll().ToDictionary(schema => schema.Id, schema => schema);
    
    /// <summary>
    /// Get all the schemas
    /// </summary>
    /// <returns></returns>
    private static IEnumerable<JSchema> GetAll()
    {
        // get the execution directory
        var sourceDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty;

        // get path to schemas directory
        var directory = Path.Combine(sourceDirectory, TemplatingConstants.SCHEMAS_DIRECTORY);
        
        // no such directory
        if (!Directory.Exists(directory))
        {
            return Enumerable.Empty<JSchema>();
        }
        
        // gets the directory info
        var info = new DirectoryInfo(directory);
        
        // return parsed json schemas for all the schemas
        return info.GetFiles()
            .Where(file => file.Name.EndsWith(TemplatingConstants.SCHEMA_EXTENSION))
            .Select(file => JSchema.Parse(File.ReadAllText(file.FullName)))
            .Where(schema => schema.Id != null);
    }
}