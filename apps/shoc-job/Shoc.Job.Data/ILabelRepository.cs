using System.Collections.Generic;
using System.Threading.Tasks;
using Shoc.Job.Model.Label;

namespace Shoc.Job.Data;

/// <summary>
/// The repository interface
/// </summary>
public interface ILabelRepository
{
    /// <summary>
    /// Gets all the objects
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<LabelModel>> GetAll(string workspaceId);
    
    /// <summary>
    /// Gets the object by id
    /// </summary>
    /// <returns></returns>
    Task<LabelModel> GetById(string workspaceId, string id);
    
    /// <summary>
    /// Gets the object by name
    /// </summary>
    /// <returns></returns>
    Task<LabelModel> GetByName(string workspaceId, string name);

    /// <summary>
    /// Creates the object with given input
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="input">The creation input</param>
    /// <returns></returns>
    Task<LabelModel> Create(string workspaceId, LabelCreateModel input);
    
    /// <summary>
    /// Ensures the labels with the given names exist
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="input">The labels to ensure</param>
    /// <returns></returns>
    Task Ensure(string workspaceId, LabelsEnsureModel input);
    
    /// <summary>
    /// Deletes the object by id
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="id">The id of the object</param>
    /// <returns></returns>
    Task<LabelModel> DeleteById(string workspaceId, string id);
}