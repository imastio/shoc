using System.Threading.Tasks;
using Shoc.Core;
using Shoc.Identity.Model;
using Shoc.Identity.Model.UserGroup;
using Shoc.Identity.Provider.Data;

namespace Shoc.Identity.Services;

/// <summary>
/// The base class for user group service
/// </summary>
public abstract class UserGroupServiceBase
{
    /// <summary>
    /// The user group repository
    /// </summary>
    protected readonly IUserGroupRepository userGroupRepository;

    /// <summary>
    /// Creates new instance of user group service base
    /// </summary>
    /// <param name="userGroupRepository">The user group repository</param>
    protected UserGroupServiceBase(IUserGroupRepository userGroupRepository)
    {
        this.userGroupRepository = userGroupRepository;
    }

    /// <summary>
    /// Requires the user group by id
    /// </summary>
    /// <param name="id">The id of the group</param>
    /// <returns></returns>
    protected async Task<UserGroupModel> RequireById(string id)
    {
        // try get by id
        var result = await this.userGroupRepository.GetById(id);

        // handle if not found
        if (result == null)
        {
            throw ErrorDefinition.NotFound().AsException();
        }

        return result;
    }

    /// <summary>
    /// Validates the name.
    /// </summary>
    /// <param name="name">The name.</param>
    protected void ValidateName(string name)
    {
        // make sure name is given
        if (string.IsNullOrWhiteSpace(name))
        {
            throw ErrorDefinition.Validation(IdentityErrors.INVALID_GROUP_NAME).AsException();
        }
    }
}