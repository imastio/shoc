using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shoc.ApiCore;
using Shoc.ApiCore.Access;
using Shoc.Job.Model;
using Shoc.Job.Model.GitRepo;
using Shoc.Job.Services;

namespace Shoc.Job.Controllers;

/// <summary>
/// The git repos endpoint
/// </summary>
[Route("api/management/workspaces/{workspaceId}/git-repos")]
[ApiController]
[ShocExceptionHandler]
public class GitReposController : ControllerBase
{
    /// <summary>
    /// The git repos service
    /// </summary>
    private readonly GitRepoService gitRepoService;

    /// <summary>
    /// Creates new instance of controller
    /// </summary>
    /// <param name="gitRepoService">The reference to service</param>
    public GitReposController(GitRepoService gitRepoService)
    {
        this.gitRepoService = gitRepoService;
    }

    /// <summary>
    /// Gets all the objects
    /// </summary>
    /// <returns></returns>
    [AuthorizeAnyAccess(JobAccesses.JOB_GIT_REPOS_LIST)]
    [HttpGet]
    public Task<IEnumerable<GitRepoModel>> GetAll(string workspaceId)
    {
        return this.gitRepoService.GetAll(workspaceId);
    }
    
    /// <summary>
    /// Gets the object by id
    /// </summary>
    /// <returns></returns>
    [AuthorizeAnyAccess(JobAccesses.JOB_GIT_REPOS_READ)]
    [HttpGet("{id}")]
    public Task<GitRepoModel> GetById(string workspaceId, string id)
    {
        return this.gitRepoService.GetById(workspaceId, id);
    }
    
    /// <summary>
    /// Creates the new object
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="input">The create input</param>
    /// <returns></returns>
    [AuthorizeAnyAccess(JobAccesses.JOB_GIT_REPOS_CREATE)]
    [HttpPost]
    public Task<GitRepoModel> Create(string workspaceId, GitRepoCreateModel input)
    {
        return this.gitRepoService.Create(workspaceId, input);
    }

    /// <summary>
    /// Deletes the object by id
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="id">The id of the object</param>
    /// <returns></returns>
    [AuthorizeAnyAccess(JobAccesses.JOB_GIT_REPOS_DELETE)]
    [HttpDelete("{id}")]
    public Task<GitRepoModel> DeleteById(string workspaceId, string id)
    {
        return this.gitRepoService.DeleteById(workspaceId, id);
    }
}

