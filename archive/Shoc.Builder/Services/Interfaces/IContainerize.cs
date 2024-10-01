using Shoc.ModelCore;

namespace Shoc.Builder.Services.Interfaces
{
    /// <summary>
    /// Interfaces for Containerize
    /// </summary>
    public interface IContainerize
    {
        /// <summary>
        /// Returns content for Dockerfile
        /// </summary>
        /// <param name="spec">The build specification</param>
        /// <returns>The content of Dockerfile</returns>
        string GetDockerFile(BuildSpec spec);
    }
}
