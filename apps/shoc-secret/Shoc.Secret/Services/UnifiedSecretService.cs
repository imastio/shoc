using System.Collections.Generic;
using System.Threading.Tasks;
using Shoc.Secret.Data;
using Shoc.Secret.Model.UnifiedSecret;

namespace Shoc.Secret.Services;

/// <summary>
/// The unified secret service
/// </summary>
public class UnifiedSecretService
{
    /// <summary>
    /// The repository instance
    /// </summary>
    private readonly IUnifiedSecretRepository unifiedSecretRepository;
    
    /// <summary>
    /// The validation service
    /// </summary>
    private readonly SecretValidationService validationService;

    /// <summary>
    /// Creates new instance of unified secret service
    /// </summary>
    /// <param name="unifiedSecretRepository">The repository instance</param>
    /// <param name="validationService">The validation service</param>
    public UnifiedSecretService(IUnifiedSecretRepository unifiedSecretRepository, SecretValidationService validationService)
    {
        this.unifiedSecretRepository = unifiedSecretRepository;
        this.validationService = validationService;
    }
    
    /// <summary>
    /// Gets objects by names
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<UnifiedSecretModel>> GetByNames(string workspaceId, string userId, IEnumerable<string> names)
    {
        // require the parent object
        await this.validationService.RequireWorkspace(workspaceId);

        // get from the storage
        return await this.unifiedSecretRepository.GetByNames(workspaceId, userId, names);
    }
}