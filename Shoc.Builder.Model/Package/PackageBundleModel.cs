namespace Shoc.Builder.Model.Package
{
    /// <summary>
    /// The package bundle model
    /// </summary>
    public class PackageBundleModel
    {
        /// <summary>
        /// The id of the bundle
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The package reference
        /// </summary>
        public string PackageId { get; set; }

        /// <summary>
        /// The root of the bundle
        /// </summary>
        public string BundleRoot { get; set; }
    }
}