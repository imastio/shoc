using System.CommandLine;

namespace Shoc.Cli.Commands.Cluster
{
    /// <summary>
    /// The command to list the clusters
    /// </summary>
    public class ClusterListCommand : Command
    {
        /// <summary>
        /// Creates new instance of the command
        /// </summary>
        public ClusterListCommand() : base("list", "List all the available clusters")
        {
        }
    }
}