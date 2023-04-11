using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Linq;
using System.Threading.Tasks;
using Shoc.Cli.Services;
using Shoc.Core;
using Shoc.Executor.Model.Job;
using Shoc.ModelCore;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Shoc.Cli.Commands.Project
{
    /// <summary>
    /// The run project command handler
    /// </summary>
    public class ProjectRunCommandHandler : ProjectCommandHandlerBase
    {
        /// <summary>
        /// The given project name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The given project version/package name
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// The arguments list
        /// </summary>
        public IEnumerable<string> Args { get; set; }

        /// <summary>
        /// Creates new instance of command handler
        /// </summary>
        /// <param name="clientService">The client service</param>
        /// <param name="authService">The auth service</param>
        public ProjectRunCommandHandler(ClientService clientService, AuthService authService) : base(clientService, authService)
        {
        }

        /// <summary>
        /// Implementation of command invocation
        /// </summary>
        /// <param name="context">The invocation context</param>
        /// <returns></returns>
        public override async Task<int> InvokeAsync(InvocationContext context)
        {
            // get the project
            var project = await this.RequireProject(this.Name);

            // get the manifest
            // TODO get run spec instead of manifest
            //var manifest = await this.RequireManifest();
            var manifest = new ShocManifest();

            // set project version name
            var versionName = string.IsNullOrWhiteSpace(this.Version) ? "latest" : this.Version;

            // do the operation authorized
            var result = await this.authService.DoAuthorized(this.Profile, async (profile, me) =>
                await this.clientService.Builder(profile).GetProjectVersionByName(me.AccessToken, project.Id, versionName)
            );

            // get version
            var projectVersion = result.FirstOrDefault();

            // make sure it exists
            if (projectVersion == null)
            {
                throw ErrorDefinition.Validation(CliErrors.PROJECT_VERSION_ERROR).AsException();
            }

            // create serializer
            var serializer = new SerializerBuilder()
                .WithNamingConvention(HyphenatedNamingConvention.Instance)
                .Build();

            // create run info
            var jobRunInfo = new JobRunInfo
            {
                Args = this.Args
            };

            // do the operation authorized
            var job = await this.authService.DoAuthorized(this.Profile, async (profile, me)
                => await this.clientService.Executor(profile).CreateProjectJob(me.AccessToken, new CreateJobInput
                {
                    ProjectId = project.Id,
                    PackageId = projectVersion.PackageId,
                    RunSpec = serializer.Serialize(manifest),
                    RunInfo = serializer.Serialize(jobRunInfo)
                })
            );

            // make sure is job has been created
            if (string.IsNullOrEmpty(job.Id))
            {
                throw ErrorDefinition.Validation(CliErrors.JOB_CREATE_FAILED).AsException();
            }

            context.Console.WriteLine($"Created job ({job.Id}) for the running");

            // do the operation authorized
            await this.authService.DoAuthorized(this.Profile, async (profile, me)
                => await this.clientService.Executor(profile).DeployProject(me.AccessToken, job.Id)
            );

            context.Console.WriteLine($"Job {job.Id} is being deployed.");
            context.Console.WriteLine(""); 
            context.Console.WriteLine("Once Job is deployed you can watch the output as follows:");
            context.Console.WriteLine($"\tshocctl project watch -j {job.Id}");
            context.Console.WriteLine("");

            return 0;
        }
    }
}