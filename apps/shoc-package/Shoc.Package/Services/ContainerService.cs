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
    
    /// <summary>
    /// Pushes an image from to the registry
    /// </summary>
    /// <param name="context">The context to push</param>
    /// <returns></returns>
    public async Task<CommandRunResult> Push(ContainerPushContext context)
    {
        // the command to run with runner
        var command = $"./Scripts/container-build.sh push --tls-verify=false --creds {context.Username}:{context.Password} {context.Image}";
        
        // run command to build
        return await this.commandRunner.RunBash(command);
    }
    
    /// <summary>
    /// Removes a local image with the name
    /// </summary>
    /// <param name="context">The context to remove</param>
    /// <returns></returns>
    public async Task<CommandRunResult> RemoveImage(ContainerRmiContext context)
    {
        // the command to run with runner
        var command = $"./Scripts/container-build.sh rmi {context.Image}";
        
        // run command to build
        return await this.commandRunner.RunBash(command);
    }
    
    /// <summary>
    /// Copies image from source to target
    /// </summary>
    /// <param name="source">The source context to pull</param>
    /// <param name="target">The target context to push</param>
    /// <returns></returns>
    public async Task<CommandRunResult> Copy(ContainerCopyContext source, ContainerCopyContext target)
    {
        // the command to run with runner
        var command = $"./Scripts/container-imagectl.sh copy --src-tls-verify=false --src-creds {source.Username}:{source.Password} --dest-tls-verify=false --dest-creds {target.Username}:{target.Password} docker://{source.Image} docker://{target.Image}";
        
        // run command to build
        return await this.commandRunner.RunBash(command);
    }
}