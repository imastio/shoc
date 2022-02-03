namespace Shoc.Builder.Model
{
    /// <summary>
    /// Create or update project model operation model
    /// </summary>
    public class CreateUpdateProjectModel
    {
        /// <summary>
        /// The project id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The project name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The project directory
        /// </summary>
        public string Directory { get; set; }

        /// <summary>
        /// The build specification
        /// </summary>
        public string BuildSpec { get; set; }

        /// <summary>
        /// The run specification
        /// </summary>
        public string RunSpec { get; set; }

        /// <summary>
        /// The owner user of the project
        /// </summary>
        public string OwnerId { get; set; }
    }
}