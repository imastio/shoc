using Shoc.Builder.Services.Interfaces;

namespace Shoc.Builder.Services
{
    /// <summary>
    /// Service for providing containerizes.
    /// </summary>
    public class ContainerizeProvider : IContainerizeProvider
    {
        /// <summary>
        /// Get container by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IContainerize Create(string name)
        {
            return name switch
            {
                "hostname" => new HostnameContainerizer(),
                "list" => new ListContainerizer(),
                "python-pip" => new PythonPipContainerizer(),
                _ => null
            };
        }
    }
}
