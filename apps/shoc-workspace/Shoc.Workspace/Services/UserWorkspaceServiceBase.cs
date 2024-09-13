using System.Threading.Tasks;
using Shoc.Core;
using Shoc.ObjectAccess.Workspace;
using Shoc.Workspace.Data;
using Shoc.Workspace.Model.UserWorkspace;

namespace Shoc.Workspace.Services;

/// <summary>
/// The base service for user workspaces
/// </summary>
public abstract class UserWorkspaceServiceBase
{
    /// <summary>
    /// The workspace service
    /// </summary>
    protected readonly WorkspaceService workspaceService;

    /// <summary>
    /// The user workspace repository
    /// </summary>
    protected readonly IUserWorkspaceRepository userWorkspaceRepository;

    /// <summary>
    /// Workspace access evaluator
    /// </summary>
    protected readonly IWorkspaceAccessEvaluator workspaceAccessEvaluator;

    /// <summary>
    /// The base implementation of the service
    /// </summary>
    /// <param name="workspaceService">The workspace service</param>
    /// <param name="userWorkspaceRepository">The user workspace repository</param>
    /// <param name="workspaceAccessEvaluator">The workspace access evaluator</param>
    protected UserWorkspaceServiceBase(WorkspaceService workspaceService, IUserWorkspaceRepository userWorkspaceRepository, IWorkspaceAccessEvaluator workspaceAccessEvaluator)
    {
        this.workspaceService = workspaceService;
        this.userWorkspaceRepository = userWorkspaceRepository;
        this.workspaceAccessEvaluator = workspaceAccessEvaluator;
    }
    
    /// <summary>
    /// Requires the object by id
    /// </summary>
    /// <param name="userId">The user id</param>
    /// <param name="id">The id of the object</param>
    /// <returns></returns>
    protected async Task<UserWorkspaceModel> RequireById(string userId, string id)
    {
        // get the result
        var result = await this.userWorkspaceRepository.GetById(userId, id);

        // not found
        if (result == null)
        {
            throw ErrorDefinition.NotFound().AsException();
        }

        return result;
    }
    
    /// <summary>
    /// Requires the object by name
    /// </summary>
    /// <param name="userId">The user id</param>
    /// <param name="name">The name of the object</param>
    /// <returns></returns>
    protected async Task<UserWorkspaceModel> RequireByName(string userId, string name)
    {
        // get the result
        var result = await this.userWorkspaceRepository.GetByName(userId, name);

        // not found
        if (result == null)
        {
            throw ErrorDefinition.NotFound().AsException();
        }

        return result;
    }
}