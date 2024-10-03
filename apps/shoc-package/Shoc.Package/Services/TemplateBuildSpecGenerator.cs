using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Shoc.Core;
using Shoc.Package.Templating.Model;
using Shoc.Package.Templating.Modules;

namespace Shoc.Package.Services;

/// <summary>
/// The JSON example generator
/// </summary>
public class TemplateBuildSpecGenerator
{
    /// <summary>
    /// Generates a sample json based on the template name and variant build spec
    /// </summary>
    /// <param name="name">The template name</param>
    /// <param name="variant">The template variant</param>
    /// <returns></returns>
    public async Task<JObject> Generate(string name, string variant)
    {
        // get the execution directory
        var sourceDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty;

        // the templates directory
        var templates = Path.Combine(sourceDirectory, TemplatingConstants.TEMPLATES_DIRECTORY);
        
        // the target file path
        var path = Path.Combine(templates, name, variant, TemplatingConstants.BUILD_SPEC_FILE);
        
        // the build spec file for the given template and variant
        var buildSpec = new FileInfo(path);

        // ensure exists
        if (!buildSpec.Exists)
        {
            throw ErrorDefinition.NotFound().AsException();
        }
        
        // read the file and return
        var schema = JSchema.Parse(await File.ReadAllTextAsync(path), new LocalSchemaResolver());

        // generate based on the schema string
        return await this.GenerateImpl(schema);
    }

    /// <summary>
    /// The implementation of generate logic
    /// </summary>
    /// <param name="schema">The schema</param>
    /// <returns></returns>
    private async Task<JObject> GenerateImpl(JSchema schema)
    {
        // new empty object
        var result = new JObject();
        
        // handle each property separately
        foreach (var (name, subSchema) in schema.Properties)
        {
            result.Add(name, await this.GenerateDefaultValue(subSchema));
        }

        // return the result
        return result;
    }
    
    /// <summary>
    /// Generates a default value for the given schema
    /// </summary>
    /// <param name="schema">The schema to use</param>
    /// <returns></returns>
    private async Task<JToken> GenerateDefaultValue(JSchema schema)
    {
        // if default is given, use default
        if (schema.Default != null)
        {
            return schema.Default;
        }

        return schema.Type switch
        {
            JSchemaType.String => JValue.CreateString(string.Empty),
            JSchemaType.Integer => new JValue(0),
            JSchemaType.Number => new JValue(0.0),
            JSchemaType.Boolean => new JValue(false),
            JSchemaType.Object => await this.GenerateImpl(schema),
            JSchemaType.Array => new JArray(),
            _ => JValue.CreateNull()
        };
    }
}