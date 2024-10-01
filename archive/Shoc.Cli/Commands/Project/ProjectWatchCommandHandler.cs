using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Threading;
using System.Threading.Tasks;
using Shoc.Cli.Services;
using Shoc.Core;

namespace Shoc.Cli.Commands.Project
{
    /// <summary>
    /// The watch project command handler
    /// </summary>
    public class ProjectWatchCommandHandler : ProjectCommandHandlerBase
    {
        /// <summary>
        /// The given job identifier
        /// </summary>
        public string JobId { get; set; }

        /// <summary>
        /// Creates new instance of command handler
        /// </summary>
        /// <param name="clientService">The client service</param>
        /// <param name="authService">The auth service</param>
        public ProjectWatchCommandHandler(ClientService clientService, AuthService authService) : base(clientService, authService)
        {
        }

        /// <summary>
        /// Implementation of command invocation
        /// </summary>
        /// <param name="context">The invocation context</param>
        /// <returns></returns>
        public override async Task<int> InvokeAsync(InvocationContext context)
        {
            // do the operation authorized
            var job = await this.authService.DoAuthorized(this.Profile, async (profile, me)
                => await this.clientService.Executor(profile).GetJobById(me.AccessToken, this.JobId)
            );

            // make sure is job has been found
            if (job == null)
            {
                throw ErrorDefinition.Validation(CliErrors.JOB_INVALID).AsException();
            }

            context.Console.WriteLine($"Getting job logs: {this.JobId}");

            // create token for cancelling operation
            var cancellationToken = new CancellationTokenSource();

            // handle event for CTRL-C and cancel printing logs
            Console.CancelKeyPress += (_, _) => { cancellationToken.Cancel(); };

            // do the operation authorized
            await this.authService.DoAuthorized(this.Profile, async (profile, me) => 
                await this.clientService.Executor(profile).WatchJob(me.AccessToken, job.Id, context.Console.WriteLine, cancellationToken.Token));
            
            return 0;
        }
    }
}