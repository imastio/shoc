namespace Shoc.Builder.Model.Package
{
    /// <summary>
    /// The project lookup query definition
    /// </summary>
    public class PackageQuery
    {
        /// <summary>
        /// The project id
        /// </summary>
        public string ProjectId { get; set; }

        /// <summary>
        /// The listing checksum
        /// </summary>
        public string ListingChecksum { get; set; }
    }
}