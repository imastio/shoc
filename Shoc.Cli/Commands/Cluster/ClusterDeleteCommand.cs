using System.CommandLine;

namespace Shoc.Cli.Commands.Cluster
{
    /// <summary>
    /// The command to delete the cluster
    /// </summary>
    public class ClusterDeleteCommand : Command
    {
        /// <summary>
        /// Creates new instance of the command
        /// </summary>
        public ClusterDeleteCommand() : base("delete", "Delete existing kubernetes cluster")
        {
            this.AddOption(new Option<string>(new[] { "-n", "--name" }, "The name of cluster to delete"));
        }
    }
}