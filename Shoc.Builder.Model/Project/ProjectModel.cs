using System;

namespace Shoc.Builder.Model.Project
{
    /// <summary>
    /// The project model
    /// </summary>
    public class ProjectModel
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

        /// <summary>
        /// The creation time
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// The update time
        /// </summary>
        public DateTime Updated { get; set; }
    }
}