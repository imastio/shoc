using System.Collections.Generic;
using System.Threading.Tasks;
using Shoc.Access.Data;
using Shoc.Access.Model;
using Shoc.Identity.Model.User;
using Shoc.Identity.Provider.Data;

namespace Shoc.Identity.Services;

/// <summary>
/// The user access service
/// </summary>
public class UserAccessService : UserServiceBase
{
    /// <summary>
    /// The access repository
    /// </summary>
    private readonly IAccessRepository accessRepository;

    /// <summary>
    /// The user access repository.
    /// </summary>
    private readonly IUserAccessRepository userAccessRepository;

    /// <summary>
    /// Creates new instance of user access service
    /// </summary>
    /// <param name="userRepository">The user repository</param>
    /// <param name="accessRepository">The access repository</param>
    /// <param name="userAccessRepository">The user access repository.</param>
    public UserAccessService(IUserRepository userRepository, IAccessRepository accessRepository, IUserAccessRepository userAccessRepository) : base(userRepository)
    {
        this.accessRepository = accessRepository;
        this.userAccessRepository = userAccessRepository;
    }

    /// <summary>
    /// Gets all the accesses granted to the user by the user id.
    /// </summary>
    /// <param name="userId">The user id</param>
    /// <returns></returns>
    public async Task<IEnumerable<UserAccessModel>> Get(string userId)
    {
        // require the object to be exist
        await this.RequireById(userId);
        
        // get all objects
        return await this.userAccessRepository.GetByUserId(userId);
    }

    /// <summary>
    /// Gets the effective accesses assigned to user
    /// </summary>
    /// <param name="userId">The user id</param>
    /// <returns></returns>
    public async Task<IEnumerable<AccessValueModel>> GetEffective(string userId)
    {
        // require the object to be exists
        await this.RequireById(userId);

        // get effective accesses
        return await this.accessRepository.GetEffectiveByUser(userId);
    }
    
    /// <summary>
    /// Creates new access record for the given user
    /// </summary>
    /// <param name="userId">The user id</param>
    /// <param name="input">The creation input</param>
    /// <returns></returns>
    public async Task<UserAccessUpdateResultModel> Update(string userId, UserAccessUpdateModel input)
    {
        // make sure object exists
        await this.RequireById(userId);
        
        // perform the operation
        return await this.userAccessRepository.UpdateByUserId(userId, input);
    }
}