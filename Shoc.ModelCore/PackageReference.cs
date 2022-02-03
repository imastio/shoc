namespace Shoc.ModelCore
{
    /// <summary>
    /// The package reference definition
    /// </summary>
    public class PackageReference
    {
        /// <summary>
        /// The package name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The package group (optional)
        /// </summary>
        public string Group { get; set; }

        /// <summary>
        /// The package version (optional)
        /// </summary>
        public string Version { get; set; }
    }
}