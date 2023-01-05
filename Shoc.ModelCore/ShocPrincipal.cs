namespace Shoc.ModelCore
{
    /// <summary>
    /// The shoc signed-in principal
    /// </summary>
    public class ShocPrincipal
    {
        /// <summary>
        /// The id of user
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// The role of subject
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// The username of user
        /// </summary>
        public string Username { get; set; }
    }
}