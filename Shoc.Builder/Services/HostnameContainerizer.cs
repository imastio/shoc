using System.Text;
using Shoc.Builder.Services.Interfaces;
using Shoc.ModelCore;

namespace Shoc.Builder.Services
{
    /// <summary>
    /// Class for Hostname project type containerizer
    /// </summary>
    public class HostnameContainerizer : ContainerizeBase, IContainerize
    {
        /// <summary>
        /// Returns content for Dockerfile
        /// </summary>
        /// <param name="spec">The build specification</param>
        /// <returns>The content of Dockerfile</returns>
        public string GetDockerFile(BuildSpec spec)
        {
            // storage for all content
            var output = new StringBuilder();

            // add lines to 
            output.AppendLine("FROM alpine");
            output.AppendLine("WORKDIR /");
            output.AppendLine("ENTRYPOINT [\"hostname\"]");

            //output.AppendLine(this.CopyStatement(spec.Input.Files));
            //output.AppendLine($"ENTRYPOINT [{string.Join(", ", spec.EntryPoint.Select())}]");

            return output.ToString();
        }
    }
}
