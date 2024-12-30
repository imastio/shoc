using Shoc.Package.Grpc.Packages;
using Shoc.Package.Model.Package;

namespace Shoc.Package.Grpc;

/// <summary>
/// The package grpc mapper
/// </summary>
public class PackageGrpcMapper
{
    /// <summary>
    /// Maps to the Grpc representation
    /// </summary>
    /// <param name="input">The input to map</param>
    /// <returns></returns>
    public PackageGrpcModel Map(PackageExtendedModel input)
    {
        return new PackageGrpcModel
        {
            Id = input.Id,
            WorkspaceId = input.WorkspaceId,
            UserId = input.UserId,
            Scope = input.Scope,
            ListingChecksum = input.ListingChecksum,
            Manifest = input.Manifest,
            Runtime = input.Runtime,
            Containerfile = input.Containerfile,
            TemplateReference = input.TemplateReference,
            RegistryId = input.RegistryId,
            Image = input.Image
        };
    }
}