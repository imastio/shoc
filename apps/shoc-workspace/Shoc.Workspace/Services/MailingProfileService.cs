using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;
using Shoc.Core;
using Shoc.Mailing;
using Shoc.Mailing.Model;

namespace Shoc.Settings.Services;

/// <summary>
/// The mailing profile service implementation
/// </summary>
public class MailingProfileService
{
    /// <summary>
    /// The supported mailing providers
    /// </summary>
    private static readonly ISet<string> SUPPORTED_PROVIDERS = new HashSet<string>
    {
        MailProviders.SMTP
    };

    /// <summary>
    /// The mailing profile repository
    /// </summary>
    private readonly IMailingProfileRepository profileRepository;

    /// <summary>
    /// The data protection provider
    /// </summary>
    private readonly IDataProtectionProvider dataProtectionProvider;

    /// <summary>
    /// Creates new instance of mailing profile service
    /// </summary>
    /// <param name="profileRepository">The profile repository</param>
    /// <param name="dataProtectionProvider">The data protection provider</param>
    public MailingProfileService(IMailingProfileRepository profileRepository, IDataProtectionProvider dataProtectionProvider)
    {
        this.profileRepository = profileRepository;
        this.dataProtectionProvider = dataProtectionProvider;
    }

    /// <summary>
    /// Gets all mailing profiles
    /// </summary>
    /// <returns></returns>
    public Task<IEnumerable<MailingProfile>> GetAll()
    {
        return this.profileRepository.GetAll();
    }

    /// <summary>
    /// Gets mailing profile by id
    /// </summary>
    /// <param name="id">The profile id</param>
    /// <returns></returns>
    public async Task<MailingProfile> GetById(string id)
    {
        // try get object by id
        var result = await this.profileRepository.GetById(id);

        // make sure object exists
        if (result == null)
        {
            throw ErrorDefinition.NotFound().AsException();
        }

        return result;
    }

    /// <summary>
    /// Creates a new mailing profile
    /// </summary>
    /// <param name="input">The creation input</param>
    /// <returns></returns>
    public async Task<MailingProfile> Create(CreateMailingProfileInput input)
    {
        // the provider is not supported
        if (!SUPPORTED_PROVIDERS.Contains(input.Provider?? string.Empty))
        {
            throw ErrorDefinition.Validation(MailingErrors.INVALID_PROVIDER).AsException();
        }

        // the code is required
        if (string.IsNullOrWhiteSpace(input.Code))
        {
            throw ErrorDefinition.Validation(MailingErrors.INVALID_CODE).AsException();
        }

        // try load existing item with the given code
        var existing = await this.profileRepository.GetByCode(input.Code);

        // make sure unique
        if (existing != null)
        {
            throw ErrorDefinition.Validation(MailingErrors.EXISTING_CODE).AsException();
        }

        // get the protector for encryption
        var protector = this.dataProtectionProvider.CreateProtector(MailingProtectionConstants.MAILING_CREDENTIALS_PURPOSE);

        // encrypt password if given
        if (!string.IsNullOrWhiteSpace(input.Password))
        {
            input.PasswordEncrypted = protector.Protect(input.Password);
        }

        // encrypt api secret if given
        if (!string.IsNullOrWhiteSpace(input.ApiSecret))
        {
            input.ApiSecretEncrypted = protector.Protect(input.ApiSecret);
        }

        // create in the repository
        return await this.profileRepository.Create(input);
    }

    /// <summary>
    /// Updates the password of the new mailing profile
    /// </summary>
    /// <param name="id">The id of the profile</param>
    /// <param name="input">The update input</param>
    /// <returns></returns>
    public async Task<MailingProfile> UpdatePassword(string id, UpdateProfilePasswordInput input)
    {
        // update exactly same object mentioned by id
        input.Id = id;

        // get requested object to update
        var existing = await this.profileRepository.GetById(id);

        // make sure it exists
        if (existing == null)
        {
            throw ErrorDefinition.NotFound().AsException();
        }

        // get the protector for encryption
        var protector = this.dataProtectionProvider.CreateProtector(MailingProtectionConstants.MAILING_CREDENTIALS_PURPOSE);

        // encrypt password 
        input.PasswordEncrypted = protector.Protect(input.Password ?? string.Empty);

        // update in repository
        return await this.profileRepository.UpdatePassword(input);
    }

    /// <summary>
    /// Updates the API Secret of the new mailing profile
    /// </summary>
    /// <param name="id">The id of the profile</param>
    /// <param name="input">The update input</param>
    /// <returns></returns>
    public async Task<MailingProfile> UpdateApiSecret(string id, UpdateProfileApiSecretInput input)
    {
        // update exactly same object mentioned by id
        input.Id = id;

        // get requested object to update
        var existing = await this.profileRepository.GetById(id);

        // make sure it exists
        if (existing == null)
        {
            throw ErrorDefinition.NotFound().AsException();
        }

        // get the protector for encryption
        var protector = this.dataProtectionProvider.CreateProtector(MailingProtectionConstants.MAILING_CREDENTIALS_PURPOSE);

        // encrypt api secret 
        input.ApiSecretEncrypted = protector.Protect(input.ApiSecret ?? string.Empty);

        // update in repository
        return await this.profileRepository.UpdateApiSecret(input);
    }

    /// <summary>
    /// Deletes mailing profile by id
    /// </summary>
    /// <param name="id">The profile id</param>
    /// <returns></returns>
    public async Task<MailingProfile> DeleteById(string id)
    {
        // try delete object by id
        var result = await this.profileRepository.DeleteById(id);

        // make sure object exists
        if (result == null)
        {
            throw ErrorDefinition.NotFound().AsException();
        }

        return result;
    }
}
