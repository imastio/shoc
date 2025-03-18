using System.Collections.Generic;
using System.Threading.Tasks;
using Shoc.Job.Model.GitRepo;
using Shoc.ObjectAccess.Model.Workspace;
using Shoc.ObjectAccess.Workspace;

namespace Shoc.Job.Services;

/// <summary>
/// The workspace git repo service
/// </summary>
public class WorkspaceGitRepoService
{
    /// <summary>
    /// The git repo service
    /// </summary>
    private readonly GitRepoService gitRepoService;

    /// <summary>
    /// The access evaluator
    /// </summary>
    private readonly IWorkspaceAccessEvaluator accessEvaluator;

    /// <summary>
    /// Creates new instance of the service
    /// </summary>
    /// <param name="gitRepoService">The git repo service</param>
    /// <param name="accessEvaluator">The access evaluator</param>
    public WorkspaceGitRepoService(GitRepoService gitRepoService, IWorkspaceAccessEvaluator accessEvaluator)
    {
        this.gitRepoService = gitRepoService;
        this.accessEvaluator = accessEvaluator;
    }
    
    /// <summary>
    /// Gets all the objects
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<GitRepoModel>> GetAll(string userId, string workspaceId)
    {
        // load items
        var items = await this.gitRepoService.GetAll(workspaceId);
        
        // ensure we have a permission to view workspace secrets
        await this.accessEvaluator.Ensure(userId, workspaceId, WorkspacePermissions.WORKSPACE_VIEW, WorkspacePermissions.WORKSPACE_LIST_GIT_REPOS);

        // map and return the result
        return items;
    }

    /// <summary>
    /// Creates the object with given input
    /// </summary>
    /// <param name="userId">The user id</param>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="input">The creation input</param>
    /// <returns></returns>
    public async Task<GitRepoModel> Create(string userId, string workspaceId, GitRepoCreateModel input)
    {
        // ensure have required access
        await this.accessEvaluator.Ensure(
            userId,
            workspaceId,
            WorkspacePermissions.WORKSPACE_VIEW,
            WorkspacePermissions.WORKSPACE_LIST_GIT_REPOS,
            WorkspacePermissions.WORKSPACE_CREATE_GIT_REPO);
        
        // ensure referring to the correct object
        input.WorkspaceId = workspaceId;

        // create the object
        return await this.gitRepoService.Create(workspaceId, input);
    }
    
    /// <summary>
    /// Creates the object with given input
    /// </summary>
    /// <param name="userId">The user id</param>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="input">The creation input</param>
    /// <returns></returns>
    public async Task<GitRepoModel> Ensure(string userId, string workspaceId, GitRepoCreateModel input)
    {
        // ensure have required access
        await this.accessEvaluator.Ensure(
            userId,
            workspaceId,
            WorkspacePermissions.WORKSPACE_VIEW,
            WorkspacePermissions.WORKSPACE_LIST_GIT_REPOS,
            WorkspacePermissions.WORKSPACE_CREATE_GIT_REPO);
        
        // ensure referring to the correct object
        input.WorkspaceId = workspaceId;

        // create the object
        return await this.gitRepoService.Ensure(workspaceId, input);
    }

    /// <summary>
    /// Deletes the object by id
    /// </summary>
    /// <param name="userId">The user id</param>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="id">The id of the object</param>
    /// <returns></returns>
    public async Task<GitRepoModel> DeleteById(string userId, string workspaceId, string id)
    {
        // ensure have required access
        await this.accessEvaluator.Ensure(
            userId,
            workspaceId,
            WorkspacePermissions.WORKSPACE_VIEW,
            WorkspacePermissions.WORKSPACE_LIST_GIT_REPOS,
            WorkspacePermissions.WORKSPACE_DELETE_GIT_REPO);
      
        // delete the object
        var result = await this.gitRepoService.DeleteById(workspaceId, id);
        
        // return deleted object
        return result;
    }
}