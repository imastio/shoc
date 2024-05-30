using System.Collections.Generic;
using System.Threading.Tasks;
using Shoc.Core;
using Shoc.Identity.Model;
using Shoc.Identity.Model.Roles;
using Shoc.Identity.Model.User;
using Shoc.Identity.Provider.Data;

namespace Shoc.Identity.Services;

/// <summary>
/// The role members service.
/// </summary>
public class RoleMembersService : RoleServiceBase
{
    /// <summary>
    /// The role member repository.
    /// </summary>
    private readonly IRoleMemberRepository roleMemberRepository;

    /// <summary>
    /// The user internal repository
    /// </summary>
    private readonly IUserInternalRepository userInternalRepository;

    /// <summary>
    /// Creates an instance of role members service.
    /// </summary>
    /// <param name="roleRepository">The role repository.</param>
    /// <param name="roleMemberRepository">The role member repository</param>
    /// <param name="userInternalRepository">The user internal repository</param>
    public RoleMembersService(IRoleRepository roleRepository, IRoleMemberRepository roleMemberRepository, IUserInternalRepository userInternalRepository) : base(roleRepository)
    {
        this.roleMemberRepository = roleMemberRepository;
        this.userInternalRepository = userInternalRepository;
    }

    /// <summary>
    /// Gets all the member users by the role id.
    /// </summary>
    /// <param name="roleId">The role id.</param>
    /// <returns>A task containing the users of the specified role.</returns>
    public async Task<IEnumerable<UserReferentialValueModel>> GetAll(string roleId)
    {
        // make sure the role exists
        await this.RequireById(roleId);

        // get all users of the role
        return await this.roleMemberRepository.GetAll(roleId);
    }

    /// <summary>
    /// Gets the particular user in the role.
    /// </summary>
    /// <param name="roleId">The role id.</param>
    /// <param name="userId">The user id.</param>
    /// <returns>A task containing the role user.</returns>
    public async Task<UserReferentialValueModel> GetById(string roleId, string userId)
    {
        // make sure both requirements are fulfilled 
        await Task.WhenAll(this.RequireById(roleId), this.RequireUserById(userId));

        // try to get the role member
        return await this.roleMemberRepository.GetById(roleId, userId);
    }

    /// <summary>
    /// Creates new role user membership record by the input.
    /// </summary>
    /// <param name="roleId">The role id.</param>
    /// <param name="input">The role user membership input.</param>
    /// <returns>A task containing the created role user membership.</returns>
    public async Task<UserReferentialValueModel> Create(string roleId, RoleMembership input)
    {
        // make sure referring to the correct object
        input.RoleId = roleId;

        // get the existing user in the role
        var result = await this.GetById(roleId, input.UserId);

        // throw if the user already exists in the role
        if (result != null)
        {
            throw ErrorDefinition.Validation(IdentityErrors.EXISTING_ROLE_USER).AsException();
        }

        // perform the operation
        return await this.roleMemberRepository.Create(input);
    }

    /// <summary>
    /// Deletes the member user from the role.
    /// </summary>
    /// <param name="roleId">The role id.</param>
    /// <param name="userId">The user id.</param>
    /// <returns>A task containing the deleted user from the role.</returns>
    public async Task<UserReferentialValueModel> DeleteById(string roleId, string userId)
    {
        // make sure both requirements are fulfilled 
        await Task.WhenAll(this.RequireById(roleId), this.RequireUserById(userId));

        // perform and return the result 
        return await this.roleMemberRepository.DeleteById(roleId, userId);
    }

    /// <summary>
    /// Requires the user by the given id.
    /// </summary>
    /// <param name="userId">The user id.</param>
    /// <returns>A task containing the user.</returns>
    private async Task<UserInternalModel> RequireUserById(string userId)
    {
        // try get the user by id
        var result = await this.userInternalRepository.GetById(userId);

        // check if exists
        if (result == null)
        {
            throw ErrorDefinition.Validation(IdentityErrors.NO_USER).AsException();
        }

        return result;
    }
}
