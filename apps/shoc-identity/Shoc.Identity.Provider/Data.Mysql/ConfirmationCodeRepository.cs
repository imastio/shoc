using System.Collections.Generic;
using System.Threading.Tasks;
using Imast.DataOps.Api;
using Shoc.Core;
using Shoc.Identity.Model;
using Shoc.Identity.Model.Flow;

namespace Shoc.Identity.Provider.Data.Mysql;

/// <summary>
/// The confirmation code repository implementation
/// </summary>
public class ConfirmationCodeRepository : IConfirmationCodeRepository
{
    /// <summary>
    /// The data operations instance
    /// </summary>
    private readonly DataOperations dataOps;

    /// <summary>
    /// Creates new instance of confirmation code repository implementation
    /// </summary>
    /// <param name="dataOps">A DataOps instance</param>
    public ConfirmationCodeRepository(DataOperations dataOps)
    {
        this.dataOps = dataOps;
    }

    /// <summary>
    /// Gets active confirmations
    /// </summary>
    /// <returns></returns>
    public Task<IEnumerable<ConfirmationCodeModel>> GetAll(string target, string type)
    {
        return this.dataOps.Connect().Query("Identity.ConfirmationCode", "GetAll").ExecuteAsync<ConfirmationCodeModel>(
            new
            {
                Target = target,
                TargetType = type
            });
    }

    /// <summary>
    /// Gets the confirmation code by link
    /// </summary>
    /// <param name="link">The link fragment</param>
    /// <returns></returns>
    public Task<ConfirmationCodeModel> GetByLink(string link)
    {
        return this.dataOps.Connect().QueryFirst("Identity.ConfirmationCode", "GetByLink")
            .ExecuteAsync<ConfirmationCodeModel>(new
            {
                Link = link
            });
    }

    /// <summary>
    /// Create a confirmation code
    /// </summary>
    /// <param name="input">The code input</param>
    /// <returns></returns>
    public Task<ConfirmationCodeModel> Create(ConfirmationCodeModel input)
    {
        // generate id if necessary
        input.Id ??= StdIdGenerator.Next(IdentityObjects.CNF)?.ToLowerInvariant();

        return this.dataOps.Connect().QueryFirst("Identity.ConfirmationCode", "Create")
            .ExecuteAsync<ConfirmationCodeModel>(input);
    }

    /// <summary>
    /// Delete confirmation code associated by id
    /// </summary>
    /// <param name="id">The id of code</param>
    /// <returns></returns>
    public Task<int> DeleteById(string id)
    {
        return this.dataOps.Connect().NonQuery("Identity.ConfirmationCode", "DeleteById").ExecuteAsync(new
        {
            Id = id
        });
    }

    /// <summary>
    /// Delete confirmation codes
    /// </summary>
    /// <param name="target">The target to confirm</param>
    /// <param name="type">The target type</param>
    /// <returns></returns>
    public Task<int> DeleteAll(string target, string type)
    {
        return this.dataOps.Connect().NonQuery("Identity.ConfirmationCode", "DeleteAll").ExecuteAsync(new
        {
            Target = target,
            TargetType = type
        });
    }
}
