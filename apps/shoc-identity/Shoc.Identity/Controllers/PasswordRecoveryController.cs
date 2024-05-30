using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shoc.ApiCore;
using Shoc.Identity.Model.Flow;
using Shoc.Identity.Services;

namespace Shoc.Identity.Controllers;

/// <summary>
/// The authentication controller
/// </summary>
[Route("api-auth/password-recovery")]
[ApiController]
[ShocExceptionHandler]
public class PasswordRecoveryController : ControllerBase
{

    /// <summary>
    /// The password recovery service
    /// </summary>
    private readonly PasswordRecoveryService passwordRecoveryService;

    /// <summary>
    /// Login router controller
    /// </summary>
    /// <param name="passwordRecoveryService">The password recovery service</param>
    public PasswordRecoveryController(PasswordRecoveryService passwordRecoveryService)
    {
        this.passwordRecoveryService = passwordRecoveryService;
    }
    
    /// <summary>
    /// The public password recovery request endpoint
    /// </summary>
    /// <param name="input">The password recovery request</param>
    /// <returns></returns>
    [HttpPost("request")]
    public Task<PasswordRecoveryRequestResult> RequestRecovery(PasswordRecoveryRequest input)
    {
        return this.passwordRecoveryService.Request(input);
    }

    /// <summary>
    /// The public endpoint to confirm password recovery
    /// </summary>
    /// <param name="input">The confirmation input</param>
    /// <returns></returns>
    [HttpPost("process")]
    public Task<PasswordRecoveryProcessResult> ProcessRecovery(PasswordRecoveryProcessRequest input)
    {
        return this.passwordRecoveryService.Process(input);
    }
}

