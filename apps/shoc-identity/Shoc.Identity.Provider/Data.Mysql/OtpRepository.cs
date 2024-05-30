using System.Collections.Generic;
using System.Threading.Tasks;
using Imast.DataOps.Api;
using Shoc.Core;
using Shoc.Identity.Model;
using Shoc.Identity.Model.Flow;

namespace Shoc.Identity.Provider.Data.Mysql;

/// <summary>
/// The OTP repository implementation
/// </summary>
public class OtpRepository : IOtpRepository
{
    /// <summary>
    /// The data operations instance
    /// </summary>
    private readonly DataOperations dataOps;

    /// <summary>
    /// Creates new instance of one-time password repository implementation
    /// </summary>
    /// <param name="dataOps">A DataOps instance</param>
    public OtpRepository(DataOperations dataOps)
    {
        this.dataOps = dataOps;
    }

    /// <summary>
    /// Gets all the one-time passwords associated with user
    /// </summary>
    /// <param name="userId">The user id</param>
    /// <returns></returns>
    public Task<IEnumerable<OneTimePassModel>> GetAll(string userId)
    {
        return this.dataOps.Connect().Query("Identity.Otp", "GetAll").ExecuteAsync<OneTimePassModel>(new
        {
            UserId = userId
        });
    }

    /// <summary>
    /// Gets one-time password by link if exists
    /// </summary>
    /// <param name="link">The link fragment</param>
    /// <returns></returns>
    public Task<OneTimePassModel> GetByLink(string link)
    {
        return this.dataOps.Connect().QueryFirst("Identity.Otp", "GetByLink").ExecuteAsync<OneTimePassModel>(new
        {
            Link = link
        });
    }

    /// <summary>
    /// Creates a one-time password
    /// </summary>
    /// <param name="input">The one-time password instance</param>
    /// <returns></returns>
    public Task<OneTimePassModel> Create(OneTimePassModel input)
    {
        // generate id if necessary
        input.Id ??= StdIdGenerator.Next(IdentityObjects.OTP)?.ToLowerInvariant();

        return this.dataOps.Connect().QueryFirst("Identity.Otp", "Create").ExecuteAsync<OneTimePassModel>(input);
    }

    /// <summary>
    /// Delete one-time passwords associated by id
    /// </summary>
    /// <param name="id">The id of OTP</param>
    /// <returns></returns>
    public Task<int> DeleteById(string id)
    {
        return this.dataOps.Connect().NonQuery("Identity.Otp", "DeleteById").ExecuteAsync(new
        {
            Id = id
        });
    }
}
