using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shoc.ApiCore;
using Shoc.ApiCore.Access;
using Shoc.Job.Model;
using Shoc.Job.Model.Label;
using Shoc.Job.Services;

namespace Shoc.Job.Controllers;

/// <summary>
/// The labels endpoint
/// </summary>
[Route("api/management/workspaces/{workspaceId}/labels")]
[ApiController]
[ShocExceptionHandler]
public class LabelsController : ControllerBase
{
    /// <summary>
    /// The label service
    /// </summary>
    private readonly LabelService labelService;

    /// <summary>
    /// Creates new instance of controller
    /// </summary>
    /// <param name="labelService">The reference to service</param>
    public LabelsController(LabelService labelService)
    {
        this.labelService = labelService;
    }

    /// <summary>
    /// Gets all the objects
    /// </summary>
    /// <returns></returns>
    [AuthorizeAnyAccess(JobAccesses.JOB_LABELS_LIST)]
    [HttpGet]
    public Task<IEnumerable<LabelModel>> GetAll(string workspaceId)
    {
        return this.labelService.GetAll(workspaceId);
    }
    
    /// <summary>
    /// Gets the object by id
    /// </summary>
    /// <returns></returns>
    [AuthorizeAnyAccess(JobAccesses.JOB_LABELS_READ)]
    [HttpGet("{id}")]
    public Task<LabelModel> GetById(string workspaceId, string id)
    {
        return this.labelService.GetById(workspaceId, id);
    }
    
    /// <summary>
    /// Creates the new object
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="input">The create input</param>
    /// <returns></returns>
    [AuthorizeAnyAccess(JobAccesses.JOB_LABELS_CREATE)]
    [HttpPost]
    public Task<LabelModel> Create(string workspaceId, LabelCreateModel input)
    {
        return this.labelService.Create(workspaceId, input);
    }

    /// <summary>
    /// Deletes the object by id
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="id">The id of the object</param>
    /// <returns></returns>
    [AuthorizeAnyAccess(JobAccesses.JOB_LABELS_DELETE)]
    [HttpDelete("{id}")]
    public Task<LabelModel> DeleteById(string workspaceId, string id)
    {
        return this.labelService.DeleteById(workspaceId, id);
    }
}

