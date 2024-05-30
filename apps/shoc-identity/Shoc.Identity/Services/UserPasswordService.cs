using System.Threading.Tasks;
using Shoc.Core;
using Shoc.Core.Security;
using Shoc.Identity.Model;
using Shoc.Identity.Model.User;
using Shoc.Identity.Provider.Data;

namespace Shoc.Identity.Services;

/// <summary>
/// The user password service
/// </summary>
public class UserPasswordService
{
    /// <summary>
    /// The minimum length of password
    /// </summary>
    private static readonly int MIN_PASSWORD_LENGTH = 6;

    /// <summary>
    /// The user internal repository
    /// </summary>
    private readonly IUserInternalRepository userInternalRepository;

    /// <summary>
    /// The password hasher
    /// </summary>
    private readonly IPasswordHasher passwordHasher;

    /// <summary>
    /// Creates new password service
    /// </summary>
    /// <param name="userInternalRepository">The internal user repository</param>
    /// <param name="passwordHasher">The password hasher</param>
    public UserPasswordService(IUserInternalRepository userInternalRepository, IPasswordHasher passwordHasher)
    {
        this.userInternalRepository = userInternalRepository;
        this.passwordHasher = passwordHasher;
    }

    /// <summary>
    /// Change the password of the given user
    /// </summary>
    /// <param name="id">The id of user to request</param>
    /// <param name="input">The password update model</param>
    /// <returns></returns>
    public async Task<UserInternalModel> UpdatePasswordById(string id, UserPasswordUpdateModel input)
    {
        // get the user 
        var user = await this.userInternalRepository.GetById(id);

        // make sure user exists
        if (user == null)
        {
            throw ErrorDefinition.NotFound().AsException();
        }

        // if password does not match with confirmation
        if (!string.Equals(input.PasswordConfirmation, input.Password))
        {
            throw ErrorDefinition.Validation(IdentityErrors.MISMATCH_PASSWORD).AsException();
        }

        // validate the password
        await this.Validate(input.Password);

        // update the password in the database
        return await this.userInternalRepository.UpdatePasswordById(id, this.passwordHasher.Hash(input.Password).AsHash());
    }

    /// <summary>
    /// Validates the password
    /// </summary>
    /// <param name="password">The password</param>
    /// <returns></returns>
    public Task Validate(string password)
    {
        // checks if email is empty
        if (string.IsNullOrWhiteSpace(password))
        {
            throw ErrorDefinition.Validation(IdentityErrors.EMPTY_PASSWORD).AsException();
        }

        // checks if password is too short
        if (password.Length < MIN_PASSWORD_LENGTH)
        {
            throw ErrorDefinition.Validation(IdentityErrors.WEAK_PASSWORD).AsException();
        }

        return Task.CompletedTask;
    }
}