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
    private readonly TemplateBuildSpecGenerator buildSpecGenerator;
    
    /// <summary>
    /// Creates new instance of the controller
    /// </summary>
    public TemplatesController(TemplateProvider templateProvider, TemplateBuildSpecGenerator buildSpecGenerator)
    {
        this.templateProvider = templateProvider;
        this.buildSpecGenerator = buildSpecGenerator;
    }
    
    /// <summary>
    /// Gets all the available templates
    /// </summary>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpGet]
    public Task<IEnumerable<TemplateModel>> GetAll()
    {
        return this.templateProvider.GetAll();
    }
    
    /// <summary>
    /// Gets the template by name
    /// </summary>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpGet("{name}")]
    public Task<TemplateModel> GetByName(string name)
    {
        return this.templateProvider.GetByName(name);
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
            Content = await this.templateProvider.GetBuildSpecByName(name, variant),
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
            Content = (await this.buildSpecGenerator.Generate(name, variant))?.ToString(),
            ContentType = "application/json"
        };
    }
}