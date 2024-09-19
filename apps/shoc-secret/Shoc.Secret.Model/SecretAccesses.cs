using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Shoc.Secret.Model;

/// <summary>
/// The definitions for accesses of the service
/// </summary>
public class SecretAccesses
{
    /// <summary>
    /// A read access to secret objects
    /// </summary>
    public const string SECRET_SECRETS_READ = "secret:secrets:read";

    /// <summary>
    /// The list access to secret objects
    /// </summary>
    public const string SECRET_SECRETS_LIST = "secret:secrets:list";
    
    /// <summary>
    /// The list references access to secret object references
    /// </summary>
    public const string SECRET_SECRETS_LIST_REFERENCES = "secret:secrets:list_references";

    /// <summary>
    /// A 'create' access to secret objects
    /// </summary>
    public const string SECRET_SECRETS_CREATE = "secret:secrets:create";
    
    /// <summary>
    /// An edit access to secret objects
    /// </summary>
    public const string SECRET_SECRETS_EDIT = "secret:secrets:edit";
    
    /// <summary>
    /// The manage access to secret objects
    /// </summary>
    public const string SECRET_SECRETS_MANAGE = "secret:secrets:manage";
    
    /// <summary>
    /// A delete access to secret objects
    /// </summary>
    public const string SECRET_SECRETS_DELETE = "secret:secrets:delete";
    
    /// <summary>
    /// A read access to user secret objects
    /// </summary>
    public const string SECRET_USER_SECRETS_READ = "secret:user_secrets:read";

    /// <summary>
    /// The list access to user secret objects
    /// </summary>
    public const string SECRET_USER_SECRETS_LIST = "secret:user_secrets:list";
    
    /// <summary>
    /// The list references access to user secret object references
    /// </summary>
    public const string SECRET_USER_SECRETS_LIST_REFERENCES = "secret:user_secrets:list_references";

    /// <summary>
    /// A 'create' access to user secret objects
    /// </summary>
    public const string SECRET_USER_SECRETS_CREATE = "secret:user_secrets:create";
    
    /// <summary>
    /// An edit access to user secret objects
    /// </summary>
    public const string SECRET_USER_SECRETS_EDIT = "secret:user_secrets:edit";
    
    /// <summary>
    /// The manage access to user secret objects
    /// </summary>
    public const string SECRET_USER_SECRETS_MANAGE = "secret:user_secrets:manage";
    
    /// <summary>
    /// A delete access to user secret objects
    /// </summary>
    public const string SECRET_USER_SECRETS_DELETE = "secret:user_secrets:delete";
    
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
        return typeof(SecretAccesses)
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(f => !f.IsInitOnly && f.IsLiteral && f.FieldType == typeof(string))
            .Select(f => f.GetRawConstantValue() as string)
            .ToHashSet();
    }
}