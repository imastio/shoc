using System.Collections.Generic;
using System.Threading.Tasks;
using Shoc.Workspace.Model.User;

namespace Shoc.Workspace.Data;

/// <summary>
/// The user invitation repository
/// </summary>
public interface IUserInvitationRepository
{
    /// <summary>
    /// Gets all the objects
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<UserInvitationModel>> GetAll(string userId);

    /// <summary>
    /// Count all the objects for the user
    /// </summary>
    /// <param name="userId">The user id</param>
    /// <returns></returns>
    Task<long> CountAll(string userId);
}