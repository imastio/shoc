using System.Linq;
using System.Threading.Tasks;
using Shoc.Core;
using Shoc.Package.Data;
using Shoc.Package.Model;
using Shoc.Package.Model.Command;
using Shoc.Package.Model.Package;
using Shoc.Package.Model.Registry;

namespace Shoc.Package.Services;

/// <summary>
/// The package service
/// </summary>
public class PackageService
{
    /// <summary>
    /// The package repository
    /// </summary>
    private readonly IPackageRepository packageRepository;

    /// <summary>
    /// The validation service
    /// </summary>
    private readonly ValidationService validationService;

    /// <summary>
    /// The registry handler service
    /// </summary>
    private readonly RegistryHandlerService registryHandlerService;

    /// <summary>
    /// The container service
    /// </summary>
    private readonly ContainerService containerService;

    /// <summary>
    /// Creates new instance of the service
    /// </summary>
    /// <param name="packageRepository">The package repository</param>
    /// <param name="validationService">The validation service</param>
    /// <param name="registryHandlerService">The registry handler service</param>
    /// <param name="containerService">The container service</param>
    public PackageService(IPackageRepository packageRepository, ValidationService validationService, RegistryHandlerService registryHandlerService, ContainerService containerService)
    {
        this.packageRepository = packageRepository;
        this.validationService = validationService;
        this.registryHandlerService = registryHandlerService;
        this.containerService = containerService;
    }

    public async Task<PackageModel> DuplicateFrom(string workspaceId, PackageDuplicateFromModel input)
    {
        // ensure referring to the correct object
        input.WorkspaceId = workspaceId;
        
        // require the parent object
        await this.validationService.RequireWorkspace(workspaceId);

        // require valid user
        await this.validationService.RequireUser(input.UserId);
        
        // validate the scope
        this.validationService.ValidateScope(input.Scope);
        
        // validate the listing checksum
        this.validationService.ValidateListingChecksum(input.ListingChecksum);
        
        // gets the registry to store the image
        input.RegistryId = (await this.registryHandlerService.GetDefaultRegistryId(workspaceId)).Id;

        // get all packages with matching workspace and checksum and filter only matching users or workspace-scoped packages
        var packages = (await this.packageRepository.GetAllByListingChecksum(input.WorkspaceId, input.ListingChecksum))
            .Where(package => package.UserId == input.UserId || package.Scope == PackageScopes.WORKSPACE)
            .OrderByDescending(package => package.Updated)
            .ToList();

        // not found any possible match or candidate to duplicate
        if (packages.Count == 0)
        {
            throw ErrorDefinition.NotFound().AsException();
        }

        // try finding a duplicate
        var duplicate = packages.FirstOrDefault(package => IsSamePackage(package, input));

        // we found an exact match no need to continue
        if (duplicate != null)
        {
            return duplicate;
        }

        // try finding a package with matching registry to speed up the copy process
        var candidate = packages.FirstOrDefault(package => package.RegistryId == input.RegistryId);

        // no matching candidate found in the same registry so just use first available
        candidate ??= packages.First();

        // proceed with the candidate
        return await this.DuplicateFromImpl(candidate, input);
    }

    /// <summary>
    /// Implementation of package duplication
    /// </summary>
    /// <param name="candidate">The candidate to duplicate</param>
    /// <param name="input">The input for creation</param>
    /// <returns></returns>
    private async Task<PackageModel> DuplicateFromImpl(PackageModel candidate, PackageDuplicateFromModel input)
    {
        // get registry for the target package
        var targetRegistry = await this.registryHandlerService.GetRegistryById(input.RegistryId);
        
        // gets the push credential for the target registry
        var targetCredentials = await this.registryHandlerService.GetPushCredential(input.RegistryId, input.WorkspaceId, input.UserId);
        
        // gets the pull credential for the source registry
        var sourceCredentials = await this.registryHandlerService.GetPullCredential(candidate.RegistryId, candidate.WorkspaceId, candidate.UserId);
        
        // build the future package id
        var packageId = StdIdGenerator.Next(PackageObjects.PACKAGE).ToLowerInvariant();
        
        // build the target image tag
        var targetImage = this.registryHandlerService.BuildImageTag(new RegistryImageContext
        {
            TargetWorkspaceId = input.WorkspaceId,
            TargetUserId = input.UserId,
            Provider = targetRegistry.Provider,
            Registry = targetRegistry.Registry,
            Namespace = targetRegistry.Namespace,
            TargetPackageScope = input.Scope,
            TargetPackageId = packageId
        });
        
        // the source context
        var sourceContext = new ContainerCopyContext
        {
            Image = candidate.Image,
            Username = sourceCredentials.Username,
            Password = sourceCredentials.PasswordPlain
        };
        
        // the target context
        var targetContext = new ContainerCopyContext
        {
            Image = targetImage,
            Username = targetCredentials.Username,
            Password = targetCredentials.PasswordPlain
        };

        // perform copy operation
        var copyResult = await this.containerService.Copy(sourceContext, targetContext);

        // copy failed
        if (!copyResult.Success)
        {
            throw ErrorDefinition.Validation(PackageErrors.IMAGE_COPY_ERROR, copyResult.Error).AsException();
        }
        
        // if everything is okay create a new package
        return await this.packageRepository.Create(input.WorkspaceId, new PackageCreateModel
        {
            Id = packageId,
            WorkspaceId = input.WorkspaceId,
            UserId = input.UserId,
            Scope = input.Scope,
            ListingChecksum = input.ListingChecksum,
            Manifest = candidate.Manifest,
            Runtime = candidate.Runtime,
            Containerfile = candidate.Containerfile,
            TemplateReference = candidate.TemplateReference,
            RegistryId = input.RegistryId,
            Image = targetImage
        });
    }

    /// <summary>
    /// Checks if the given package matches the given duplication request
    /// </summary>
    /// <param name="package">The package</param>
    /// <param name="input">The duplication request input</param>
    /// <returns></returns>
    private static bool IsSamePackage(PackageModel package, PackageDuplicateFromModel input)
    {
        // registry is not same
        if (package.RegistryId != input.RegistryId)
        {
            return false;
        }

        // check different scopes
        if (package.Scope != input.Scope)
        {
            return false;
        }

        // in case of workspace scope it's the same package regardless of the creator
        if (package.Scope == PackageScopes.WORKSPACE)
        {
            return true;
        }
        
        // otherwise it's the same package only if creators are same
        return package.UserId == input.UserId;
    }
}