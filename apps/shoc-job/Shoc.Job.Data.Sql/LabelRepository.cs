using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Imast.DataOps.Api;
using Shoc.Core;
using Shoc.Job.Model;
using Shoc.Job.Model.Label;

namespace Shoc.Job.Data.Sql;

/// <summary>
/// The repository interface implementation
/// </summary>
public class LabelRepository : ILabelRepository
{
    /// <summary>
    /// The data operations instance
    /// </summary>
    private readonly DataOperations dataOps;

    /// <summary>
    /// Creates new instance of repository implementation
    /// </summary>
    /// <param name="dataOps">A DataOps instance</param>
    public LabelRepository(DataOperations dataOps)
    {
        this.dataOps = dataOps;
    }

    /// <summary>
    /// Gets all the objects
    /// </summary>
    /// <returns></returns>
    public Task<IEnumerable<LabelModel>> GetAll(string workspaceId)
    {
        return this.dataOps.Connect().Query("Label", "GetAll").ExecuteAsync<LabelModel>(new
        {
            WorkspaceId = workspaceId
        });
    }

    /// <summary>
    /// Gets the objects by names
    /// </summary>
    /// <returns></returns>
    public Task<IEnumerable<LabelModel>> GetByNames(string workspaceId, IEnumerable<string> names)
    {
        return this.dataOps.Connect().Query("Label", "GetByNames").ExecuteAsync<LabelModel>(new
        {
            WorkspaceId = workspaceId,
            Names = names
        });
    }

    /// <summary>
    /// Gets the object by id
    /// </summary>
    /// <returns></returns>
    public Task<LabelModel> GetById(string workspaceId, string id)
    {
        return this.dataOps.Connect().QueryFirst("Label", "GetById").ExecuteAsync<LabelModel>(new
        {
            WorkspaceId = workspaceId,
            Id = id
        });
    }
    
    /// <summary>
    /// Gets the object by name
    /// </summary>
    /// <returns></returns>
    public Task<LabelModel> GetByName(string workspaceId, string name)
    {
        return this.dataOps.Connect().QueryFirst("Label", "GetByName").ExecuteAsync<LabelModel>(new
        {
            WorkspaceId = workspaceId,
            Name = name
        });
    }

    /// <summary>
    /// Creates the object with given input
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="input">The creation input</param>
    /// <returns></returns>
    public Task<LabelModel> Create(string workspaceId, LabelCreateModel input)
    {
        // assign the id
        input.WorkspaceId = workspaceId;
        input.Id ??= StdIdGenerator.Next(JobObjects.LABEL).ToLowerInvariant();

        return this.dataOps.Connect().QueryFirst("Label", "Create").ExecuteAsync<LabelModel>(input);
    }

    /// <summary>
    /// Ensures the labels with the given names exist
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="input">The labels to ensure</param>
    /// <returns></returns>
    public Task Ensure(string workspaceId, LabelsEnsureModel input)
    {
        // items to ensure
        var items = (input.Names ?? Enumerable.Empty<string>()).Select(name => new LabelCreateModel
        {
            Id = StdIdGenerator.Next(JobObjects.LABEL).ToLowerInvariant(),
            WorkspaceId = input.WorkspaceId,
            Name = name
        });
        
        return this.dataOps.Connect().NonQuery("Label", "Ensure").ExecuteAsync(items);
    }

    /// <summary>
    /// Deletes the object by id
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="id">The id of the object</param>
    /// <returns></returns>
    public Task<LabelModel> DeleteById(string workspaceId, string id)
    {
        return this.dataOps.Connect().QueryFirst("Label", "DeleteById").ExecuteAsync<LabelModel>(new
        {
            WorkspaceId = workspaceId,
            Id = id
        });
    }
}