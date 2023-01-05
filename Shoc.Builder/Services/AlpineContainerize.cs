using System.Linq;
using System.Text;
using Shoc.Builder.Services.Interfaces;
using Shoc.ModelCore;

namespace Shoc.Builder.Services
{
    /// <summary>
    /// Class for Alpine
    /// </summary>
    public class AlpineContainerize : ContainerizeBase, IContainerize
    {
        /// <summary>
        /// Returns content for Dockerfile
        /// </summary>
        /// <param name="spec">The build specification</param>
        /// <returns>The content of Dockerfile</returns>
        public string GetFileContents(BuildSpec spec)
        {
            // storage for all content
            var output = new StringBuilder();

            // add lines to 
            output.AppendLine("FROM alpine");
            output.AppendLine("WORKDIR /");
            output.AppendLine(this.CopyStatement(spec.Input.Copy));

            output.AppendLine($"ENTRYPOINT [{string.Join(", ", spec.EntryPoint.Select(this.MakeParameter))}]");

            return output.ToString();
        }
    }
}
