using System.Threading.Tasks;
using Shoc.Identity.Provider.Data;

namespace Shoc.Identity.Services;

/// <summary>
/// The current session service
/// </summary>
public class CurrentSessionService
{
    /// <summary>
    /// The user repository service
    /// </summary>
    private readonly IUserInternalRepository userInternalRepository;

    /// <summary>
    /// Creates new instance of current session service
    /// </summary>
    /// <param name="userInternalRepository">The session service</param>
    public CurrentSessionService(IUserInternalRepository userInternalRepository)
    {
        this.userInternalRepository = userInternalRepository;
    }

    /// <summary>
    /// Gets the effective access list
    /// </summary>
    /// <param name="subject">The current subject</param>
    /// <returns></returns>
    public async Task<object> Get(string subject)
    {
        // no id is given means not authenticated
        if (string.IsNullOrWhiteSpace(subject))
        {
            return new
            {
                Authenticated = false
            };
        }

        // try load the user with given id
        var user = await this.userInternalRepository.GetById(subject);

        // no such user found
        if (user == null)
        {
            return new
            {
                Authenticated = false
            };
        }

        return new
        {
            Authenticated = true,
            user.Id,
            user.FullName,
            user.Email,
            user.EmailVerified
        };
    }
}