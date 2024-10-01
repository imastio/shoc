using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shoc.ApiCore;
using Shoc.Package.Services;

namespace Shoc.Package.Controllers;

/// <summary>
/// The schemas controller
/// </summary>
[Route("api/schemas")]
[ApiController]
[ShocExceptionHandler]
public class SchemasController : ControllerBase
{
    /// <summary>
    /// The schema provider
    /// </summary>
    private readonly SchemaProvider schemaProvider;
    
    /// <summary>
    /// Creates new instance of access definitions controller
    /// </summary>
    public SchemasController(SchemaProvider schemaProvider)
    {
        this.schemaProvider = schemaProvider;
    }
    
    /// <summary>
    /// Gets the schema by name
    /// </summary>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpGet("{name}")]
    public async Task<IActionResult> Get(string name)
    {
        // load the json schema
        var result = await this.schemaProvider.GetSchema(name);
        
        // return as json
        return new ContentResult
        {
            Content = result,
            ContentType = "application/json"
        };
    }
}