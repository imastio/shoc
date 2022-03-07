namespace Shoc.Builder.Model.Package
{
    /// <summary>
    /// The definitions of package statuses
    /// </summary>
    public static class PackageStatuses
    {
        /// <summary>
        /// The package was initialized
        /// </summary>
        public const string INIT = "init";

        /// <summary>
        /// The package is uploading from the remote client
        /// </summary>
        public const string UPLOADING = "uploading";

        /// <summary>
        /// The package is building
        /// </summary>
        public const string BUILDING = "building";

        /// <summary>
        /// The package is successfully built
        /// </summary>
        public const string BUILT = "built";

        /// <summary>
        /// The package was not built successfully
        /// </summary>
        public const string FAILED = "failed";
    }
}