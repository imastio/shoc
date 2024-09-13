using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shoc.ApiCore;
using Shoc.Identity.Model.Flow;
using Shoc.Identity.Services;

namespace Shoc.Identity.Controllers;

/// <summary>
/// The otp controller
/// </summary>
[Route("api-auth/otp")]
[ApiController]
[ShocExceptionHandler]
public class OtpController : ControllerBase
{
    /// <summary>
    /// The email confirmation service
    /// </summary>
    private readonly OtpService otpService;

    /// <summary>
    /// Creates new instance of confirmation controller
    /// </summary>
    /// <param name="otpService">The one-time password service</param>
    public OtpController(OtpService otpService)
    {
        this.otpService = otpService;
    }

    /// <summary>
    /// The public OTP placement endpoint
    /// </summary>
    /// <param name="input">The OTP request input</param>
    /// <returns></returns>
    [HttpPost("request")]
    public Task<OneTimePassRequestResult> RequestOneTimePassword(OneTimePassRequest input)
    {
        return this.otpService.Request(input);
    }
}

