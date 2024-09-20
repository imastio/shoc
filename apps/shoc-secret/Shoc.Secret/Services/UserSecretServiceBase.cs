using System.Threading.Tasks;
using Shoc.Core;
using Shoc.Secret.Data;
using Shoc.Secret.Model.UserSecret;

namespace Shoc.Secret.Services;

/// <summary>
/// The user secret service base
/// </summary>
public abstract class UserSecretServiceBase
{
    /// <summary>
    /// The user secret repository
    /// </summary>
    protected readonly IUserSecretRepository userSecretRepository;

    /// <summary>
    /// The protection provider
    /// </summary>
    protected readonly UserSecretProtectionProvider protectionProvider;
    
    /// <summary>
    /// The validation service
    /// </summary>
    protected readonly SecretValidationService validationService;

    /// <summary>
    /// Creates a new service
    /// </summary>
    /// <param name="userSecretRepository">The repository</param>
    /// <param name="protectionProvider">The protection provider</param>
    /// <param name="validationService">The validation service</param>
    protected UserSecretServiceBase(IUserSecretRepository userSecretRepository, UserSecretProtectionProvider protectionProvider, SecretValidationService validationService)
    {
        this.userSecretRepository = userSecretRepository;
        this.protectionProvider = protectionProvider;
        this.validationService = validationService;
    }

    /// <summary>
    /// Requires the object by id
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="userId">The user id</param>
    /// <param name="id">The id of the object</param>
    /// <returns></returns>
    protected async Task<UserSecretModel> RequireById(string workspaceId, string userId, string id)
    {
        // id should be a valid string
        if (string.IsNullOrWhiteSpace(id))
        {
            throw ErrorDefinition.NotFound().AsException();
        }

        // try getting object by id
        var result = await this.userSecretRepository.GetById(workspaceId, userId, id);

        // make sure object exists
        if (result == null)
        {
            throw ErrorDefinition.NotFound().AsException();
        }

        // return the object
        return result;
    }
}