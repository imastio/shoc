using System.Collections.Generic;
using System.Threading.Tasks;
using Imast.DataOps.Api;
using Shoc.Core;
using Shoc.Job.Model;
using Shoc.Job.Model.GitRepo;

namespace Shoc.Job.Data.Sql;

/// <summary>
/// The repository interface implementation
/// </summary>
public class GitRepoRepository : IGitRepoRepository
{
    /// <summary>
    /// The data operations instance
    /// </summary>
    private readonly DataOperations dataOps;

    /// <summary>
    /// Creates new instance of repository implementation
    /// </summary>
    /// <param name="dataOps">A DataOps instance</param>
    public GitRepoRepository(DataOperations dataOps)
    {
        this.dataOps = dataOps;
    }

    /// <summary>
    /// Gets all the objects
    /// </summary>
    /// <returns></returns>
    public Task<IEnumerable<GitRepoModel>> GetAll(string workspaceId)
    {
        return this.dataOps.Connect().Query("GitRepo", "GetAll").ExecuteAsync<GitRepoModel>(new
        {
            WorkspaceId = workspaceId
        });
    }

    /// <summary>
    /// Gets the object by id
    /// </summary>
    /// <returns></returns>
    public Task<GitRepoModel> GetById(string workspaceId, string id)
    {
        return this.dataOps.Connect().QueryFirst("GitRepo", "GetById").ExecuteAsync<GitRepoModel>(new
        {
            WorkspaceId = workspaceId,
            Id = id
        });
    }
    
    /// <summary>
    /// Gets the object by remote url
    /// </summary>
    /// <returns></returns>
    public Task<GitRepoModel> GetByRemoteUrl(string workspaceId, string remoteUrl)
    {
        return this.dataOps.Connect().QueryFirst("GitRepo", "GetByRemoteUrl").ExecuteAsync<GitRepoModel>(new
        {
            WorkspaceId = workspaceId,
            RemoteUrl = remoteUrl
        });
    }
    
    /// <summary>
    /// Gets the object by source and repository
    /// </summary>
    /// <returns></returns>
    public Task<GitRepoModel> GetBySourceAndRepository(string workspaceId, string source, string repository)
    {
        return this.dataOps.Connect().QueryFirst("GitRepo", "GetBySourceAndRepository").ExecuteAsync<GitRepoModel>(new
        {
            WorkspaceId = workspaceId,
            Source = source,
            Repository = repository
        });
    }

    /// <summary>
    /// Creates the object with given input
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="input">The creation input</param>
    /// <returns></returns>
    public Task<GitRepoModel> Create(string workspaceId, GitRepoCreateModel input)
    {
        // assign the id
        input.WorkspaceId = workspaceId;
        input.Id ??= StdIdGenerator.Next(JobObjects.GIT_REPOSITORY).ToLowerInvariant();

        return this.dataOps.Connect().QueryFirst("GitRepo", "Create").ExecuteAsync<GitRepoModel>(input);
    }
    
    /// <summary>
    /// Ensures the object with given input
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="input">The creation input</param>
    /// <returns></returns>
    public Task<GitRepoModel> Ensure(string workspaceId, GitRepoCreateModel input)
    {
        // assign the id
        input.WorkspaceId = workspaceId;
        input.Id ??= StdIdGenerator.Next(JobObjects.GIT_REPOSITORY).ToLowerInvariant();

        return this.dataOps.Connect().QueryFirst("GitRepo", "Ensure").ExecuteAsync<GitRepoModel>(input);
    }

    /// <summary>
    /// Deletes the object by id
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="id">The id of the object</param>
    /// <returns></returns>
    public Task<GitRepoModel> DeleteById(string workspaceId, string id)
    {
        return this.dataOps.Connect().QueryFirst("GitRepo", "DeleteById").ExecuteAsync<GitRepoModel>(new
        {
            WorkspaceId = workspaceId,
            Id = id
        });
    }
}