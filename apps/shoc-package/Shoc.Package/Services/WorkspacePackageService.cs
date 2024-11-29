using System.Threading.Tasks;
using Shoc.ObjectAccess.Model.Workspace;
using Shoc.ObjectAccess.Workspace;
using Shoc.Package.Model.Package;

namespace Shoc.Package.Services;

/// <summary>
/// The workspace package service
/// </summary>
public class WorkspacePackageService
{
    /// <summary>
    /// The package service
    /// </summary>
    private readonly PackageService packageService;

    /// <summary>
    /// The workspace access evaluator
    /// </summary>
    private readonly IWorkspaceAccessEvaluator workspaceAccessEvaluator;

    /// <summary>
    /// Creates new instance of the service
    /// </summary>
    /// <param name="packageService">The package service</param>
    /// <param name="workspaceAccessEvaluator">The workspace access evaluator</param>
    public WorkspacePackageService(PackageService packageService, IWorkspaceAccessEvaluator workspaceAccessEvaluator)
    {
        this.packageService = packageService;
        this.workspaceAccessEvaluator = workspaceAccessEvaluator;
    }
    
    /// <summary>
    /// Creates a new duplicate object
    /// </summary>
    /// <returns></returns>
    public async Task<PackageModel> DuplicateFrom(string userId, string workspaceId, PackageDuplicateFromModel input)
    {
        // ensure have required access
        await this.workspaceAccessEvaluator.Ensure(
            userId,
            workspaceId,
            WorkspacePermissions.WORKSPACE_VIEW,
            WorkspacePermissions.WORKSPACE_LIST_PACKAGES,
            WorkspacePermissions.WORKSPACE_BUILD_PACKAGE);
        
        // ensure referring to the correct object
        input.WorkspaceId = workspaceId;
        input.UserId = userId;

        // create the object
        return await this.packageService.DuplicateFrom(workspaceId, input);
    }
}