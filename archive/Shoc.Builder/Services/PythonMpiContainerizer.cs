using System.Text;
using Shoc.Builder.Services.Interfaces;
using Shoc.ModelCore;

namespace Shoc.Builder.Services
{
    /// <summary>
    /// Class for Python-mpi project type containerizer
    /// </summary>
    public class PythonMpiContainerizer : ContainerizeBase, IContainerize
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
            output.AppendLine("FROM ghcr.io/imastio/shoc/public/images/openmpi-python:latest");
            output.AppendLine("WORKDIR /home/shoc");
            output.AppendLine("COPY . .");
            output.AppendLine("RUN chown shoc:shoc -R .");

            return output.ToString();
        }
    }
}
