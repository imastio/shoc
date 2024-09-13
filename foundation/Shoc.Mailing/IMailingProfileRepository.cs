using System.Collections.Generic;
using System.Threading.Tasks;
using Shoc.Mailing.Model;

namespace Shoc.Mailing;

/// <summary>
/// The mailing profile interface
/// </summary>
public interface IMailingProfileRepository
{
    /// <summary>
    /// Gets all mailing profiles
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<MailingProfile>> GetAll();

    /// <summary>
    /// Gets mailing profile by id
    /// </summary>
    /// <param name="id">The profile id</param>
    /// <returns></returns>
    Task<MailingProfile> GetById(string id);

    /// <summary>
    /// Gets mailing profile by code
    /// </summary>
    /// <param name="code">The profile code</param>
    /// <returns></returns>
    Task<MailingProfile> GetByCode(string code);

    /// <summary>
    /// Creates a new mailing profile
    /// </summary>
    /// <param name="input">The creation input</param>
    /// <returns></returns>
    Task<MailingProfile> Create(CreateMailingProfileInput input);

    /// <summary>
    /// Updates the password of the new mailing profile
    /// </summary>
    /// <param name="input">The update input</param>
    /// <returns></returns>
    Task<MailingProfile> UpdatePassword(UpdateProfilePasswordInput input);

    /// <summary>
    /// Updates the API Secret of the new mailing profile
    /// </summary>
    /// <param name="input">The update input</param>
    /// <returns></returns>
    Task<MailingProfile> UpdateApiSecret(UpdateProfileApiSecretInput input);

    /// <summary>
    /// Deletes mailing profile by id
    /// </summary>
    /// <param name="id">The profile id</param>
    /// <returns></returns>
    Task<MailingProfile> DeleteById(string id);
}
