using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using Shoc.Core;
using Shoc.Registry.Model;
using Shoc.Registry.Model.Key;

namespace Shoc.Registry.Crypto;

/// <summary>
/// The utility class for algorithm resolution
/// </summary>
public static class AlgorithmUtility
{
    /// <summary>
    /// Gets the curve name based on algorithm
    /// </summary>
    public static string GetCurveName(string algorithm)
    {
        return algorithm switch
        {
            RegistrySigningKeyAlgorithms.ES256 => JsonWebKeyECTypes.P256,
            RegistrySigningKeyAlgorithms.ES384 => JsonWebKeyECTypes.P384,
            RegistrySigningKeyAlgorithms.ES512 => JsonWebKeyECTypes.P521,
            _ => throw ErrorDefinition.Validation(RegistryErrors.INVALID_KEY_ALGORITHM).AsException()
        };
    }
    
    /// <summary>
    /// Gets the named curve value by name
    /// </summary>
    public static ECCurve GetCurveValue(string curve)
    {
        return curve switch
        {
            JsonWebKeyECTypes.P256 => ECCurve.NamedCurves.nistP256,
            JsonWebKeyECTypes.P384 => ECCurve.NamedCurves.nistP384,
            JsonWebKeyECTypes.P521 => ECCurve.NamedCurves.nistP521,
            _ => throw ErrorDefinition.Validation(RegistryErrors.INVALID_KEY_ALGORITHM).AsException()
        };
    }

    /// <summary>
    /// Gets the key type based on desired signing algorithm
    /// </summary>
    /// <param name="algorithm">The algorithm</param>
    /// <returns></returns>
    /// <exception cref="ShocException"></exception>
    public static string GetKeyType(string algorithm)
    {
        // for the algorithms starting with R or P consider RSA key type
        if (algorithm.StartsWith('R') || algorithm.StartsWith('P'))
        {
            return SigningKeyTypes.RSA;
        }

        // if algorithm starts with E consider EC key type
        if (algorithm.StartsWith('E'))
        {
            return SigningKeyTypes.EC;
        }

        // error otherwise
        throw ErrorDefinition.Validation(RegistryErrors.INVALID_KEY_ALGORITHM).AsException();
    }
}