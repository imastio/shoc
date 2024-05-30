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
    public const string CONNECT_USERS_READ = "connect:users:read";

    /// <summary>
    /// The users list access
    /// </summary>
    public const string CONNECT_USERS_LIST = "connect:users:list";

    /// <summary>
    /// The users list references access
    /// </summary>
    public const string CONNECT_USERS_LIST_REFERENCES = "connect:users:list_references";

    /// <summary>
    /// The users create access
    /// </summary>
    public const string CONNECT_USERS_CREATE = "connect:users:create";
    
    /// <summary>
    /// The user identity edit access
    /// </summary>
    public const string CONNECT_USERS_EDIT_IDENTITY = "connect:users:edit_identity";
    
    /// <summary>
    /// The user profile edit access (edit name, personal details)
    /// </summary>
    public const string CONNECT_USERS_EDIT_PROFILE = "connect:users:edit_profile";
    
    /// <summary>
    /// The user manage edit access (sensitive information)
    /// </summary>
    public const string CONNECT_USERS_MANAGE = "connect:users:manage";
    
    /// <summary>
    /// The user manage edit access to access info
    /// </summary>
    public const string CONNECT_USERS_MANAGE_ACCESS = "connect:users:manage_access";
    
    /// <summary>
    /// Manage a user
    /// </summary>
    public const string CONNECT_USERS_DELETE = "connect:users:delete";
    
    /// <summary>
    /// The user groups read
    /// </summary>
    public const string CONNECT_USER_GROUPS_READ = "connect:user_groups:read";

    /// <summary>
    /// The user groups list
    /// </summary>
    public const string CONNECT_USER_GROUPS_LIST = "connect:user_groups:list";

    /// <summary>
    /// The user groups create access
    /// </summary>
    public const string CONNECT_USER_GROUPS_CREATE = "connect:user_groups:create";
    
    /// <summary>
    /// The user groups edit access
    /// </summary>
    public const string CONNECT_USER_GROUPS_EDIT = "connect:user_groups:edit";
    
    /// <summary>
    /// The user groups manage access
    /// </summary>
    public const string CONNECT_USER_GROUPS_MANAGE = "connect:user_groups:manage";
    
    /// <summary>
    /// The user groups manage access to access info
    /// </summary>
    public const string CONNECT_USER_GROUPS_MANAGE_ACCESS = "connect:user_groups:manage_access";
    
    /// <summary>
    /// The user groups delete access
    /// </summary>
    public const string CONNECT_USER_GROUPS_DELETE = "connect:user_groups:delete";

    /// <summary>
    /// The privileges read access.
    /// </summary>
    public const string CONNECT_PRIVILEGES_READ = "connect:privileges:read";

    /// <summary>
    /// The privileges list access.
    /// </summary>
    public const string CONNECT_PRIVILEGES_LIST = "connect:privileges:list";

    /// <summary>
    /// The privileges list references access.
    /// </summary>
    public const string CONNECT_PRIVILEGES_LIST_REFERENCES = "connect:privileges:list_references";

    /// <summary>
    /// The privileges create access.
    /// </summary>
    public const string CONNECT_PRIVILEGES_CREATE = "connect:privileges:create";

    /// <summary>
    /// The privileges edit access.
    /// </summary>
    public const string CONNECT_PRIVILEGES_EDIT = "connect:privileges:edit";
    
    /// <summary>
    /// The privileges manage access.
    /// </summary>
    public const string CONNECT_PRIVILEGE_MANAGE_ACCESS = "connect:privileges:manage_access";

    /// <summary>
    /// The privileges delete access.
    /// </summary>
    public const string CONNECT_PRIVILEGES_DELETE = "connect:privileges:delete";

    /// <summary>
    /// The roles read access.
    /// </summary>
    public const string CONNECT_ROLES_READ = "connect:roles:read";

    /// <summary>
    /// The roles list access.
    /// </summary>
    public const string CONNECT_ROLES_LIST = "connect:roles:list";

    /// <summary>
    /// The roles list references access.
    /// </summary>
    public const string CONNECT_ROLES_LIST_REFERENCES = "connect:roles:list_references";

    /// <summary>
    /// The roles create access.
    /// </summary>
    public const string CONNECT_ROLES_CREATE = "connect:roles:create";

    /// <summary>
    /// The roles edit access.
    /// </summary>
    public const string CONNECT_ROLES_EDIT = "connect:roles:edit";

    /// <summary>
    /// The roles manage access.
    /// </summary>
    public const string CONNECT_ROLES_MANAGE = "connect:roles:manage";

    /// <summary>
    /// The roles delete access.
    /// </summary>
    public const string CONNECT_ROLES_DELETE = "connect:roles:delete";
    
    /// <summary>
    /// The applications read
    /// </summary>
    public const string CONNECT_APPLICATIONS_READ = "connect:applications:read";

    /// <summary>
    /// The applications list
    /// </summary>
    public const string CONNECT_APPLICATIONS_LIST = "connect:applications:list";

    /// <summary>
    /// The applications list references
    /// </summary>
    public const string CONNECT_APPLICATIONS_LIST_REFERENCES = "connect:applications:list_references";

    /// <summary>
    /// The applications manage
    /// </summary>
    public const string CONNECT_APPLICATIONS_MANAGE = "connect:applications:manage";
   
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