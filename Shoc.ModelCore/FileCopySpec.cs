namespace Shoc.ModelCore
{
    /// <summary>
    /// The file copy specification
    /// </summary>
    public class FileCopySpec
    {
        /// <summary>
        /// The local file to copy from
        /// </summary>
        public string From { get; set; }

        /// <summary>
        /// The destination to copy to
        /// </summary>
        public string To { get; set; }
    }
}