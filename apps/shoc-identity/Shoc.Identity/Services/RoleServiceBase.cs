using System.Threading.Tasks;
using Shoc.Core;
using Shoc.Identity.Model;
using Shoc.Identity.Model.Roles;
using Shoc.Identity.Provider.Data;

namespace Shoc.Identity.Services;

/// <summary>
/// The base class for role service.
/// </summary>
public class RoleServiceBase
{
    /// <summary>
    /// The role repository.
    /// </summary>
    protected readonly IRoleRepository roleRepository;

    /// <summary>
    /// Creates new instance of role service base.
    /// </summary>
    /// <param name="roleRepository">The role repository.</param>
    protected RoleServiceBase(IRoleRepository roleRepository)
    {
        this.roleRepository = roleRepository;
    }

    /// <summary>
    /// Requires the role by id.
    /// </summary>
    /// <param name="id">The id of the role.</param>
    /// <returns>A task containing the role.</returns>
    protected async Task<RoleModel> RequireById(string id)
    {
        // try get role by id
        var result = await this.roleRepository.GetById(id);

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
    protected static void ValidateName(string name)
    {
        // throw error if name is null, empty or the length exceeds maximum allowed length
        if (string.IsNullOrWhiteSpace(name) || name.Length > RoleConstants.NAME_MAX_LENGTH)
        {
            throw ErrorDefinition.Validation(IdentityErrors.INVALID_ROLE_NAME).AsException();
        }
    }

    /// <summary>
    /// Validates the description.
    /// </summary>
    /// <param name="description">The description.</param>
    protected static void ValidateDescription(string description)
    {
        // throw error if description length exceeds maximum allowed length
        if (description?.Length > RoleConstants.DESCRIPTION_MAX_LENGTH)
        {
            throw ErrorDefinition.Validation(IdentityErrors.INVALID_ROLE_DESCRIPTION).AsException();
        }
    }
}
