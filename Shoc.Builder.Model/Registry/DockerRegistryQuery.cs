namespace Shoc.Builder.Model.Registry
{
    /// <summary>
    /// The docker registry query
    /// </summary>
    public class DockerRegistryQuery
    {
        /// <summary>
        /// The registry name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The owner id
        /// </summary>
        public string OwnerId { get; set; }

        /// <summary>
        /// The looking up shared or private
        /// </summary>
        public bool? Shared { get; set; }
    }
}