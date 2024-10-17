using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Shoc.Core;
using Shoc.Package.Model;
using Shoc.Package.Templating.Model;

namespace Shoc.Package.Services;

/// <summary>
/// The cached template provider
/// </summary>
public class LocalTemplateProvider
{
    /// <summary>
    /// The logger
    /// </summary>
    private readonly ILogger<LocalTemplateProvider> logger;

    /// <summary>
    /// Creates new instance of the local template provider
    /// </summary>
    /// <param name="logger">The logger instance</param>
    public LocalTemplateProvider(ILogger<LocalTemplateProvider> logger)
    {
        this.logger = logger;
    }

    /// <summary>
    /// loads all the template variants
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<TemplateDefinition>> LoadAll()
    {
        // results
        var result = new List<TemplateDefinition>();

        // iterate over all the directories
        foreach (var templatePath in Directory.GetDirectories(GetTemplatesDirectory()))
        {
            // the variants of the template
            var variants = new Dictionary<string, TemplateVariantDefinition>();
            
            // the template directory
            var templateDir = new DirectoryInfo(templatePath);
            
            // the name of the template
            var name = templateDir.Name;

            // template variant
            TemplateVariantDefinition defaultVariant;

            try
            {
                // load the default variant (in the template directory without nesting)
                defaultVariant = await Load(name, TemplatingConstants.DEFAULT_VARIANT, templateDir);
            }
            catch (Exception e)
            {
                this.logger.LogWarning($"Template initialization error due to default variant: {e.Message}");
                continue;
            }
            
            // add default variant
            variants[defaultVariant.Variant] = defaultVariant;

            // iterate over all the subdirectories
            foreach (var variantDir in templateDir.GetDirectories())
            {
                try
                {
                    // load the next variant
                    var variant = await Load(name, variantDir.Name, variantDir, defaultVariant);

                    // add variant to the variants
                    variants[variant.Variant] = variant;
                }
                catch (Exception e)
                {
                    this.logger.LogWarning($"Template variant initialization error: {e.Message}");
                }
            }
            
            // add to results
            result.Add(new TemplateDefinition
            {
                Name = name,
                Title = defaultVariant.Title,
                Description = defaultVariant.Description,
                Author = defaultVariant.Author,
                Variants = variants
            });
        }

        return result;
    }

    /// <summary>
    /// Loads a template variant with the given name from the given directory
    /// </summary>
    /// <param name="name">The name of the template</param>
    /// <param name="variant">The name of the variant</param>
    /// <param name="variantDir">The variant directory</param>
    /// <param name="fallback">The fallback values (null for default)</param>
    /// <returns></returns>
    private static async Task<TemplateVariantDefinition> Load(string name, string variant, DirectoryInfo variantDir, TemplateVariantDefinition fallback = null)
    {
        // get the metadata content
        var metadataContent = await LoadFileOrNull(Path.Combine(variantDir.FullName, TemplatingConstants.METADATA_FILE));

        // if metadata content is fetched, deserialize and use, otherwise use fallback data for creating a metadata
        var metadata = string.IsNullOrWhiteSpace(metadataContent) ? null : DeserializeMetadata(metadataContent);
        
        // get the runtime content
        var runtimeContent = await LoadFileOrNull(Path.Combine(variantDir.FullName, TemplatingConstants.RUNTIME_FILE));

        // if runtime is there try parsing the object
        var runtime = string.IsNullOrWhiteSpace(runtimeContent) ? null : DeserializeRuntime(runtimeContent);
        
        // get the build spec content
        var buildSpec = await LoadFileOrNull(Path.Combine(variantDir.FullName, TemplatingConstants.BUILD_SPEC_FILE));
        
        // get the containerfile content
        var containerfile = await LoadFileOrNull(Path.Combine(variantDir.FullName, TemplatingConstants.CONTAINERFILE_TEMPLATE_FILE));
        
        // get the overview content
        var overview = await LoadFileOrNull(Path.Combine(variantDir.FullName, TemplatingConstants.OVERVIEW_FILE));
        
        // get the specification content
        var specification = await LoadFileOrNull(Path.Combine(variantDir.FullName, TemplatingConstants.SPECIFICATION_FILE));
        
        // get the examples content
        var examples = await LoadFileOrNull(Path.Combine(variantDir.FullName, TemplatingConstants.EXAMPLES_FILE));
        
        // build the result
        var result = new TemplateVariantDefinition
        {
            Name = name,
            Variant = variant,
            Title = metadata?.Title ?? fallback?.Title,
            Description = metadata?.Description ?? fallback?.Description,
            Author = metadata?.Author ?? fallback?.Author,
            BuildSpec = buildSpec ?? fallback?.BuildSpec,
            Runtime = runtime ?? fallback?.Runtime ?? TemplateRuntimeModel.DEFAULT,
            Containerfile = containerfile ?? fallback?.Containerfile,
            OverviewMarkdown = overview ?? fallback?.OverviewMarkdown,
            SpecificationMarkdown = specification ?? fallback?.SpecificationMarkdown,
            ExamplesMarkdown = examples ?? fallback?.ExamplesMarkdown
        };
        
        // title is required
        if (string.IsNullOrWhiteSpace(result.Title))
        {
            throw ErrorDefinition.Validation(PackageErrors.TEMPLATE_INIT_ERROR).AsException($"Template: {name}:{variant} - Title is required");
        }
        
        // containerfile is required
        if (string.IsNullOrWhiteSpace(result.Containerfile))
        {
            throw ErrorDefinition.Validation(PackageErrors.TEMPLATE_INIT_ERROR).AsException($"Template: {name}:{variant} - Containerfile is required");
        }

        return result;
    }
    
    /// <summary>
    /// Deserialize the runtime json file into a model
    /// </summary>
    /// <param name="runtime">The runtime json</param>
    /// <returns></returns>
    private static TemplateRuntimeModel DeserializeRuntime(string runtime)
    {
        return JsonSerializer.Deserialize<TemplateRuntimeModel>(runtime, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
    }
    
    /// <summary>
    /// Deserialize the metadata json file into a model
    /// </summary>
    /// <param name="metadata">The metadata json</param>
    /// <returns></returns>
    private static TemplateMetadataModel DeserializeMetadata(string metadata)
    {
        return JsonSerializer.Deserialize<TemplateMetadataModel>(metadata, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
    }

    /// <summary>
    /// Loads the file with the give path or null if does not exist
    /// </summary>
    /// <param name="path">The path to the file</param>
    /// <returns></returns>
    private static async Task<string> LoadFileOrNull(string path)
    {
        // file does not exist
        if (!File.Exists(path))
        {
            return null;
        }

        return await File.ReadAllTextAsync(path);
    }
    
    /// <summary>
    /// Gets the templates directory
    /// </summary>
    /// <returns></returns>
    private static string GetTemplatesDirectory()
    {
        // get the execution directory
        var sourceDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty;

        return Path.Combine(sourceDirectory, TemplatingConstants.TEMPLATES_DIRECTORY);
    }
}