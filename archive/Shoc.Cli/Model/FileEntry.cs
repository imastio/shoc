using System;

namespace Shoc.Cli.Model
{
    /// <summary>
    /// The resolved file entry
    /// </summary>
    public class FileEntry
    {
        /// <summary>
        /// The path of file
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// The file size
        /// </summary>
        public long Size { get; set; }

        /// <summary>
        /// The last modification time
        /// </summary>
        public long LastModified { get; set; }
    }
}