using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Schema;
using Shoc.Core;
using Shoc.Package.Templating.Model;
using Shoc.Package.Templating.Modules;

namespace Shoc.Package.Services;

/// <summary>
/// The template provider
/// </summary>
public class TemplateProvider
{
    /// <summary>
    /// The template cache
    /// </summary>
    private readonly IEnumerable<TemplateDefinition> cache;
    
    /// <summary>
    /// Creates a new instance for the template provider
    /// </summary>
    /// <param name="localTemplateProvider">The local provider</param>
    public TemplateProvider(LocalTemplateProvider localTemplateProvider)
    {
        this.cache = localTemplateProvider.LoadAll().Result;
    }

    /// <summary>
    /// Gets all the template descriptors
    /// </summary>
    /// <returns></returns>
    public Task<IEnumerable<TemplateDescriptorModel>> GetAll()
    {
        return Task.FromResult(this.cache.Select(Map));
    }

    /// <summary>
    /// Gets the variants of the template
    /// </summary>
    /// <param name="name">The name of the template</param>
    /// <returns></returns>
    public Task<IEnumerable<TemplateVariantDefinition>> GetVariants(string name)
    {
        // get the template
        var template = this.cache.FirstOrDefault(item => item.Name == name);

        // template not found
        if (template == null)
        {
            throw ErrorDefinition.NotFound().AsException();
        }

        return Task.FromResult(template.Variants.Values.AsEnumerable());
    }

    /// <summary>
    /// Gets the variant of the template
    /// </summary>
    /// <param name="name">The name of the template</param>
    /// <param name="variant">The variant name</param>
    /// <returns></returns>
    public Task<TemplateVariantDefinition> GetVariant(string name, string variant)
    {
        // get the template
        var template = this.cache.FirstOrDefault(item => item.Name == name);

        // template not found
        if (template == null)
        {
            throw ErrorDefinition.NotFound().AsException();
        }

        // try finding the variant by name
        var result = template.Variants.TryGetValue(variant, out var variantDef) ? variantDef : null;
        
        // variant not found
        if (result == null)
        {
            throw ErrorDefinition.NotFound().AsException();
        }

        return Task.FromResult(result);
    }

    /// <summary>
    /// Gets the variant of the template
    /// </summary>
    /// <param name="name">The name of the template</param>
    /// <param name="variant">The variant name</param>
    /// <returns></returns>
    public async Task<JSchema> GetVariantBuildSpec(string name, string variant)
    {
        // get the template variant
        var templateVariant = await this.GetVariant(name, variant);

        // parse the schema
        return JSchema.Parse(templateVariant.BuildSpec, new LocalSchemaResolver());
    }

    /// <summary>
    /// Maps the template definition to template descriptor
    /// </summary>
    /// <param name="input">The input to map</param>
    /// <returns></returns>
    private static TemplateDescriptorModel Map(TemplateDefinition input)
    {
        return new TemplateDescriptorModel
        {
            Name = input.Name,
            Title = input.Title,
            Description = input.Description,
            Author = input.Author,
            Variants = input.Variants.Keys
        };
    }
}