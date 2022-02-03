using Shoc.ModelCore;

namespace Shoc.Cli.Model
{
    /// <summary>
    /// The project manifest 
    /// </summary>
    public class ShocManifest
    {
        /// <summary>
        /// The project identity
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The build specification for the project
        /// </summary>
        public BuildSpec Build { get; set; }

        /// <summary>
        /// The run specification for the project
        /// </summary>
        public RunSpec Run { get; set; }
    }
}