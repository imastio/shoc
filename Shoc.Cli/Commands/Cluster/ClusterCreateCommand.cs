using System.CommandLine;

namespace Shoc.Cli.Commands.Cluster
{
    /// <summary>
    /// The command to create the cluster
    /// </summary>
    public class ClusterCreateCommand : Command
    {
        /// <summary>
        /// Creates new instance command
        /// </summary>
        public ClusterCreateCommand() : base("create", "Create new kubernetes cluster")
        {
            this.AddOption(new Option<string>(new[] { "-p", "--path" }, "The path of the KUBECONFIG"));
        }
    }
}