namespace Shoc.Identity.Model.User
{
    /// <summary>
    /// The user filter model
    /// </summary>
    public class UserFilterModel
    {
        /// <summary>
        /// The search term to apply
        /// </summary>
        public string Search { get; set; }

        /// <summary>
        /// The user type
        /// </summary>
        public string Type { get; set; }
    }
}