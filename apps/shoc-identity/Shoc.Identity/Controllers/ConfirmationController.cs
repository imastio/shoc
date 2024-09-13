using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shoc.ApiCore;
using Shoc.Identity.Model.Flow;
using Shoc.Identity.Services;

namespace Shoc.Identity.Controllers;

/// <summary>
/// The confirmation controller
/// </summary>
[Route("api-auth/confirmation")]
[ApiController]
[ShocExceptionHandler]
public class ConfirmationController : ControllerBase
{
    /// <summary>
    /// The email confirmation service
    /// </summary>
    private readonly ConfirmationService confirmationService;

    /// <summary>
    /// Creates new instance of confirmation controller
    /// </summary>
    /// <param name="confirmationService">The password recovery service</param>
    public ConfirmationController(ConfirmationService confirmationService)
    {
        this.confirmationService = confirmationService;
    }

    /// <summary>
    /// The public confirmation code request
    /// </summary>
    /// <param name="input">The code request input</param>
    /// <returns></returns>
    [HttpPost("request")]
    public Task<ConfirmationRequestResult> RequestConfirmation(ConfirmationRequest input)
    {
        return this.confirmationService.Request(input);
    }
    
    /// <summary>
    /// The public confirmation endpoint
    /// </summary>
    /// <param name="link">The link fragment</param>
    /// <param name="proof">The proof of link</param>
    /// <returns></returns>
    [HttpGet("confirm/{link}/crypto-proof/{proof}")]
    public async Task<IActionResult> ConfirmEmail(string link, string proof)
    {
        return this.Redirect(await this.confirmationService.ConfirmWithLink(this.HttpContext, link, proof));
    }

    /// <summary>
    /// The public to process confirmation request
    /// </summary>
    /// <param name="input">The confirmation input</param>
    /// <returns></returns>
    [HttpPost("confirm")]
    public Task<ConfirmationProcessResult> ConfirmWithRequest(ConfirmationProcessRequest input)
    {
        // process confirmation 
        return this.confirmationService.ConfirmWithRequest(this.HttpContext, input);
    }
}

