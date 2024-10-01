using System.Collections.Generic;
using System.Linq;
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
    /// Gets all the available schemas
    /// </summary>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpGet]
    public async Task<IEnumerable<object>> GetAll()
    {
        // get names
        var result = await this.schemaProvider.GetSchemaNames();

        // build objects and return the result
        return result.Select(name => new { Name = name });
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