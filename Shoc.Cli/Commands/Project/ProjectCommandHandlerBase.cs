using System;
using System.CommandLine.Invocation;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Shoc.Builder.Model.Project;
using Shoc.Cli.Model;
using Shoc.Cli.Services;
using Shoc.Cli.Utility;
using Shoc.Core;

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
        private const string MANIFEST_FILE = "shoc-manifest.yml";

        /// <summary>
        /// The client service
        /// </summary>
        protected readonly ClientService clientService;

        /// <summary>
        /// The authentication service
        /// </summary>
        protected readonly AuthService authService;
        
        /// <summary>
        /// The context directory
        /// </summary>
        public DirectoryInfo Directory { get; set; }

        /// <summary>
        /// The profile name
        /// </summary>
        public string Profile { get; set; }

        /// <summary>
        /// Creates new instance of command handler
        /// </summary>
        /// <param name="clientService">The client service</param>
        /// <param name="authService">The auth service</param>
        protected ProjectCommandHandlerBase(ClientService clientService, AuthService authService)
        {
            this.clientService = clientService;
            this.authService = authService;
        }

        /// <summary>
        /// Gets the manifest if exists
        /// </summary>
        /// <returns></returns>
        protected async Task<ShocManifest> GetManifest()
        {
            // the path to manifest file
            var path = Path.Combine(this.Directory.FullName, MANIFEST_FILE);

            // check if manifest file exists
            return File.Exists(path) ? Yml.Deserialize<ShocManifest>(await File.ReadAllTextAsync(path)) : null;
        }

        /// <summary>
        /// Gets the manifest if exists
        /// </summary>
        /// <returns></returns>
        protected async Task<ShocManifest> RequireManifest()
        {
            // try get manifest
            var manifest = await this.GetManifest();

            // make sure manifest exists
            if (manifest == null)
            {
                throw ErrorDefinition.Validation(CliErrors.MISSING_MANIFEST, "The manifest is missing.").AsException();
            }

            return manifest;
        }

        /// <summary>
        /// Saves the given manifest to the project file
        /// </summary>
        /// <param name="manifest">The manifest to save</param>
        /// <returns></returns>
        protected async Task<ShocManifest> SaveManifest(ShocManifest manifest)
        {
            // the path to manifest file
            var path = Path.Combine(this.Directory.FullName, MANIFEST_FILE);

            // write the manifest to the directory
            await File.WriteAllTextAsync(path, Yml.Serialize(manifest));

            // get saved object
            return await this.GetManifest();
        }

        /// <summary>
        /// Require the existing project in the context
        /// </summary>
        /// <returns></returns>
        protected Task<ProjectModel> RequireProject()
        {
            return this.WithProject((_, proj) => Task.FromResult(proj));
        }

        /// <summary>
        /// Execute the action for an existing project and the manifest
        /// </summary>
        /// <param name="action">The action to execute</param>
        /// <returns></returns>
        protected async Task<T> WithProject<T>(Func<ShocManifest, ProjectModel, Task<T>> action)
        {
            // get the manifest
            var manifest = await this.RequireManifest();

            // do an authorized action
            var project =  await this.authService.DoAuthorized(this.Profile, async (profile, auth) =>
            {
                // the builder client
                var builder = this.clientService.Builder(profile);
                
                // get the project by path
                var query = await builder.GetProjectsByPath(auth.AccessToken, manifest.Directory, manifest.Name);

                // get the result
                var result = query.FirstOrDefault();

                // make sure result is there
                if (result == null)
                {
                    throw ErrorDefinition.Validation(CliErrors.MISSING_PROJECT).AsException();
                }
                
                return result;
            });

            // execute action with manifest and project
            return await action(manifest, project);
        }
        
        /// <summary>
        /// Implementation of project command invocation
        /// </summary>
        /// <param name="context">The invocation context</param>
        /// <returns></returns>
        public abstract Task<int> InvokeAsync(InvocationContext context);
    }
}