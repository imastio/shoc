namespace Shoc.ModelCore
{
    /// <summary>
    /// The run resources specification
    /// </summary>
    public class RunResourcesSpec
    {
        /// <summary>
        /// The CPU resource units
        /// </summary>
        public string Cpu { get; set; }

        /// <summary>
        /// The memory resource units
        /// </summary>
        public string Memory { get; set; }

        /// <summary>
        /// The Nvidia GPU resource unites
        /// </summary>
        public string NvidiaGpu { get; set; }
    }
}