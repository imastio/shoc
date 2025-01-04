using Shoc.Core;
using Shoc.Secret.Grpc.Secrets;
using Shoc.Secret.Model;
using Shoc.Secret.Model.UnifiedSecret;

namespace Shoc.Secret.Grpc;

/// <summary>
/// The grpc object mapper for unified secret
/// </summary>
public class UnifiedSecretGrpcMapper
{
    /// <summary>
    /// Maps to the Grpc representation
    /// </summary>
    /// <param name="input">The input to map</param>
    /// <returns></returns>
    public UnifiedSecretGrpcModel Map(UnifiedSecretModel input)
    {
        return new UnifiedSecretGrpcModel
        {
            Id = input.Id,
            WorkspaceId = input.WorkspaceId,
            UserId = input.UserId ?? string.Empty,
            Kind = MapKind(input.Kind),
            Name = input.Name,
            Description = input.Description,
            Disabled = input.Disabled,
            Encrypted = input.Encrypted,
            Value = input.Value
        };
    }

    /// <summary>
    /// Maps the kind of the secret 
    /// </summary>
    /// <param name="kind">The kind to map</param>
    /// <returns></returns>
    public static UnifiedSecretKind MapKind(string kind)
    {
        return kind switch
        {
            UnifiedSecretKinds.WORKSPACE => UnifiedSecretKind.Workspace,
            UnifiedSecretKinds.USER => UnifiedSecretKind.User,
            _ => throw ErrorDefinition.Validation(SecretErrors.INVALID_KIND, "The secret kind is not valid").AsException()
        };
    }
}