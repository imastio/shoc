using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shoc.ApiCore;
using Shoc.Package.Services;
using Shoc.Package.Templating.Model;

namespace Shoc.Package.Controllers;

/// <summary>
/// The templates controller
/// </summary>
[Route("api/templates")]
[ApiController]
[ShocExceptionHandler]
public class TemplatesController : ControllerBase
{
    /// <summary>
    /// The templates provider
    /// </summary>
    private readonly TemplateProvider templateProvider;
    
    /// <summary>
    /// The build spec generator
    /// </summary>
    private readonly TemplateInitializationService initializationService;
    
    /// <summary>
    /// Creates new instance of the controller
    /// </summary>
    public TemplatesController(TemplateProvider templateProvider, TemplateInitializationService initializationService)
    {
        this.templateProvider = templateProvider;
        this.initializationService = initializationService;
    }
    
    /// <summary>
    /// Gets all the available templates
    /// </summary>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpGet]
    public Task<IEnumerable<TemplateDescriptorModel>> GetAll()
    {
        return this.templateProvider.GetAll();
    }
    
    /// <summary>
    /// Gets the variants by template name
    /// </summary>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpGet("{name}/variants")]
    public Task<IEnumerable<TemplateVariantDefinition>> GetVariants(string name)
    {
        return this.templateProvider.GetVariants(name);
    }
    
    /// <summary>
    /// Gets the variant by name and variant
    /// </summary>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpGet("{name}/variants/{variant}")]
    public Task<TemplateVariantDefinition> GetVariant(string name, string variant)
    {
        return this.templateProvider.GetVariant(name, variant);
    }
    
    /// <summary>
    /// Gets the template build spec by name
    /// </summary>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpGet("{name}/variants/{variant}/build-spec")]
    public async Task<IActionResult> GetBuildSpecByName(string name, string variant)
    {
        return new ContentResult
        {
            Content = (await this.templateProvider.GetVariant(name, variant)).BuildSpec,
            ContentType = "application/json"
        };
    }
    
    /// <summary>
    /// Gets the template build spec example by name
    /// </summary>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpGet("{name}/variants/{variant}/build-spec/instance")]
    public async Task<IActionResult> GetBuildSpecInstanceByName(string name, string variant)
    {
        return new ContentResult
        {
            Content = (await this.initializationService.Generate(name, variant))?.ToString(),
            ContentType = "application/json"
        };
    }
}