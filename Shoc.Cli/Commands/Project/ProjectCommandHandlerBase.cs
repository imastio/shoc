using System.CommandLine.Invocation;
using System.IO;
using System.Threading.Tasks;
using Shoc.Cli.Model;
using Shoc.Cli.Utility;

namespace Shoc.Cli.Commands.Project
{
    /// <summary>
    /// The common class for project command handlers
    /// </summary>
    public abstract class ProjectCommandHandlerBase : ICommandHandler
    {
        /// <summary>
        /// The shoc manifest file name
        /// </summary>
        protected const string MANIFEST_FILE = "shoc-manifest.yml";

        /// <summary>
        /// The context directory
        /// </summary>
        public DirectoryInfo Directory { get; set; }

        /// <summary>
        /// The profile name
        /// </summary>
        public string Profile { get; set; }

        /// <summary>
        /// Gets the manifest if exists
        /// </summary>
        /// <returns></returns>
        public async Task<ShocManifest> GetManifest()
        {
            // the path to manifest file
            var path = Path.Combine(this.Directory.FullName, MANIFEST_FILE);

            // check if manifest file exists
            return File.Exists(path) ? Yml.Deserialize<ShocManifest>(await File.ReadAllTextAsync(path)) : null;
        }

        /// <summary>
        /// Saves the given manifest to the project file
        /// </summary>
        /// <param name="manifest">The manifest to save</param>
        /// <returns></returns>
        public async Task<ShocManifest> SaveManifest(ShocManifest manifest)
        {
            // the path to manifest file
            var path = Path.Combine(this.Directory.FullName, MANIFEST_FILE);

            // write the manifest to the directory
            await File.WriteAllTextAsync(path, Yml.Serialize(manifest));

            // get saved object
            return await this.GetManifest();
        }

        /// <summary>
        /// Implementation of project command invocation
        /// </summary>
        /// <param name="context">The invocation context</param>
        /// <returns></returns>
        public abstract Task<int> InvokeAsync(InvocationContext context);
    }
}