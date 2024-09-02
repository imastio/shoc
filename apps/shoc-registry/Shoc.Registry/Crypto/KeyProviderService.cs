using System;
using System.Security.Cryptography;
using System.Text.Json;
using Microsoft.IdentityModel.Tokens;
using Shoc.Core;
using Shoc.Registry.Model;
using Shoc.Registry.Model.Key;

namespace Shoc.Registry.Crypto;

/// <summary>
/// The key provider service
/// </summary>
public class KeyProviderService
{
    /// <summary>
    /// The default key size
    /// </summary>
    private const int DEFAULT_KEY_SIZE = 2048;
    
    /// <summary>
    /// Creates new instance of key provider service
    /// </summary>
    public KeyProviderService()
    {
    }
    
    /// <summary>
    /// Creates new instance of asymmetric security key
    /// </summary>
    /// <param name="algorithm">The algorithm</param>
    /// <param name="keyId">The key id</param>
    /// <returns></returns>
    public BaseKeyPayload Generate(string algorithm, string keyId)
    {
        // current time 
        var now = DateTime.UtcNow;

        // generate a key
        var key = GenerateSecurityKey(algorithm, keyId);

        switch (key)
        {
            // handle when RSA key
            case RsaSecurityKey rsaKey:
            {
                // use parameters from exported RSA (including private) or direct
                var parameters = rsaKey.Rsa?.ExportParameters(true) ?? rsaKey.Parameters;
            
                // fill the payload
                return new RsaKeyPayload
                {
                    KeyId = rsaKey.KeyId,
                    Algorithm = algorithm,
                    Created = now,
                    D = parameters.D,
                    DP = parameters.DP,
                    DQ = parameters.DQ,
                    Exponent = parameters.Exponent,
                    InverseQ = parameters.InverseQ,
                    Modulus = parameters.Modulus,
                    P = parameters.P,
                    Q = parameters.Q
                };
            }
            // handle when EC key
            case ECDsaSecurityKey ecKey:
            {
                // export parameters (including private)
                var parameters = ecKey.ECDsa.ExportParameters(true);

                // build payload
                return new EcKeyPayload
                {
                    KeyId = ecKey.KeyId,
                    Algorithm = algorithm,
                    Created = now,
                    Q = new EcKeyPointPayload
                    {
                        X = parameters.Q.X,
                        Y = parameters.Q.Y
                    },
                    D = parameters.D
                };
            }
            default:
                // not supported otherwise
                throw ErrorDefinition.Validation(RegistryErrors.INVALID_KEY_ALGORITHM).AsException();
        }
    }

    /// <summary>
    /// Serialize the key payload
    /// </summary>
    /// <param name="keyPayload">The key payload to serialize</param>
    /// <returns></returns>
    public string Serialize(BaseKeyPayload keyPayload)
    {
        return keyPayload switch
        {
            RsaKeyPayload rsa => JsonSerializer.Serialize(rsa),
            EcKeyPayload ec => JsonSerializer.Serialize(ec),
            _ => JsonSerializer.Serialize(keyPayload)
        };
    }

    /// <summary>
    /// Deserialize the JSON-serialized key payload
    /// </summary>
    /// <param name="algorithm">The algorithm</param>
    /// <param name="keyPayloadJson">The JSON key payload to deserialize</param>
    /// <returns></returns>
    public BaseKeyPayload Deserialize(string algorithm, string keyPayloadJson)
    {
        // get the type based on the algorithm
        var type = AlgorithmUtility.GetKeyType(algorithm);

        // deserialize based on the type
        return type switch
        {
            SigningKeyTypes.RSA => JsonSerializer.Deserialize<RsaKeyPayload>(keyPayloadJson),
            SigningKeyTypes.EC => JsonSerializer.Deserialize<EcKeyPayload>(keyPayloadJson),
            _ => throw ErrorDefinition.Validation(RegistryErrors.INVALID_KEY_ALGORITHM).AsException()
        };
    }

    /// <summary>
    /// Converts key payload into an asymmetric security key
    /// </summary>
    /// <param name="keyPayload">The key payload</param>
    /// <returns></returns>
    public AsymmetricSecurityKey ToSecurityKey(BaseKeyPayload keyPayload)
    {
        switch (keyPayload)
        {
            // handle when key payload is RSA 
            case RsaKeyPayload rsaKeyPayload:
                return new RsaSecurityKey(new RSAParameters
                {
                    D = rsaKeyPayload.D,
                    DP = rsaKeyPayload.DP,
                    DQ = rsaKeyPayload.DQ,
                    Exponent = rsaKeyPayload.Exponent,
                    InverseQ = rsaKeyPayload.InverseQ,
                    Modulus = rsaKeyPayload.Modulus,
                    P = rsaKeyPayload.P,
                    Q = rsaKeyPayload.Q 
                })
                {
                    KeyId = keyPayload.KeyId
                };
            // handle when key payload is EC 
            case EcKeyPayload ecKeyPayload:
            {
                // build parameters
                var parameters = new ECParameters {
                    Curve = AlgorithmUtility.GetCurveValue(AlgorithmUtility.GetCurveName(keyPayload.Algorithm)),
                    D = ecKeyPayload.D,
                    Q = new ECPoint
                    {
                        X = ecKeyPayload.Q.X,
                        Y = ecKeyPayload.Q.Y
                    }
                };

                // build security key
                return new ECDsaSecurityKey(ECDsa.Create(parameters))
                {
                    KeyId = keyPayload.KeyId
                };
            }
            default:
                // not supported
                throw ErrorDefinition.Validation(RegistryErrors.INVALID_KEY_ALGORITHM).AsException();
        }
    }
    
    /// <summary>
    /// Creates new instance of asymmetric security key
    /// </summary>
    /// <param name="algorithm">The algorithm</param>
    /// <param name="keyId">The key id</param>
    /// <returns></returns>
    private static AsymmetricSecurityKey GenerateSecurityKey(string algorithm, string keyId)
    {
        // get algorithm from the type
        var type = AlgorithmUtility.GetKeyType(algorithm);

        // create a new key based on the key type
        return type switch
        {
            SigningKeyTypes.RSA => new RsaSecurityKey(RSA.Create(DEFAULT_KEY_SIZE))
            {
                KeyId = keyId
            },
            SigningKeyTypes.EC => new ECDsaSecurityKey(ECDsa.Create(AlgorithmUtility.GetCurveValue(AlgorithmUtility.GetCurveName(algorithm))))
            {
                KeyId = keyId
            },
            _ => throw ErrorDefinition.Validation(RegistryErrors.INVALID_KEY_ALGORITHM).AsException()
        };
    }
    
    

    
    
    
        
}