using System.Threading.Tasks;
using Imast.DataOps.Api;
using Shoc.Core;
using Shoc.Identity.Model;
using Shoc.Identity.Model.Flow;

namespace Shoc.Identity.Provider.Data.Mysql;

/// <summary>
/// The users sign-in history repository implementation
/// </summary>
public class SigninHistoryRepository : ISigninHistoryRepository
{
    /// <summary>
    /// The data operations instance
    /// </summary>
    private readonly DataOperations dataOps;

    /// <summary>
    /// Creates new instance of sign-in history repository implementation
    /// </summary>
    /// <param name="dataOps">A DataOps instance</param>
    public SigninHistoryRepository(DataOperations dataOps)
    {
        this.dataOps = dataOps;
    }

    /// <summary>
    /// Gets the sign-in history
    /// </summary>
    /// <param name="userId">The user id</param>
    /// <param name="page">The page number</param>
    /// <param name="size">The page size</param>
    /// <returns></returns>
    public async Task<SigninHistoryPageResult> GetAll(string userId, int page, int size)
    {
        // the argument
        var arg = new
        {
            UserId = userId,
            Offset = page * size,
            Count = size
        };

        // load page of items based on filter
        var items = await this.dataOps.Connect()
            .Query("Identity.SigninHistory", "GetAll")
            .ExecuteAsync<SigninHistoryRecordModel>(arg);

        // count total count separately by now
        // fix the multi-query with binding
        var totalCount = await this.dataOps.Connect()
            .QueryFirst("Identity.SigninHistory", "CountAll")
            .ExecuteAsync<long>(arg);

        return new SigninHistoryPageResult
        {
            Items = items,
            TotalCount = totalCount
        };
    }

    /// <summary>
    /// Record the sign-in history
    /// </summary>
    /// <param name="input">The history input</param>
    /// <returns></returns>
    public Task<SigninHistoryRecordModel> Create(SigninHistoryRecordModel input)
    {
        // generate id if necessary
        input.Id ??= StdIdGenerator.Next(IdentityObjects.HIST)?.ToLowerInvariant();

        return this.dataOps.Connect().QueryFirst("Identity.SigninHistory", "Create").ExecuteAsync<SigninHistoryRecordModel>(input);
    }
}