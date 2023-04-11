namespace Shoc.Identity.Model
{
    /// <summary>
    /// The create root model
    /// </summary>
    public class CreateRootModel
    {
        /// <summary>
        /// The preferred email for root
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// The preferred password for root
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// The full name for root
        /// </summary>
        public string FullName { get; set; }
    }
}