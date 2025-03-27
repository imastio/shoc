using System;
using System.Threading.Tasks;
using Shoc.Core;
using Shoc.Identity.Model;
using Shoc.Identity.Model.Flow;
using Shoc.Identity.Model.Provider;
using Shoc.Identity.Provider.Data;

namespace Shoc.Identity.Services;

/// <summary>
/// The base service
/// </summary>
public class OidcProviderServiceBase
{
    /// <summary>
    /// The maximum length of the name
    /// </summary>
    private const int MAX_NAME_LENGTH = 256;

    /// <summary>
    /// The maximum length of the code
    /// </summary>
    private const int MAX_CODE_LENGTH = 100;

    /// <summary>
    /// The maximum length of the client id
    /// </summary>
    private const int MAX_CLIENT_ID_LENGTH = 1024;
    
    /// <summary>
    /// The maximum length of the client secret
    /// </summary>
    private const int MAX_CLIENT_SECRET_LENGTH = 4096;

    /// <summary>
    /// The maximum length of the scope
    /// </summary>
    private const int MAX_SCOPE_LENGTH = 512;

    /// <summary>
    /// The default scope
    /// </summary>
    protected const string DEFAULT_SCOPE = "openid email profile";
    
    /// <summary>
    /// The repository
    /// </summary>
    protected readonly IOidcProviderRepository oidcProviderRepository;

    /// <summary>
    /// The protection provider
    /// </summary>
    protected readonly IdentityProviderProtectionProvider protectionProvider;

    /// <summary>
    /// Creates new instance of the service
    /// </summary>
    protected OidcProviderServiceBase(IOidcProviderRepository oidcProviderRepository, IdentityProviderProtectionProvider protectionProvider)
    {
        this.oidcProviderRepository = oidcProviderRepository;
        this.protectionProvider = protectionProvider;
    }

    /// <summary>
    /// Require the object with given identifier to exist
    /// </summary>
    /// <param name="id">The id of the object</param>
    /// <returns></returns>
    protected async Task<OidcProviderModel> RequireById(string id)
    {
        // try load the object
        var existing = await this.oidcProviderRepository.GetById(id);

        // make sure object exists
        if (existing == null)
        {
            throw ErrorDefinition.NotFound().AsException();
        }

        return existing;
    }

    /// <summary>
    /// Validate the name
    /// </summary>
    /// <param name="name">The name</param>
    protected static void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw ErrorDefinition.Validation(IdentityErrors.INVALID_PROVIDER_NAME).AsException();
        }

        if (name.Length > MAX_NAME_LENGTH)
        {
            throw ErrorDefinition.Validation(IdentityErrors.INVALID_PROVIDER_NAME).AsException();
        }
    }
    
    /// <summary>
    /// Validate the code
    /// </summary>
    /// <param name="code">The code</param>
    protected static void ValidateCode(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            throw ErrorDefinition.Validation(IdentityErrors.INVALID_PROVIDER_CODE).AsException();
        }
        
        if (code.Length > MAX_CODE_LENGTH)
        {
            throw ErrorDefinition.Validation(IdentityErrors.INVALID_PROVIDER_CODE).AsException();
        }
    }
    
    /// <summary>
    /// Validate the type
    /// </summary>
    /// <param name="type">The type</param>
    protected static void ValidateType(string type)
    {
        if (string.IsNullOrWhiteSpace(type))
        {
            throw ErrorDefinition.Validation(IdentityErrors.INVALID_PROVIDER_TYPE).AsException();
        }

        if (!IdentityProviders.ALL.Contains(type))
        {
            throw ErrorDefinition.Validation(IdentityErrors.INVALID_PROVIDER_TYPE).AsException();
        }
    }
    
    
    /// <summary>
    /// Validate the scope
    /// </summary>
    /// <param name="scope">The scope</param>
    protected static void ValidateScope(string scope)
    {
        if (string.IsNullOrWhiteSpace(scope))
        {
            throw ErrorDefinition.Validation(IdentityErrors.INVALID_PROVIDER_SCOPE).AsException();
        }
        
        if (scope.Length > MAX_SCOPE_LENGTH)
        {
            throw ErrorDefinition.Validation(IdentityErrors.INVALID_PROVIDER_SCOPE).AsException();
        }
    }
    
    /// <summary>
    /// Validate the client id
    /// </summary>
    /// <param name="clientId">The client id</param>
    protected static void ValidateClientId(string clientId)
    {
        if (string.IsNullOrWhiteSpace(clientId))
        {
            throw ErrorDefinition.Validation(IdentityErrors.INVALID_PROVIDER_CLIENT_ID).AsException();
        }
        
        if (clientId.Length > MAX_CLIENT_ID_LENGTH)
        {
            throw ErrorDefinition.Validation(IdentityErrors.INVALID_PROVIDER_CLIENT_ID).AsException();
        }
    }
    
    /// <summary>
    /// Validate the client secret
    /// </summary>
    /// <param name="clientSecret">The client secret</param>
    protected static void ValidateClientSecret(string clientSecret)
    {
        if (string.IsNullOrWhiteSpace(clientSecret))
        {
            throw ErrorDefinition.Validation(IdentityErrors.INVALID_PROVIDER_CLIENT_SECRET).AsException();
        }
        
        if (clientSecret.Length > MAX_CLIENT_SECRET_LENGTH)
        {
            throw ErrorDefinition.Validation(IdentityErrors.INVALID_PROVIDER_CLIENT_SECRET).AsException();
        }
    }
    
    /// <summary>
    /// Validate the icon url
    /// </summary>
    /// <param name="iconUrl">The icon url</param>
    protected static void ValidateIconUrl(string iconUrl)
    {
        if (string.IsNullOrWhiteSpace(iconUrl))
        {
            return;
        }

        if (Uri.TryCreate(iconUrl, UriKind.Absolute, out _))
        {
            return;
        }
        
        throw ErrorDefinition.Validation(IdentityErrors.INVALID_PROVIDER_ICON_URL).AsException();
    }
    
    /// <summary>
    /// Validate the authority
    /// </summary>
    /// <param name="authority">The authority</param>
    protected static void ValidateAuthority(string authority)
    {
        if (string.IsNullOrWhiteSpace(authority))
        {
            throw ErrorDefinition.Validation(IdentityErrors.INVALID_PROVIDER_ICON_URL).AsException();
        }

        if (!Uri.TryCreate(authority, UriKind.Absolute, out _))
        {
            throw ErrorDefinition.Validation(IdentityErrors.INVALID_PROVIDER_ICON_URL).AsException();
        }
    }

    /// <summary>
    /// Validate the domain name
    /// </summary>
    /// <param name="domainName">The domain name</param>
    protected static void ValidateDomainName(string domainName)
    {
        // check if domain name is valid
        if (Uri.CheckHostName(domainName) != UriHostNameType.Dns)
        {
            throw ErrorDefinition.Validation(IdentityErrors.INVALID_PROVIDER_ICON_URL).AsException();
        }
    }
}