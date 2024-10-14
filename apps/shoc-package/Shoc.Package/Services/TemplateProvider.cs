using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
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
    /// Gets all the template definitions
    /// </summary>
    /// <returns></returns>
    public Task<IEnumerable<TemplateModel>> GetAll()
    {
        // the root directory
        var directory = GetTemplatesDirectory();

        // no such directory
        if (!Directory.Exists(directory))
        {
            return Task.FromResult(Enumerable.Empty<TemplateModel>());
        }

        // the directory info
        var info = new DirectoryInfo(directory);

        // map sub-directories and return result
        return Task.FromResult(info.GetDirectories().Select(template => new TemplateModel
        {
            Name = template.Name,
            Variants = template.GetDirectories().Select(variant => variant.Name).ToArray()
        }));
    }
    
    /// <summary>
    /// Gets template by name
    /// </summary>
    /// <returns></returns>
    public Task<TemplateModel> GetByName(string name)
    {
        // get the target directory
        var directory = Path.Combine(GetTemplatesDirectory(), name);
        
        // ensure object exists
        if (!Directory.Exists(directory))
        {
            throw ErrorDefinition.NotFound().AsException();
        }

        // the directory info
        var info = new DirectoryInfo(directory);
        
        // build and return the result
        return Task.FromResult(new TemplateModel
        {
            Name = info.Name,
            Variants = info.GetDirectories().Select(variant => variant.Name).ToArray()
        });
    }

    /// <summary>
    /// Gets the build spec by template name and variant
    /// </summary>
    /// <param name="name"></param>
    /// <param name="variant"></param>
    /// <returns></returns>
    public Task<string> GetBuildSpecByName(string name, string variant)
    {
        // the target file path
        var path = Path.Combine(GetTemplatesDirectory(), name, variant, TemplatingConstants.BUILD_SPEC_FILE);
        
        // the build spec file for the given template and variant
        var buildSpec = new FileInfo(path);

        // ensure exists
        if (!buildSpec.Exists)
        {
            throw ErrorDefinition.NotFound().AsException();
        }
        
        // read the file and return
        return File.ReadAllTextAsync(path);
    }
    
    /// <summary>
    /// Gets the build spec by template name and variant
    /// </summary>
    /// <param name="name"></param>
    /// <param name="variant"></param>
    /// <returns></returns>
    public async Task<JSchema> GetBuildSpecSchemaByName(string name, string variant)
    {
        // read the file and return
        return JSchema.Parse(await this.GetBuildSpecByName(name, variant), new LocalSchemaResolver());
    }
    
    /// <summary>
    /// Gets the build spec by template name and variant
    /// </summary>
    /// <param name="name"></param>
    /// <param name="variant"></param>
    /// <returns></returns>
    public async Task<TemplateRuntimeModel> GetRuntimeByName(string name, string variant)
    {
        // the target file path
        var path = Path.Combine(GetTemplatesDirectory(), name, variant, TemplatingConstants.RUNTIME_FILE);
        
        // the build spec file for the given template and variant
        var runtime = new FileInfo(path);

        // ensure exists
        if (!runtime.Exists)
        {
            throw ErrorDefinition.NotFound().AsException();
        }
        
        // read the file and return
        return JsonConvert.DeserializeObject<TemplateRuntimeModel>(await File.ReadAllTextAsync(runtime.FullName));
    }
    
    /// <summary>
    /// Gets the build spec by template name and variant
    /// </summary>
    /// <param name="name"></param>
    /// <param name="variant"></param>
    /// <returns></returns>
    public Task<string> GetTemplateByName(string name, string variant)
    {
        // the target file path
        var path = Path.Combine(GetTemplatesDirectory(), name, variant, TemplatingConstants.CONTAINERFILE_TEMPLATE_FILE);
        
        // the build spec file for the given template and variant
        var runtime = new FileInfo(path);

        // ensure exists
        if (!runtime.Exists)
        {
            throw ErrorDefinition.NotFound().AsException();
        }
        
        // read the file and return
        return File.ReadAllTextAsync(runtime.FullName);
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