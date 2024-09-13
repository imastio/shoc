using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Shoc.Identity.Model;

/// <summary>
/// The set of system defined accesses
/// </summary>
public static class IdentityAccesses
{
    /// <summary>
    /// The users read access
    /// </summary>
    public const string IDENTITY_USERS_READ = "identity:users:read";

    /// <summary>
    /// The users list access
    /// </summary>
    public const string IDENTITY_USERS_LIST = "identity:users:list";

    /// <summary>
    /// The users list references access
    /// </summary>
    public const string IDENTITY_USERS_LIST_REFERENCES = "identity:users:list_references";

    /// <summary>
    /// The users create access
    /// </summary>
    public const string IDENTITY_USERS_CREATE = "identity:users:create";
    
    /// <summary>
    /// The user identity edit access
    /// </summary>
    public const string IDENTITY_USERS_EDIT_IDENTITY = "identity:users:edit_identity";
    
    /// <summary>
    /// The user profile edit access (edit name, personal details)
    /// </summary>
    public const string IDENTITY_USERS_EDIT_PROFILE = "identity:users:edit_profile";
    
    /// <summary>
    /// The user manage edit access (sensitive information)
    /// </summary>
    public const string IDENTITY_USERS_MANAGE = "identity:users:manage";
    
    /// <summary>
    /// The user manage edit access to access info
    /// </summary>
    public const string IDENTITY_USERS_MANAGE_ACCESS = "identity:users:manage_access";
    
    /// <summary>
    /// Manage a user
    /// </summary>
    public const string IDENTITY_USERS_DELETE = "identity:users:delete";
    
    /// <summary>
    /// The user groups read
    /// </summary>
    public const string IDENTITY_USER_GROUPS_READ = "identity:user_groups:read";

    /// <summary>
    /// The user groups list
    /// </summary>
    public const string IDENTITY_USER_GROUPS_LIST = "identity:user_groups:list";

    /// <summary>
    /// The user groups create access
    /// </summary>
    public const string IDENTITY_USER_GROUPS_CREATE = "identity:user_groups:create";
    
    /// <summary>
    /// The user groups edit access
    /// </summary>
    public const string IDENTITY_USER_GROUPS_EDIT = "identity:user_groups:edit";
    
    /// <summary>
    /// The user groups manage access
    /// </summary>
    public const string IDENTITY_USER_GROUPS_MANAGE = "identity:user_groups:manage";
    
    /// <summary>
    /// The user groups manage access to access info
    /// </summary>
    public const string IDENTITY_USER_GROUPS_MANAGE_ACCESS = "identity:user_groups:manage_access";
    
    /// <summary>
    /// The user groups delete access
    /// </summary>
    public const string IDENTITY_USER_GROUPS_DELETE = "identity:user_groups:delete";

    /// <summary>
    /// The privileges read access.
    /// </summary>
    public const string IDENTITY_PRIVILEGES_READ = "identity:privileges:read";

    /// <summary>
    /// The privileges list access.
    /// </summary>
    public const string IDENTITY_PRIVILEGES_LIST = "identity:privileges:list";

    /// <summary>
    /// The privileges list references access.
    /// </summary>
    public const string IDENTITY_PRIVILEGES_LIST_REFERENCES = "identity:privileges:list_references";

    /// <summary>
    /// The privileges create access.
    /// </summary>
    public const string IDENTITY_PRIVILEGES_CREATE = "identity:privileges:create";

    /// <summary>
    /// The privileges edit access.
    /// </summary>
    public const string IDENTITY_PRIVILEGES_EDIT = "identity:privileges:edit";
    
    /// <summary>
    /// The privileges manage access.
    /// </summary>
    public const string IDENTITY_PRIVILEGE_MANAGE_ACCESS = "identity:privileges:manage_access";

    /// <summary>
    /// The privileges delete access.
    /// </summary>
    public const string IDENTITY_PRIVILEGES_DELETE = "identity:privileges:delete";

    /// <summary>
    /// The roles read access.
    /// </summary>
    public const string IDENTITY_ROLES_READ = "identity:roles:read";

    /// <summary>
    /// The roles list access.
    /// </summary>
    public const string IDENTITY_ROLES_LIST = "identity:roles:list";

    /// <summary>
    /// The roles list references access.
    /// </summary>
    public const string IDENTITY_ROLES_LIST_REFERENCES = "identity:roles:list_references";

    /// <summary>
    /// The roles create access.
    /// </summary>
    public const string IDENTITY_ROLES_CREATE = "identity:roles:create";

    /// <summary>
    /// The roles edit access.
    /// </summary>
    public const string IDENTITY_ROLES_EDIT = "identity:roles:edit";

    /// <summary>
    /// The roles manage access.
    /// </summary>
    public const string IDENTITY_ROLES_MANAGE = "identity:roles:manage";

    /// <summary>
    /// The roles delete access.
    /// </summary>
    public const string IDENTITY_ROLES_DELETE = "identity:roles:delete";
    
    /// <summary>
    /// The applications read
    /// </summary>
    public const string IDENTITY_APPLICATIONS_READ = "identity:applications:read";

    /// <summary>
    /// The applications list
    /// </summary>
    public const string IDENTITY_APPLICATIONS_LIST = "identity:applications:list";

    /// <summary>
    /// The applications list references
    /// </summary>
    public const string IDENTITY_APPLICATIONS_LIST_REFERENCES = "identity:applications:list_references";

    /// <summary>
    /// The applications manage
    /// </summary>
    public const string IDENTITY_APPLICATIONS_MANAGE = "identity:applications:manage";
   
    /// <summary>
    /// Get and initialize all the constants
    /// </summary>
    public static readonly ISet<string> ALL = GetAll();

    /// <summary>
    /// Gets all the constant values
    /// </summary>
    /// <returns></returns>
    private static ISet<string> GetAll()
    {
        return typeof(IdentityAccesses)
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(f => !f.IsInitOnly && f.IsLiteral && f.FieldType == typeof(string))
            .Select(f => f.GetRawConstantValue() as string)
            .ToHashSet();
    }
}