namespace Shoc.Engine.Model
{
    /// <summary>
    /// The image push info
    /// </summary>
    public class ImagePushInput
    {
        /// <summary>
        /// The username
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// The password in plaintext
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// The versions of the image
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// The image uri
        /// </summary>
        public string ImageUri { get; set; }
    }
}