using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shoc.Access.Model;
using Shoc.ApiCore;
using Shoc.ApiCore.Access;
using Shoc.ApiCore.Auth;
using Shoc.Job.Model.GitRepo;
using Shoc.Job.Services;

namespace Shoc.Job.Controllers;

/// <summary>
/// The git repos endpoint
/// </summary>
[Route("api/workspaces/{workspaceId}/git-repos")]
[ApiController]
[ShocExceptionHandler]
[AuthorizeMinUserType(KnownUserTypes.EXTERNAL)]
public class WorkspaceGitReposController : ControllerBase
{
    /// <summary>
    /// The service
    /// </summary>
    private readonly WorkspaceGitRepoService service;

    /// <summary>
    /// Creates new instance of controller
    /// </summary>
    /// <param name="service">The reference to service</param>
    public WorkspaceGitReposController(WorkspaceGitRepoService service)
    {
        this.service = service;
    }

    /// <summary>
    /// Gets all the objects
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <returns></returns>
    [HttpGet]
    public Task<IEnumerable<GitRepoModel>> GetAll(string workspaceId)
    {
        return this.service.GetAll(this.HttpContext.GetPrincipal().Id, workspaceId);
    }
    
    /// <summary>
    /// Creates a new object
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="input">The creation input</param>
    /// <returns></returns>
    [HttpPost]
    public Task<GitRepoModel> Create(string workspaceId, [FromBody] GitRepoCreateModel input)
    {
        return this.service.Create(this.HttpContext.GetPrincipal().Id, workspaceId, input);
    }
    
    /// <summary>
    /// Ensures a new objects are present
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="input">The ensuring input</param>
    /// <returns></returns>
    [HttpPost("ensure")]
    public Task<GitRepoModel> Ensure(string workspaceId, [FromBody] GitRepoCreateModel input)
    {
        return this.service.Ensure(this.HttpContext.GetPrincipal().Id, workspaceId, input);
    }

    /// <summary>
    /// Deletes the object by id
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="id">The id of the object</param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public Task<GitRepoModel> DeleteById(string workspaceId, string id)
    {
        return this.service.DeleteById(this.HttpContext.GetPrincipal().Id, workspaceId, id);
    }
}

