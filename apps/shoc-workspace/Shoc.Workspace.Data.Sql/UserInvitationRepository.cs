using System.Collections.Generic;
using System.Threading.Tasks;
using Imast.DataOps.Api;
using Shoc.Workspace.Model.User;

namespace Shoc.Workspace.Data.Sql;

/// <summary>
/// The user invitation repository
/// </summary>
public class UserInvitationRepository: IUserInvitationRepository
{
    /// <summary>
    /// The data operations instance
    /// </summary>
    private readonly DataOperations dataOps;

    /// <summary>
    /// Creates new instance of repository implementation
    /// </summary>
    /// <param name="dataOps">A DataOps instance</param>
    public UserInvitationRepository(DataOperations dataOps)
    {
        this.dataOps = dataOps;
    }

    /// <summary>
    /// Gets all the objects
    /// </summary>
    /// <returns></returns>
    public Task<IEnumerable<UserInvitationModel>> GetAll(string userId)
    {
        return this.dataOps.Connect().Query("Workspace.UserInvitation", "GetAll").ExecuteAsync<UserInvitationModel>(new
        {
            UserId = userId 
        });
    }

    /// <summary>
    /// Count all the objects for the user
    /// </summary>
    /// <param name="userId">The user id</param>
    /// <returns></returns>
    public Task<long> CountAll(string userId)
    {
        return this.dataOps.Connect().QueryFirst("Workspace.UserInvitation", "CountAll").ExecuteAsync<long>(new
        {
            UserId = userId 
        });
    }
}