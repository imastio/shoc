using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Shoc.Core;
using Shoc.Workspace.Data;
using Shoc.Workspace.Model;
using Shoc.Workspace.Model.Workspace;

namespace Shoc.Workspace.Services;

/// <summary>
/// The base service for workspaces
/// </summary>
public abstract class WorkspaceServiceBase
{
    /// <summary>
    /// The name pattern for workspace
    /// </summary>
    private static readonly Regex NAME_PATTERN = new(@"^[a-z\d](?:[a-z\d]|-(?=[a-z\d])){0,38}$");
    
    /// <summary>
    /// The object repository
    /// </summary>
    protected readonly IWorkspaceRepository workspaceRepository;

    /// <summary>
    /// The workspace user repository
    /// </summary>
    protected readonly IWorkspaceUserRepository workspaceUserRepository;

    /// <summary>
    /// The base implementation of the service
    /// </summary>
    /// <param name="workspaceRepository">The object repository</param>
    /// <param name="workspaceUserRepository">The workspace user repository</param>
    protected WorkspaceServiceBase(IWorkspaceRepository workspaceRepository, IWorkspaceUserRepository workspaceUserRepository)
    {
        this.workspaceRepository = workspaceRepository;
        this.workspaceUserRepository = workspaceUserRepository;
    }

    /// <summary>
    /// Requires the object by id
    /// </summary>
    /// <param name="id">The id of the object</param>
    /// <returns></returns>
    protected async Task<WorkspaceModel> RequireWorkspaceById(string id)
    {
        // id should be a valid string
        if (string.IsNullOrWhiteSpace(id))
        {
            throw ErrorDefinition.NotFound().AsException();
        }

        // try getting object by id
        var result = await this.workspaceRepository.GetById(id);

        // make sure object exists
        if (result == null)
        {
            throw ErrorDefinition.NotFound().AsException();
        }

        // return the object
        return result;
    }
    
    /// <summary>
    /// Validate user by id
    /// </summary>
    /// <param name="id">The id to validate</param>
    protected async Task RequireUser(string id)
    {
        // no user id given
        if (string.IsNullOrWhiteSpace(id))
        {
            throw ErrorDefinition.Validation(WorkspaceErrors.INVALID_USER).AsException();
        }
        
        // try getting object
        var result = await this.workspaceUserRepository.GetById(id);

        // the user does not exist
        if (result == null)
        {
            throw ErrorDefinition.Validation(WorkspaceErrors.INVALID_USER).AsException();
        }
    }

    /// <summary>
    /// Validate object type
    /// </summary>
    /// <param name="type">The type to validate</param>
    protected static void ValidateType(string type)
    {
        // make sure valid type
        if (WorkspaceTypes.ALL.Contains(type))
        {
            return;
        }

        throw ErrorDefinition.Validation(WorkspaceErrors.INVALID_TYPE).AsException();
    }

    /// <summary>
    /// Validate object status
    /// </summary>
    /// <param name="status">The status to validate</param>
    protected static void ValidateStatus(string status)
    {
        // make sure valid status
        if (WorkspaceStatuses.ALL.Contains(status))
        {
            return;
        }

        throw ErrorDefinition.Validation(WorkspaceErrors.INVALID_STATUS).AsException();
    }

    /// <summary>
    /// Validate object name
    /// </summary>
    /// <param name="name">The name to validate</param>
    protected static void ValidateName(string name)
    {
        // name matches the pattern
        if (NAME_PATTERN.IsMatch(name))
        {
            return;
        }

        throw ErrorDefinition.Validation(WorkspaceErrors.INVALID_NAME).AsException();
    }
}