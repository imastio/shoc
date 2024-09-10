using System.Collections.Generic;
using System.Threading.Tasks;
using Shoc.Cluster.Model.Cluster;

namespace Shoc.Cluster.Data;

/// <summary>
/// The repository interface
/// </summary>
public interface IClusterRepository
{
    /// <summary>
    /// Gets all the objects
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<ClusterModel>> GetAll(string workspaceId);
    
    /// <summary>
    /// Gets all the extended objects
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<ClusterExtendedModel>> GetAllExtended(string workspaceId);
    
    /// <summary>
    /// Get all referential values
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<ClusterReferentialValueModel>> GetAllReferentialValues(string workspaceId);
    
    /// <summary>
    /// Gets the object by id
    /// </summary>
    /// <returns></returns>
    Task<ClusterModel> GetById(string workspaceId, string id);
    
    /// <summary>
    /// Gets the object by workspace id and the name 
    /// </summary>
    /// <returns></returns>
    Task<ClusterModel> GetByName(string workspaceId, string name);

    /// <summary>
    /// Creates the object with given input
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="input">The creation input</param>
    /// <returns></returns>
    Task<ClusterModel> Create(string workspaceId, ClusterCreateModel input);

    /// <summary>
    /// Updates the object by id
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="id">The object id</param>
    /// <param name="input">The update input</param>
    /// <returns></returns>
    Task<ClusterModel> UpdateById(string workspaceId, string id, ClusterUpdateModel input);

    /// <summary>
    /// Updates the object configuration by id
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="id">The object id</param>
    /// <param name="input">The update input</param>
    /// <returns></returns>
    Task<ClusterModel> UpdateConfigurationById(string workspaceId, string id, ClusterConfigurationUpdateModel input);

    /// <summary>
    /// Deletes the object by id
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="id">The id of the object</param>
    /// <returns></returns>
    Task<ClusterModel> DeleteById(string workspaceId, string id);
}