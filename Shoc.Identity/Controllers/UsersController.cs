using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shoc.ApiCore;
using Shoc.ApiCore.Protection;
using Shoc.Identity.Model;
using Shoc.Identity.Services;

namespace Shoc.Identity.Controllers
{
    /// <summary>
    /// The users controller
    /// </summary>
    [Route("api/users")]
    [ApiController]
    [ShocExceptionHandler]
    [BearerOnly]
    [AuthorizeMinUserType(UserTypes.ADMIN)]
    public class UsersController : ControllerBase
    {
        /// <summary>
        /// The user service
        /// </summary>
        private readonly UserService userService;

        /// <summary>
        /// Creates new instance of users controller
        /// </summary>
        /// <param name="userService">The users service</param>
        public UsersController(UserService userService)
        {
            this.userService = userService;
        }

        /// <summary>
        /// Gets all the users
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public Task<IEnumerable<UserModel>> GetAll()
        {
            return this.userService.GetAll();
        }

        /// <summary>
        /// Gets the user by id
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var result = await this.userService.GetById(id);

            // no object with such an id
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        /// <summary>
        /// Gets the user by email
        /// </summary>
        /// <returns></returns>
        [HttpGet("by-email/{email}")]
        public async Task<IActionResult> GetByEmail(string email)
        {
            var result = await this.userService.GetByEmail(email);

            // no object with such an id
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        /// <summary>
        /// Creates new user based on input
        /// </summary>
        /// <param name="input">The user input</param>
        /// <returns></returns>
        [HttpPost]
        public Task<UserModel> Create([FromBody] CreateUserModel input)
        {
            return this.userService.Create(input);
        }

        /// <summary>
        /// Deletes a entity with the given id
        /// </summary>
        /// <param name="id">The id of entity to delete</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public Task<UserModel> DeleteById(string id)
        {
            return this.userService.DeleteById(id);
        }
    }
}