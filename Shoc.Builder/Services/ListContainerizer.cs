using System.Text;
using Shoc.Builder.Services.Interfaces;
using Shoc.ModelCore;

namespace Shoc.Builder.Services
{
    /// <summary>
    /// Class for List project type containerizer
    /// </summary>
    public class ListContainerizer : ContainerizeBase, IContainerize
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
            output.AppendLine("WORKDIR /app/shoc");
            output.AppendLine("COPY . .");
            output.AppendLine("ENTRYPOINT [\"ls\"]");

            return output.ToString();
        }
    }
}
