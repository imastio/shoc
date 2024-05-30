using System.Threading.Tasks;
using Shoc.Identity.Model.Flow;

namespace Shoc.Identity.Provider.Data;

/// <summary>
/// The users sign-in history interface
/// </summary>
public interface ISigninHistoryRepository
{
    /// <summary>
    /// Gets the sign-in history
    /// </summary>
    /// <param name="userId">The user id</param>
    /// <param name="page">The page number</param>
    /// <param name="size">The page size</param>
    /// <returns></returns>
    Task<SigninHistoryPageResult> GetAll(string userId, int page, int size);

    /// <summary>
    /// Record the sign-in history
    /// </summary>
    /// <param name="input">The history input</param>
    /// <returns></returns>
    Task<SigninHistoryRecordModel> Create(SigninHistoryRecordModel input);
}