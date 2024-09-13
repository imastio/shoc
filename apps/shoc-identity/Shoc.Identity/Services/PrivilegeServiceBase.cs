using System.Threading.Tasks;
using Shoc.Core;
using Shoc.Identity.Model;
using Shoc.Identity.Model.Privileges;
using Shoc.Identity.Provider.Data;

namespace Shoc.Identity.Services;

/// <summary>
/// The base service for privileges.
/// </summary>
public class PrivilegeServiceBase
{
    /// <summary>
    /// The privilege repository.
    /// </summary>
    protected readonly IPrivilegeRepository privilegeRepository;

    /// <summary>
    /// Creates new instance of privilege service base.
    /// </summary>
    /// <param name="privilegeRepository">The privilege repository.</param>
    protected PrivilegeServiceBase(IPrivilegeRepository privilegeRepository)
    {
        this.privilegeRepository = privilegeRepository;
    }

    /// <summary>
    /// Requires the privilege by id.
    /// </summary>
    /// <param name="id">The id of the privilege.</param>
    /// <returns></returns>
    protected async Task<PrivilegeModel> RequireById(string id)
    {
        // try get privilege by id
        var result = await this.privilegeRepository.GetById(id);

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
        if (string.IsNullOrWhiteSpace(name) || name.Length > PrivilegeConstants.NAME_MAX_LENGTH)
        {
            throw ErrorDefinition.Validation(IdentityErrors.INVALID_PRIVILEGE_NAME).AsException();
        }
    }

    /// <summary>
    /// Validates the category.
    /// </summary>
    /// <param name="category">The category.</param>
    protected static void ValidateCategory(string category)
    {
        // throw error if category is null, empty or the length exceeds maximum allowed length
        if (string.IsNullOrWhiteSpace(category) || category.Length > PrivilegeConstants.CATEGORY_MAX_LENGTH)
        {
            throw ErrorDefinition.Validation(IdentityErrors.INVALID_PRIVILEGE_CATEGORY).AsException();
        }
    }

    /// <summary>
    /// Validates the description.
    /// </summary>
    /// <param name="description">The description.</param>
    protected static void ValidateDescription(string description)
    {
        // throw error if description length exceeds maximum allowed length
        if (description?.Length > PrivilegeConstants.DESCRIPTION_MAX_LENGTH)
        {
            throw ErrorDefinition.Validation(IdentityErrors.INVALID_PRIVILEGE_DESCRIPTION).AsException();
        }
    }
}

