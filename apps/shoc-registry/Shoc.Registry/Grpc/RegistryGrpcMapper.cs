using Shoc.Registry.Grpc.Registries;
using Shoc.Registry.Model.Registry;

namespace Shoc.Registry.Grpc;

/// <summary>
/// The registry grpc mapper
/// </summary>
public class RegistryGrpcMapper
{
    /// <summary>
    /// Maps to the Grpc representation
    /// </summary>
    /// <param name="input">The input to map</param>
    /// <returns></returns>
    public RegistryGrpcModel Map(RegistryModel input)
    {
        return new RegistryGrpcModel
        {
            Id = input.Id,
            WorkspaceId = input.WorkspaceId ?? string.Empty,
            Name = input.Name,
            DisplayName = input.DisplayName,
            Status = input.Status,
            Provider = input.Provider,
            UsageScope = input.UsageScope,
            Registry = input.Registry,
            Namespace = input.Name
        };
    }
}