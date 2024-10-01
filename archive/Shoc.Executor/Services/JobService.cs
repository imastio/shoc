using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;
using Shoc.ApiCore.AuthClient;
using Shoc.Builder.Client;
using Shoc.Core;
using Shoc.Executor.Data;
using Shoc.Executor.Model;
using Shoc.Executor.Model.Deployment;
using Shoc.Executor.Model.Job;
using Shoc.Executor.Services.Interfaces;
using Shoc.Kube.Client;
using Shoc.ModelCore;

namespace Shoc.Executor.Services
{
    /// <summary>
    /// The job service
    /// </summary>
    public class JobService
    {
        /// <summary>
        /// The job deploying progress
        /// </summary>
        private const int JOB_DEPLOYING_PROGRESS = 25;

        /// <summary>
        /// The job repository
        /// </summary>
        private readonly IJobRepository jobRepository;

        /// <summary>
        /// The client for builder
        /// </summary>
        private readonly BuilderClient builderClient;

        /// <summary>
        /// The auth provider
        /// </summary>
        private readonly AuthProvider authProvider;

        /// <summary>
        /// The kubernetes cluster repository
        /// </summary>
        private readonly IKubernetesClusterRepository clusterRepository;

        /// <summary>
        /// The data protection provider
        /// </summary>
        private readonly IDataProtectionProvider protectionProvider;

        /// <summary>
        /// The deployments provider
        /// </summary>
        private readonly IDeploymentProvider deploymentProvider;

        /// <summary>
        /// Creates new instance of job service
        /// </summary>
        /// <param name="jobRepository">The job repository</param>
        /// <param name="builderClient">The builder client</param>
        /// <param name="authProvider">The authentication provider</param>
        /// <param name="clusterRepository">The cluster repository</param>
        /// <param name="protectionProvider">The data protection provider</param>
        /// <param name="deploymentProvider">The deployments provider</param>
        public JobService(
            IJobRepository jobRepository,
            BuilderClient builderClient,
            AuthProvider authProvider,
            IKubernetesClusterRepository clusterRepository,
            IDataProtectionProvider protectionProvider,
            IDeploymentProvider deploymentProvider
            )
        {
            this.jobRepository = jobRepository;
            this.builderClient = builderClient;
            this.authProvider = authProvider;
            this.clusterRepository = clusterRepository;
            this.protectionProvider = protectionProvider;
            this.deploymentProvider = deploymentProvider;
        }

        /// <summary>
        /// Gets the job by identifier
        /// </summary>
        /// <param name="principal">The current principal</param>
        /// <param name="id">The job identifier</param>
        /// <returns></returns>
        public async Task<JobModel> GetById(ShocPrincipal principal, string id)
        {
            // get job
            var job = await this.jobRepository.GetById(principal.Subject, id);

            // make sure job exists
            if (job == null)
            {
                throw ErrorDefinition.NotFound(ExecutorErrors.JOB_NOT_FOUND).AsException();
            }

            // return job
            return job;
        }

        /// <summary>
        /// Creates the job by given input
        /// </summary>
        /// <param name="principal">The current principal</param>
        /// <param name="input">The create job input</param>
        /// <returns></returns>
        public async Task<JobModel> Create(ShocPrincipal principal, CreateJobInput input)
        {
            // do the operation authorized
            var project = await this.authProvider.DoAuthorized(async (token) => await this.builderClient.GetProjectById(token, input.ProjectId, principal.Subject));

            // make sure projects exists
            if (project == null)
            {
                throw ErrorDefinition.NotFound().AsException();
            }

            // do the operation authorized
            var package = await this.authProvider.DoAuthorized(async (token) => await this.builderClient.GetPackageById(token, input.ProjectId, input.PackageId, principal.Subject));

            // make sure package exists
            if (package == null)
            {
                throw ErrorDefinition.NotFound().AsException();
            }

            // do the operation authorized
            var registry = await this.authProvider.DoAuthorized(async (token) => await this.builderClient.GetRegistryById(token, package.RegistryId));

            // make sure registry exists
            if (registry == null)
            {
                throw ErrorDefinition.NotFound().AsException();
            }

            // assign default owner if not assigned yet
            input.OwnerId = principal.Subject;
            input.Status = JobStatuses.INIT;
            input.Progress = 0;
            input.ProgressMessage = string.Empty;

            // initiate the creation
            return await this.jobRepository.Create(input);
        }

        /// <summary>
        /// Deploys the job with id
        /// </summary>
        /// <param name="principal">The current principal</param>
        /// <param name="input">The job deploy input</param>
        /// <returns></returns>
        public async Task<JobModel> Deploy(ShocPrincipal principal, DeployJobInput input)
        {
            // get the job
            var job = await this.jobRepository.GetById(principal.Subject, input.Id);

            // make sure job exists
            if (job == null)
            {
                throw ErrorDefinition.NotFound().AsException();
            }

            // update package status
            await this.jobRepository.UpdateStatus(new JobStatusModel()
            {
                Id = job.Id,
                Status = JobStatuses.DEPLOYING,
                Progress = JOB_DEPLOYING_PROGRESS,
                ProgressMessage = "Deploying the objects for the job"
            });

            // do the operation authorized
            var project = await this.authProvider.DoAuthorized(async (token) => await this.builderClient.GetProjectById(token, job.ProjectId, principal.Subject));

            // make sure projects exists
            if (project == null)
            {
                throw ErrorDefinition.NotFound().AsException();
            }

            // do the operation authorized
            var package = await this.authProvider.DoAuthorized(async (token) => await this.builderClient.GetPackageById(token, job.ProjectId, job.PackageId, principal.Subject));

            // make sure package exists
            if (package == null)
            {
                throw ErrorDefinition.NotFound().AsException();
            }

            // do the operation authorized
            var registry = await this.authProvider.DoAuthorized(async (token) => await this.builderClient.GetRegistryById(token, package.RegistryId));

            // make sure registry exists
            if (registry == null)
            {
                throw ErrorDefinition.NotFound().AsException();
            }

            // get any cluster for now
            var cluster = (await this.clusterRepository.GetAll()).FirstOrDefault();

            // make sure cluster exists
            if (cluster == null)
            {
                throw ErrorDefinition.NotFound().AsException();
            }

            // get protector
            var protector = this.protectionProvider.CreateProtector(ExecutorProtection.CLUSTER_CONFIG);
            var protectorRegistry = this.protectionProvider.CreateProtector(ExecutorProtection.REGISTRY_CREDENTIALS);

            // get the deployment base on project type
            var deployment = this.deploymentProvider.Create(new CreateDeploymentInput
            {
                Job = job,
                Type = project.Type,
                Kubeconfig = protector.Unprotect(cluster.EncryptedKubeConfig),
                RegistryUsername = registry.Username,
                RegistryPassword = protectorRegistry.Unprotect(registry.EncryptedPassword),
                RegistryEmail = registry.Email,
                RegistryUrl = $"{registry.RegistryUri.TrimEnd('/')}/{registry.Repository}",
                Image = $"{package.ImageUri}:{package.Id}",
                NodeCount = input.Workers
            });

            // make sure deployment exists
            if (deployment == null)
            {
                throw ErrorDefinition.NotFound().AsException();
            }

            await deployment.Deploy();

            // initiate the creation
            return job;
        }

        /// <summary>
        /// Watch job logs
        /// </summary>
        /// <param name="principal">The current principal</param>
        /// <param name="id">The job identifier</param>
        /// <returns></returns>
        public async Task<Stream> WatchJob(ShocPrincipal principal, string id)
        {
            // get the job
            var job = await this.jobRepository.GetById(principal.Subject, id);

            // make sure job exists
            if (job == null)
            {
                throw ErrorDefinition.NotFound().AsException();
            }

            // get any cluster for now
            var cluster = (await this.clusterRepository.GetAll()).FirstOrDefault();

            // make sure cluster exists
            if (cluster == null)
            {
                throw ErrorDefinition.NotFound().AsException();
            }

            // get the protector
            var protector = this.protectionProvider.CreateProtector(ExecutorProtection.CLUSTER_CONFIG);

            // get kube configuration
            var kubeConfig = protector.Unprotect(cluster.EncryptedKubeConfig);

            // get kubernetes client
            var kubeClient = new KubernetesClient(kubeConfig, job.Id);

            // gets pods of job
            var pods = (await kubeClient.GetPodsByLabel("app", job.Id)).ToList();

            // check if any pod exists
            if (!pods.Any())
            {
                throw ErrorDefinition.NotFound(ExecutorErrors.PODS_MISSING).AsException();
            }

            // get log stream of the first pod
            return await kubeClient.GetPodLog(pods.First());
        }
    }
}