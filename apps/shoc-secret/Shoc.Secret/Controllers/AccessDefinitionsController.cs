using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shoc.Access.Model;
using Shoc.ApiCore;
using Shoc.ApiCore.Access;
using Shoc.Secret.Model;

namespace Shoc.Secret.Controllers;

/// <summary>
/// The access definitions controller
/// </summary>
[Route("api/access-definitions")]
[ApiController]
[ShocExceptionHandler]
[AuthorizeMinUserType(KnownUserTypes.EXTERNAL)]
public class AccessDefinitionsController : ControllerBase
{
    /// <summary>
    /// Creates new instance of access definitions controller
    /// </summary>
    public AccessDefinitionsController()
    {
    }
    
    /// <summary>
    /// Gets all definitions
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public Task<IEnumerable<string>> GetAll()
    {
        return Task.FromResult<IEnumerable<string>>(SecretAccesses.ALL);
    }
}