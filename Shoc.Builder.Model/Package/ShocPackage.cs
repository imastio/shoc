using System;

namespace Shoc.Builder.Model.Package
{
    /// <summary>
    /// The shoc package definition
    /// </summary>
    public class ShocPackage
    {
        /// <summary>
        /// The package id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The project id
        /// </summary>
        public string ProjectId { get; set; }

        /// <summary>
        /// The state of package
        /// </summary>
        public string Status { get; set; }
        
        /// <summary>
        /// The registry id
        /// </summary>
        public string RegistryId { get; set; }
        
        /// <summary>
        /// The image uri
        /// </summary>
        public string ImageUri { get; set; }
        
        /// <summary>
        /// The build specification used for the image
        /// </summary>
        public string BuildSpec { get; set; }

        /// <summary>
        /// The image building file content (Dockerfile, etc)
        /// </summary>
        public string ImageRecipe { get; set; }

        /// <summary>
        /// The checksum of overall files listing
        /// </summary>
        public string ListingChecksum { get; set; }

        /// <summary>
        /// The package checksum
        /// </summary>
        public string ImageChecksum { get; set; }

        /// <summary>
        /// The progress indicator
        /// </summary>
        public int Progress { get; set; }

        /// <summary>
        /// The progress message
        /// </summary>
        public string ProgressMessage { get; set; }
        
        /// <summary>
        /// The package creation time
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// The package update time
        /// </summary>
        public DateTime Updated { get; set; }
    }
}