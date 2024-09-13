using System.Collections.Generic;
using System.Threading.Tasks;
using Imast.DataOps.Api;
using Shoc.Core;
using Shoc.Mailing.Model;

namespace Shoc.Mailing.Sql;

/// <summary>
/// The mailing profile repository
/// </summary>
public class MailingProfileRepository : IMailingProfileRepository
{
    /// <summary>
    /// The data ops reference
    /// </summary>
    private readonly DataOperations dataOps;

    /// <summary>
    /// Creates new instance of the repository
    /// </summary>
    /// <param name="dataOps">The data operations</param>
    public MailingProfileRepository(DataOperations dataOps)
    {
        this.dataOps = dataOps;
    }

    /// <summary>
    /// Gets all mailing profiles
    /// </summary>
    /// <returns></returns>
    public Task<IEnumerable<MailingProfile>> GetAll()
    {
        return Guard.DoAsync(() => this.dataOps.Connect().Query("Mailing", "GetAll").ExecuteAsync<MailingProfile>());
    }

    /// <summary>
    /// Gets mailing profile by id
    /// </summary>
    /// <param name="id">The profile id</param>
    /// <returns></returns>
    public Task<MailingProfile> GetById(string id)
    {
        return Guard.DoAsync(() => this.dataOps.Connect().QueryFirst("Mailing", "GetById").ExecuteAsync<MailingProfile>(new
        {
            Id = id
        }));
    }

    /// <summary>
    /// Gets mailing profile by code
    /// </summary>
    /// <param name="code">The profile code</param>
    /// <returns></returns>
    public Task<MailingProfile> GetByCode(string code)
    {
        return Guard.DoAsync(() => this.dataOps.Connect().QueryFirst("Mailing", "GetByCode").ExecuteAsync<MailingProfile>(new
        {
            Code = code
        }));
    }

    /// <summary>
    /// Creates a new mailing profile
    /// </summary>
    /// <param name="input">The creation input</param>
    /// <returns></returns>
    public Task<MailingProfile> Create(CreateMailingProfileInput input)
    {
        // create an id before saving 
        input.Id ??= StdIdGenerator.Next(MailingObjects.MAILING_PROFILE);

        return Guard.DoAsync(() => this.dataOps.Connect().QueryFirst("Mailing", "Create").ExecuteAsync<MailingProfile>(input));
    }

    /// <summary>
    /// Updates the password of the new mailing profile
    /// </summary>
    /// <param name="input">The update input</param>
    /// <returns></returns>
    public Task<MailingProfile> UpdatePassword(UpdateProfilePasswordInput input)
    {
        return Guard.DoAsync(() => this.dataOps.Connect().QueryFirst("Mailing", "UpdatePassword").ExecuteAsync<MailingProfile>(input));

    }

    /// <summary>
    /// Updates the API Secret of the new mailing profile
    /// </summary>
    /// <param name="input">The update input</param>
    /// <returns></returns>
    public Task<MailingProfile> UpdateApiSecret(UpdateProfileApiSecretInput input)
    {
        return Guard.DoAsync(() => this.dataOps.Connect().QueryFirst("Mailing", "UpdateApiSecret").ExecuteAsync<MailingProfile>(input));

    }

    /// <summary>
    /// Deletes mailing profile by id
    /// </summary>
    /// <param name="id">The profile id</param>
    /// <returns></returns>
    public Task<MailingProfile> DeleteById(string id)
    {
        return Guard.DoAsync(() => this.dataOps.Connect().QueryFirst("Mailing", "DeleteById").ExecuteAsync<MailingProfile>(new
        {
            Id = id
        }));
    }
}
