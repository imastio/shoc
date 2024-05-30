using System.Threading.Tasks;
using Shoc.Core;
using Shoc.Identity.Model.User;
using Shoc.Identity.Provider.Data;

namespace Shoc.Identity.Services;

/// <summary>
/// The user service 
/// </summary>
public class UserServiceBase
{
    /// <summary>
    /// The user repository
    /// </summary>
    protected readonly IUserRepository userRepository;

    /// <summary>
    /// Creates new instance of user service
    /// </summary>
    /// <param name="userRepository">The user repository</param>
    protected UserServiceBase(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    /// <summary>
    /// Require the user by id
    /// </summary>
    /// <param name="id">The user id</param>
    /// <returns></returns>
    protected async Task<UserModel> RequireById(string id)
    {
        var result = await this.userRepository.GetById(id);

        if (result == null)
        {
            throw ErrorDefinition.NotFound().AsException();
        }

        return result;
    }
}