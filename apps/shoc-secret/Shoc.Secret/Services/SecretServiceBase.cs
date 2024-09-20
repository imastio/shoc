using System.Threading.Tasks;
using Shoc.Core;
using Shoc.Secret.Data;
using Shoc.Secret.Model.Secret;

namespace Shoc.Secret.Services;

/// <summary>
/// The secret service base
/// </summary>
public abstract class SecretServiceBase
{
    /// <summary>
    /// The secret repository
    /// </summary>
    protected readonly ISecretRepository secretRepository;

    /// <summary>
    /// The protection provider
    /// </summary>
    protected readonly SecretProtectionProvider protectionProvider;
    
    /// <summary>
    /// The validation service
    /// </summary>
    protected readonly SecretValidationService validationService;

    /// <summary>
    /// Creates a new service
    /// </summary>
    /// <param name="secretRepository">The repository</param>
    /// <param name="protectionProvider">The protection provider</param>
    /// <param name="validationService">The validation service</param>
    protected SecretServiceBase(ISecretRepository secretRepository, SecretProtectionProvider protectionProvider, SecretValidationService validationService)
    {
        this.secretRepository = secretRepository;
        this.protectionProvider = protectionProvider;
        this.validationService = validationService;
    }
    
    /// <summary>
    /// Requires the object by id
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="id">The id of the object</param>
    /// <returns></returns>
    protected async Task<SecretModel> RequireById(string workspaceId, string id)
    {
        // id should be a valid string
        if (string.IsNullOrWhiteSpace(id))
        {
            throw ErrorDefinition.NotFound().AsException();
        }

        // try getting object by id
        var result = await this.secretRepository.GetById(workspaceId, id);

        // make sure object exists
        if (result == null)
        {
            throw ErrorDefinition.NotFound().AsException();
        }

        // return the object
        return result;
    }
}