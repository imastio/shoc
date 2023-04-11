using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Shoc.ApiCore;
using Shoc.Identity.Model;
using Shoc.Identity.Services;

namespace Shoc.Identity.Controllers
{
    /// <summary>
    /// The setup controller
    /// </summary>
    [Route("api/setup")]
    [ApiController]
    [ShocExceptionHandler]
    public class SetupController : ControllerBase
    {
        /// <summary>
        /// The user service
        /// </summary>
        private readonly UserService userService;

        /// <summary>
        /// Setup controller
        /// </summary>
        /// <param name="userService">The user service</param>
        public SetupController(UserService userService)
        {
            this.userService = userService;
        }

        /// <summary>
        /// The public setup endpoint
        /// </summary>
        /// <param name="input">The setup input</param>
        /// <returns></returns>
        [HttpPost("root")]
        [AllowAnonymous]
        public async Task<UserModel> CreateRoot(CreateRootModel input)
        {
            return await this.userService.CreateRoot(input);
        }
    }
}
