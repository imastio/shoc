using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shoc.ApiCore;
using Shoc.ApiCore.Access;
using Shoc.Workspace.Model;
using Shoc.Workspace.Model.Workspace;
using Shoc.Workspace.Services;

namespace Shoc.Workspace.Controllers;

/// <summary>
/// The workspaces endpoint
/// </summary>
[Route("api/workspaces")]
[ApiController]
[ShocExceptionHandler]
public class WorkspacesController : ControllerBase
{
    /// <summary>
    /// The object service
    /// </summary>
    private readonly WorkspaceService workspaceService;

    /// <summary>
    /// Creates new instance of the controller
    /// </summary>
    /// <param name="workspaceService">The object service</param>
    public WorkspacesController(WorkspaceService workspaceService)
    {
        this.workspaceService = workspaceService;
    }

    /// <summary>
    /// Gets all the objects
    /// </summary>
    /// <returns></returns>
    [AuthorizeAnyAccess(WorkspaceAccesses.WORKSPACE_WORKSPACES_LIST)]
    [HttpGet]
    public Task<IEnumerable<WorkspaceModel>> GetAll()
    {
        return this.workspaceService.GetAll();
    }
    

    /// <summary>
    /// Gets all the object options
    /// </summary>
    /// <returns></returns>
    [AuthorizeAnyAccess(WorkspaceAccesses.WORKSPACE_WORKSPACES_LIST_REFERENCES)]
    [HttpGet("referential-values")]
    public Task<IEnumerable<WorkspaceReferentialValueModel>> GetAllReferentialValues()
    {
        return this.workspaceService.GetAllReferentialValues();
    }

    /// <summary>
    /// Gets object by id
    /// </summary>
    /// <param name="id">The id of object</param>
    /// <returns></returns>
    [AuthorizeAnyAccess(WorkspaceAccesses.WORKSPACE_WORKSPACES_READ)]
    [HttpGet("{id}")]
    public Task<WorkspaceModel> GetById(string id)
    {
        return this.workspaceService.GetById(id);
    }
    
    /// <summary>
    /// Creates a new object
    /// </summary>
    /// <param name="input">The object creation model</param>
    /// <returns></returns>
    [AuthorizeAnyAccess(WorkspaceAccesses.WORKSPACE_WORKSPACES_CREATE)]
    [HttpPost]
    public Task<WorkspaceModel> Create(WorkspaceCreateModel input)
    {
        return this.workspaceService.Create(input);
    }

    /// <summary>
    /// Updates the object by given input
    /// </summary>
    /// <param name="id">The id of the object</param>
    /// <param name="input">The object update input</param>
    /// <returns></returns>
    [AuthorizeAnyAccess(WorkspaceAccesses.WORKSPACE_WORKSPACES_EDIT)]
    [HttpPut("{id}")]
    public Task<WorkspaceModel> UpdateById(string id, WorkspaceUpdateModel input)
    {
        return this.workspaceService.UpdateById(id, input);
    }

    /// <summary>
    /// Deletes object by id
    /// </summary>
    /// <param name="id">The id of object</param>
    /// <returns></returns>
    [AuthorizeAnyAccess(WorkspaceAccesses.WORKSPACE_WORKSPACES_DELETE)]
    [HttpDelete("{id}")]
    public Task<WorkspaceModel> DeleteById(string id)
    {
        return this.workspaceService.DeleteById(id);
    }
}

