using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;
using Shoc.ApiCore.GrpcClient;
using Shoc.Core;
using Shoc.Registry.Data;
using Shoc.Registry.Model;
using Shoc.Registry.Model.Credential;

namespace Shoc.Registry.Services;

/// <summary>
/// The registry credential service
/// </summary>
public class RegistryCredentialService : RegistryServiceBase
{
    /// <summary>
    /// The maximum username length
    /// </summary>
    private const int MAX_USERNAME_LENGTH = 256;

    /// <summary>
    /// The maximum email length
    /// </summary>
    private const int MAX_EMAIL_LENGTH = 256;

    /// <summary>
    /// The maximum password length
    /// </summary>
    private const int MAX_PASSWORD_LENGTH = 4096;

    /// <summary>
    /// The password protection purpose
    /// </summary>
    private const string PASSWORD_PROTECTION_PURPOSE = "password_protection";
    
    /// <summary>
    /// The registry credential repository
    /// </summary>
    private readonly IRegistryCredentialRepository registryCredentialRepository;

    /// <summary>
    /// The registry credential service
    /// </summary>
    /// <param name="registryCredentialRepository">The registry credential repository</param>
    /// <param name="registryRepository">The registry repository</param>
    /// <param name="grpcClientProvider">The grpc client provider</param>
    /// <param name="dataProtectionProvider">The data protection provider</param>
    public RegistryCredentialService(IRegistryCredentialRepository registryCredentialRepository, IRegistryRepository registryRepository, IGrpcClientProvider grpcClientProvider, IDataProtectionProvider dataProtectionProvider) : base(registryRepository, grpcClientProvider, dataProtectionProvider)
    {
        this.registryCredentialRepository = registryCredentialRepository;
    }

    /// <summary>
    /// Gets all the objects
    /// </summary>
    /// <param name="registryId">The registry id</param>
    /// <returns></returns>
    public Task<IEnumerable<RegistryCredentialModel>> GetAll(string registryId)
    {
        return this.registryCredentialRepository.GetAll(registryId);
    }

    /// <summary>
    /// Gets all the objects with filter
    /// </summary>
    /// <param name="registryId">The registry id</param>
    /// <param name="filter">The filter to apply</param>
    /// <returns></returns>
    public Task<IEnumerable<RegistryCredentialModel>> GetBy(string registryId, RegistryCredentialFilter filter)
    {
        return this.registryCredentialRepository.GetBy(registryId, filter);
    }

    /// <summary>
    /// Gets all the objects
    /// </summary>
    /// <param name="registryId">The registry id</param>
    /// <returns></returns>
    public Task<IEnumerable<RegistryCredentialExtendedModel>> GetAllExtended(string registryId)
    {
        return this.registryCredentialRepository.GetAllExtended(registryId);
    }

    /// <summary>
    /// Gets all the extended objects by filter
    /// </summary>
    /// <param name="registryId">The registry id</param>
    /// <param name="filter">The filter to apply</param>
    /// <returns></returns>
    public Task<IEnumerable<RegistryCredentialExtendedModel>> GetExtendedBy(string registryId, RegistryCredentialFilter filter)
    {
        return this.registryCredentialRepository.GetExtendedBy(registryId, filter);
    }

    /// <summary>
    /// Gets the object by id
    /// </summary>
    /// <param name="registryId">The registry id</param>
    /// <param name="id">The record id</param>
    /// <returns></returns>
    public async Task<RegistryCredentialModel> GetById(string registryId, string id)
    {
        // require parent object to exist
        await this.RequireRegistryById(registryId);

        // id should be a valid string
        if (string.IsNullOrWhiteSpace(id))
        {
            throw ErrorDefinition.NotFound().AsException();
        }
        
        // get the object by id
        var result = await this.registryCredentialRepository.GetById(registryId, id);

        // check if result exist
        if (result == null)
        {
            throw ErrorDefinition.NotFound().AsException();
        }

        return result;
    }

    /// <summary>
    /// Creates the object with given input
    /// </summary>
    /// <param name="registryId">The parent object id</param>
    /// <param name="input">The creation input</param>
    /// <returns></returns>
    public async Task<RegistryCredentialModel> Create(string registryId, RegistryCredentialCreateModel input)
    {
        // ensure referring the correct object
        input.RegistryId = registryId;

        // require the registry
        await this.RequireRegistryById(input.RegistryId);

        // validate the source
        ValidateSource(input.Source);
        
        // validate the username
        ValidateUsername(input.Username);
        
        // validate the email
        ValidateEmail(input.Email);
        
        // validate the password
        ValidatePassword(input.Password);

        // create a protector
        var protector = this.dataProtectionProvider.CreateProtector(PASSWORD_PROTECTION_PURPOSE);
        
        // protect and initialize the encrypted password field
        input.PasswordEncrypted = protector.Protect(input.Password);
        
        // if workspace is given we should require to be an existing object
        if (!string.IsNullOrWhiteSpace(input.WorkspaceId))
        {
            await this.RequireWorkspace(input.WorkspaceId);
        }
        
        // if user is given we should require to be an existing object
        if (!string.IsNullOrWhiteSpace(input.UserId))
        {
            await this.RequireUser(input.UserId);
        }

        // if both workspace and user are given we need to require user to be a member of the workspace
        if (!string.IsNullOrWhiteSpace(input.WorkspaceId) && !string.IsNullOrWhiteSpace(input.UserId))
        {
            await this.RequireMembership(input.WorkspaceId, input.UserId);
        }

        // create in the storage
        return await this.registryCredentialRepository.Create(input);
    }

    /// <summary>
    /// Updates the object by id
    /// </summary>
    /// <param name="registryId">The registry id</param>
    /// <param name="id">The object id</param>
    /// <param name="input">The update input</param>
    /// <returns></returns>
    public async Task<RegistryCredentialModel> UpdateById(string registryId, string id, RegistryCredentialUpdateModel input)
    {
        // ensure referring to the same object
        input.RegistryId = registryId;
        input.Id = id;
        
        // require the parent object
        await this.RequireRegistryById(input.RegistryId);
        
        // validate email
        ValidateEmail(input.Email);
        
        // validate username
        ValidateUsername(input.Username);
        
        // update in the storage
        return await this.registryCredentialRepository.UpdateById(registryId, id, input);
    }

    /// <summary>
    /// Updates the object password by id
    /// </summary>
    /// <param name="registryId">The registry id</param>
    /// <param name="id">The object id</param>
    /// <param name="input">The update input</param>
    /// <returns></returns>
    public async Task<RegistryCredentialModel> UpdatePasswordById(string registryId, string id, RegistryCredentialPasswordUpdateModel input)
    {
        // ensure referring to the same object
        input.RegistryId = registryId;
        input.Id = id;
        
        // require the parent object
        await this.RequireRegistryById(input.RegistryId);
        
        // validate password
        ValidatePassword(input.Password);
        
        // create a protector
        var protector = this.dataProtectionProvider.CreateProtector(PASSWORD_PROTECTION_PURPOSE);
        
        // protect and initialize the encrypted password field
        input.PasswordEncrypted = protector.Protect(input.Password);
        
        // update in the storage
        return await this.registryCredentialRepository.UpdatePasswordById(registryId, id, input);
    }

    /// <summary>
    /// Deletes the object by id
    /// </summary>
    /// <param name="registryId">The registry id</param>
    /// <param name="id">The id of the object</param>
    /// <returns></returns>
    public async Task<RegistryCredentialModel> DeleteById(string registryId, string id)
    {
        // require parent object
        await this.RequireRegistryById(registryId);

        // check if object id is given
        if (string.IsNullOrWhiteSpace(id))
        {
            throw ErrorDefinition.NotFound().AsException();
        }

        // try performing operation and get the result
        var result = await this.registryCredentialRepository.DeleteById(registryId, id);

        // check if successful
        if (result == null)
        {
            throw ErrorDefinition.NotFound().AsException();
        }

        return result;
    }
    
    /// <summary>
    /// Validates the given source
    /// </summary>
    /// <param name="source">The source to validate</param>
    private static void ValidateSource(string source)
    {
        // ensure source is defined
        if (!RegistryCredentialSources.ALL.Contains(source))
        {
            throw ErrorDefinition.Validation(RegistryErrors.INVALID_CREDENTIAL_SOURCE).AsException();
        }
    }

    /// <summary>
    /// Validates the username
    /// </summary>
    /// <param name="username">The username to validate</param>
    private static void ValidateUsername(string username)
    {
        // if no username is given or length exceeds the maximum valid raise an error
        if (string.IsNullOrWhiteSpace(username) || username.Length > MAX_USERNAME_LENGTH)
        {
            throw ErrorDefinition.Validation(RegistryErrors.INVALID_CREDENTIAL_USERNAME).AsException();
        }
    }
    
    /// <summary>
    /// Validates the email
    /// </summary>
    /// <param name="email">The email to validate</param>
    private static void ValidateEmail(string email)
    {
        // empty email is valid
        if (string.IsNullOrWhiteSpace(email))
        {
            return;
        }

        // if email length exceeds the maximum raise an error
        if (email.Length > MAX_EMAIL_LENGTH)
        {
            throw ErrorDefinition.Validation(RegistryErrors.INVALID_CREDENTIAL_EMAIL).AsException();
        }
    }
    
    /// <summary>
    /// Validates the password
    /// </summary>
    /// <param name="password">The password to validate</param>
    private static void ValidatePassword(string password)
    {
        // if no password is given or length exceeds the maximum valid raise an error
        if (string.IsNullOrWhiteSpace(password) || password.Length > MAX_PASSWORD_LENGTH)
        {
            throw ErrorDefinition.Validation(RegistryErrors.INVALID_CREDENTIAL_PASSWORD).AsException();
        }
    }
}