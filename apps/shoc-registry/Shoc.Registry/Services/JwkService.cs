using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using Shoc.ApiCore.GrpcClient;
using Shoc.Registry.Crypto;

namespace Shoc.Registry.Services;

/// <summary>
/// The JWK key service
/// </summary>
public class JwkService : AuthenticationServiceBase
{
    /// <summary>
    /// Creates new instance of the service
    /// </summary>
    /// <param name="registryService">The registry service</param>
    /// <param name="registrySigningKeyService">The signing key service</param>
    /// <param name="keyProviderService">The key provider service</param>
    /// <param name="grpcClientProvider">The grpc client provider</param>
    public JwkService(RegistryService registryService, RegistrySigningKeyService registrySigningKeyService, KeyProviderService keyProviderService, IGrpcClientProvider grpcClientProvider) 
        : base(registryService, registrySigningKeyService, keyProviderService, grpcClientProvider)
    {
    }
    
    /// <summary>
    /// Gets the set of JWK keys for the given registry (global or workspace)
    /// </summary>
    /// <param name="workspaceName">The workspace name (optional)</param>
    /// <param name="registryName">The registry name</param>
    /// <returns></returns>
    public async Task<IEnumerable<IDictionary<string, object>>> GetJwks(string workspaceName, string registryName)
    {
        // gets the target registry
        var registry = await this.GetRegistry(workspaceName, registryName);
        
        // gets all the keys in the registry
        var payloads = await this.registrySigningKeyService.GetAllPayloads(registry.Id);

        // transform the record into a JWK 
        return payloads.Select(payload =>
        {
            // convert to asymmetric key
            var key = this.keyProviderService.ToSecurityKey(payload);
            
            // convert key to public jwk
            return this.keyProviderService.ToPublicJwk(payload.Algorithm, key);
        });
    }
    
    /// <summary>
    /// Gets the single certificate bundle
    /// </summary>
    /// <param name="workspaceName">The workspace name (optional)</param>
    /// <param name="registryName">The registry name</param>
    /// <returns></returns>
    public async Task<string> GetCertificates(string workspaceName, string registryName)
    {
        // gets the target registry
        var registry = await this.GetRegistry(workspaceName, registryName);
        
        // gets all the keys in the registry
        var payloads = await this.registrySigningKeyService.GetAllPayloads(registry.Id);

        // transform the record into a JWK 
        var certificates = payloads.Select(payload =>
        {
            // convert to asymmetric key
            var key = this.keyProviderService.ToSecurityKey(payload);

            // generate the jwk
            var jwk = JsonWebKeyConverter.ConvertFromSecurityKey(key);
                
            // convert key to public certificate
            return this.keyProviderService.ToCertificate(jwk);
        });

        return this.keyProviderService.CreateBundle(certificates);
    }
}