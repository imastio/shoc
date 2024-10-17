using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

namespace Shoc.Package.Services;

/// <summary>
/// The template initialization service
/// </summary>
public class TemplateInitializationService
{
    /// <summary>
    /// The template provider
    /// </summary>
    private readonly TemplateProvider templateProvider;

    /// <summary>
    /// The template build spec generate
    /// </summary>
    /// <param name="templateProvider">The template </param>
    public TemplateInitializationService(TemplateProvider templateProvider)
    {
        this.templateProvider = templateProvider;
    }

    /// <summary>
    /// Generates a sample json based on the template name and variant build spec
    /// </summary>
    /// <param name="name">The template name</param>
    /// <param name="variant">The template variant</param>
    /// <returns></returns>
    public async Task<JObject> Generate(string name, string variant)
    {
        // gets the template variant
        var spec = await this.templateProvider.GetVariantBuildSpec(name, variant);

        // generate based on the schema string
        return await this.GenerateImpl(spec);
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