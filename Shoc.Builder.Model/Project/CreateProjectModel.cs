namespace Shoc.Builder.Model.Project
{
    /// <summary>
    /// Create or update project model operation model
    /// </summary>
    public class CreateProjectModel
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
        /// The owner user of the project
        /// </summary>
        public string OwnerId { get; set; }
    }
}