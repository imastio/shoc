using System.Linq;
using System.Text;
using Shoc.Builder.Services.Interfaces;
using Shoc.ModelCore;

namespace Shoc.Builder.Services
{
    /// <summary>
    /// Class for Python-pip project type containerizer
    /// </summary>
    public class PythonPipContainerizer : ContainerizeBase, IContainerize
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
            output.AppendLine("FROM python:alpine");
            output.AppendLine("WORKDIR /app/shoc");
            output.AppendLine("COPY . .");
            output.AppendLine("RUN pip install -r requirements.txt");
            output.AppendLine("ENTRYPOINT [\"python\", \"index.py\"]");

            return output.ToString();
        }
    }
}
