using System.Threading.Tasks;
using Shoc.Package.Model.Command;

namespace Shoc.Package.Services;

/// <summary>
/// The service for container operations
/// </summary>
public class ContainerService
{
    /// <summary>
    /// Additional arguments for buildah
    /// </summary>
    private const string EXTRA_BUILDAH_ARGS = "--cap-drop ALL --cap-add CAP_CHOWN --cap-add CAP_FOWNER";
    
    /// <summary>
    /// The command runner
    /// </summary>
    private readonly CommandRunner commandRunner;

    /// <summary>
    /// Creates a new instance of the service
    /// </summary>
    /// <param name="commandRunner">The command runner</param>
    public ContainerService(CommandRunner commandRunner)
    {
        this.commandRunner = commandRunner;
    }

    /// <summary>
    /// Builds an image from the context
    /// </summary>
    /// <param name="context">The context to build</param>
    /// <returns></returns>
    public async Task<CommandRunResult> Build(ContainerBuildContext context)
    {
        // the command to run with runner
        var command = $"./Scripts/container-build.sh bud -f {context.Containerfile} -t {context.Image} {EXTRA_BUILDAH_ARGS} {context.WorkingDirectory}";
        
        // run command to build
        return await this.commandRunner.RunBash(command);
    }
}