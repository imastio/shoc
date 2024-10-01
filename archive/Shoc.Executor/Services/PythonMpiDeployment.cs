using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Shoc.Executor.Model.Deployment;
using Shoc.Executor.Services.Interfaces;
using Shoc.Kube.Model;

namespace Shoc.Executor.Services
{
    /// <summary>
    /// Class for Python-pip project type deployment
    /// </summary>
    public class PythonMpiDeployment : DeploymentBase, IDeployment
    {
        /// <summary>
        /// The name of the headless service
        /// </summary>
        private readonly string headlessServiceName;

        /// <summary>
        /// The name of the stateful set
        /// </summary>
        private readonly string statefulSetName;

        /// <summary>
        /// The name/label of the mpi workers
        /// </summary>
        private readonly string mpiWorkerName;

        /// <summary>
        /// The name/label of the mpi workers
        /// </summary>
        private readonly string mpiWorkerNameFormat;

        /// <summary>
        /// Creates new instance of class
        /// </summary>
        /// <param name="input"></param>
        public PythonMpiDeployment(CreateDeploymentInput input) : base(input)
        {
            this.headlessServiceName = $"headless-service-{input.Job.Id}";
            this.statefulSetName = $"statefulset-{this.deploymentInput.Job.Id}";
            this.mpiWorkerName = $"mpi-worker-{this.deploymentInput.Job.Id}";
            this.mpiWorkerNameFormat = $"statefulset-{this.deploymentInput.Job.Id}-{{0}}.headless-service-{this.deploymentInput.Job.Id}\n";
        }

        /// <summary>
        /// Deploys the required kinds into Kubernetes
        /// </summary>
        public async Task Deploy()
        {
            // make sure namespace exists
            await this.AssureNamespace();

            // create the secret for pulling images
            await this.CreatePullSecret();

            // creates headless service in kubernetes
            await this.CreateHeadlessService();

            // creates statefulset in kubernetes
            await this.CreateStatefulSet();

            // creates config map for file including workers in kubernetes
            await this.CreateConfigMap();

            // creates batch job in kubernetes
            await this.CreateJob();
        }

        /// <summary>
        /// Creates secret in kubernetes
        /// </summary>
        /// <returns></returns>
        private async Task CreateConfigMap()
        {
            // init string builder
            var stBuilder = new StringBuilder();

            // adds names as for each mpi worker/pod into hosts file
            for (var i = 0; i < this.deploymentInput.NodeCount; i++)
            {
                stBuilder.Append(string.Format(this.mpiWorkerNameFormat, i));
            }

            // create config for hosts file, will be mapped as file
            var configs = new Dictionary<string, string>
            {
                { "hosts", stBuilder.ToString() }
            };

            // create config map in kube
            await this.kubeClient.CreateConfigMap(new CreateConfigMapInput
            {
                Name = $"config-{this.deploymentInput.Job.Id}",
                Configs = configs
            });
        }

        /// <summary>
        /// Creates statefulset in kubernetes
        /// </summary>
        /// <returns></returns>
        private async Task CreateHeadlessService()
        {
            await this.kubeClient.CreateHeadlessService(new CreateHeadlessServiceInput
            {
                Name = this.headlessServiceName,
                Selector = new Dictionary<string, string>
                {
                    { "app", this.mpiWorkerName }
                },
                PortName = "ssh",
                Port = 22,
                TargetPort = 22
            });
        }

        /// <summary>
        /// Creates statefulset in kubernetes
        /// </summary>
        /// <returns></returns>
        private async Task CreateStatefulSet()
        {
            await this.kubeClient.CreateStatefulSet(new CreateStatefulSetInput
            {
                Name = this.statefulSetName,
                ServiceName = this.headlessServiceName,
                ServiceSelector = new Dictionary<string, string>
                {
                    { "app", this.mpiWorkerName }
                },
                Replicas = this.deploymentInput.NodeCount,
                PodLabels = new Dictionary<string, string>
                {
                    { "app", this.mpiWorkerName }
                },
                ContainerName = "worker",
                ContainerImageUri = this.deploymentInput.Image,
                ContainerPort = this.deploymentInput.ServicePort
            });
        }

        /// <summary>
        /// Creates secret in kubernetes
        /// </summary>
        /// <returns></returns>
        protected override async Task CreateJob()
        {
            Thread.Sleep(20000);
            await this.kubeClient.CreateJobWithOverridenCommand(deploymentInput.Image, new List<string>{ "mpirun", "--machinefile", "hosts/hosts", "python", "index.py"});
        }
    }
}