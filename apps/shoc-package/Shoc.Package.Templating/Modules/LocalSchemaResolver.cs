using System.IO;
using System.Text;
using Newtonsoft.Json.Schema;

namespace Shoc.Package.Templating.Modules;

/// <summary>
/// The schema resolver based on local files
/// </summary>
public class LocalSchemaResolver : JSchemaUrlResolver
{
    /// <summary>
    /// Gets the schema resource
    /// </summary>
    /// <param name="context">The context</param>
    /// <param name="reference">The schema reference</param>
    /// <returns></returns>
    public override Stream GetSchemaResource(ResolveSchemaContext context, SchemaReference reference)
    {
        // schema is not referenced with base URI
        if (reference.BaseUri == null)
        {
            return base.GetSchemaResource(context, reference);
        }

        // handle if the referenced schema is found locally
        if (KnownSchemas.ALL.TryGetValue(reference.BaseUri, out var localSchema))
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(localSchema.ToString()));
        }
        
        // keep default logic otherwise
        return base.GetSchemaResource(context, reference);
    }
}