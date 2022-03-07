namespace Shoc.ModelCore
{
    /// <summary>
    /// The run specification for the project
    /// </summary>
    public class RunSpec
    {
        /// <summary>
        /// The run output specification
        /// </summary>
        public RunOutputSpec Output { get; set; }

        /// <summary>
        /// The resource requests
        /// </summary>
        public RunResourcesSpec Requests { get; set; }

        /// <summary>
        /// The resource limits
        /// </summary>
        public RunResourcesSpec Limits { get; set; }
    }
}