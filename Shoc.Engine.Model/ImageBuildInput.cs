using System.IO;

namespace Shoc.Engine.Model
{
    /// <summary>
    /// The image build info
    /// </summary>
    public class ImageBuildInput
    {
        /// <summary>
        /// The name of the image
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// The version of the image
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// The dockerfile
        /// </summary>
        public string Dockerfile { get; set; }

        /// <summary>
        /// The payload to create image from
        /// </summary>
        public Stream Payload { get; set; }

        /// <summary>
        /// The image uri
        /// </summary>
        public string ImageUri { get; set; }
    }
}