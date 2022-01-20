namespace Shoc.Engine.Model
{
    /// <summary>
    /// The engine instance info
    /// </summary>
    public class EngineInstanceInfo
    {
        /// <summary>
        /// The identifier of engine instance
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The name of engine instance
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The driver of engine instance
        /// </summary>
        public string Driver { get; set; }

        /// <summary>
        /// The flag to indicate if engine is in running state
        /// </summary>
        public bool Running { get; set; }

        /// <summary>
        /// The number of images in engine instance
        /// </summary>
        public long? Images { get; set; }

        /// <summary>
        /// The number of containers running in engine instance
        /// </summary>
        public long? Containers { get; set; }
    }
}