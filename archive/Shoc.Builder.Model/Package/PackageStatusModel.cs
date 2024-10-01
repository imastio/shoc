namespace Shoc.Builder.Model.Package
{
    /// <summary>
    /// The package status update structure
    /// </summary>
    public class PackageStatusModel
    {
        /// <summary>
        /// The package id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The status of package
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// The package progress
        /// </summary>
        public int Progress { get; set; }

        /// <summary>
        /// The progress message
        /// </summary>
        public string ProgressMessage { get; set; }
    }
}