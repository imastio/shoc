namespace Shoc.Builder.Model.Package
{
    /// <summary>
    /// The package image model
    /// </summary>
    public class PackageImageModel
    {
        /// <summary>
        /// The package id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The image uri
        /// </summary>
        public string ImageUri { get; set; }

        /// <summary>
        /// The image building file content (Dockerfile, etc)
        /// </summary>
        public string ImageRecipe { get; set; }

        /// <summary>
        /// The package checksum
        /// </summary>
        public string ImageChecksum { get; set; }
    }
}