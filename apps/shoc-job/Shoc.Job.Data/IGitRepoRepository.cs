using System.Collections.Generic;
using System.Threading.Tasks;
using Shoc.Job.Model.GitRepo;

namespace Shoc.Job.Data;

/// <summary>
/// The repository interface
/// </summary>
public interface IGitRepoRepository
{
    /// <summary>
    /// Gets all the objects
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<GitRepoModel>> GetAll(string workspaceId);
    
    /// <summary>
    /// Gets the object by id
    /// </summary>
    /// <returns></returns>
    Task<GitRepoModel> GetById(string workspaceId, string id);
    
    /// <summary>
    /// Gets the object by remote url
    /// </summary>
    /// <returns></returns>
    Task<GitRepoModel> GetByRemoteUrl(string workspaceId, string remoteUrl);
    
    /// <summary>
    /// Gets the object by source and repository
    /// </summary>
    /// <returns></returns>
    Task<GitRepoModel> GetBySourceAndRepository(string workspaceId, string source, string repository);

    /// <summary>
    /// Creates the object with given input
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="input">The creation input</param>
    /// <returns></returns>
    Task<GitRepoModel> Create(string workspaceId, GitRepoCreateModel input);
    
    /// <summary>
    /// Ensures the object with given input
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="input">The creation input</param>
    /// <returns></returns>
    Task<GitRepoModel> Ensure(string workspaceId, GitRepoCreateModel input);
    
    /// <summary>
    /// Deletes the object by id
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="id">The id of the object</param>
    /// <returns></returns>
    Task<GitRepoModel> DeleteById(string workspaceId, string id);
}