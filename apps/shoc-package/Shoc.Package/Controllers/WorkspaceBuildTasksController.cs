using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shoc.Access.Model;
using Shoc.ApiCore;
using Shoc.ApiCore.Access;
using Shoc.ApiCore.Auth;
using Shoc.Package.Model.BuildTask;
using Shoc.Package.Services;

namespace Shoc.Package.Controllers;

/// <summary>
/// The secrets endpoint
/// </summary>
[Route("api/workspaces/{workspaceId}/build-tasks")]
[ApiController]
[ShocExceptionHandler]
[AuthorizeMinUserType(KnownUserTypes.EXTERNAL)]
public class WorkspaceBuildTasksController : ControllerBase
{
    /// <summary>
    /// The service
    /// </summary>
    private readonly WorkspaceBuildTaskService buildTaskService;

    /// <summary>
    /// Creates new instance of controller
    /// </summary>
    /// <param name="buildTaskService">The reference to service</param>
    public WorkspaceBuildTasksController(WorkspaceBuildTaskService buildTaskService)
    {
        this.buildTaskService = buildTaskService;
    }

    /// <summary>
    /// Gets all the objects
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <returns></returns>
    [HttpGet]
    public Task<IEnumerable<BuildTaskModel>> GetAll(string workspaceId)
    {
        return this.buildTaskService.GetAll(this.HttpContext.GetPrincipal().Id, workspaceId);
    }
    
    /// <summary>
    /// Creates a new object
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="input">The creation input</param>
    /// <returns></returns>
    [HttpPost]
    public Task<BuildTaskModel> Create(string workspaceId, [FromBody] BuildTaskCreateModel input)
    {
        return this.buildTaskService.Create(this.HttpContext.GetPrincipal().Id, workspaceId, input);
    }

    /// <summary>
    /// Updates the object bundle
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="id">The id of the object</param>
    /// <returns></returns>
    [HttpPut("{id}/bundle")]
    [DisableFormValueModelBinding]
    [DisableRequestSizeLimit]
    public async Task<BuildTaskModel> UpdateBundleById(string workspaceId, string id)
    {
        var buildTask = default(BuildTaskModel);
        
        // start streaming the request file
        await this.Request.StreamFiles(async file => 
        {
            // open stream to read
            await using var stream = file.OpenReadStream();

            // upload the bundle 
            buildTask = await this.buildTaskService.UpdateBundleById(this.HttpContext.GetPrincipal().Id, workspaceId, id, stream);
        });

        return buildTask;
    }
}

