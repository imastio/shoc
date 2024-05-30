namespace Shoc.Identity.Model.User
{
    /// <summary>
    /// Updates the profile picture model
    /// </summary>
    public class UserPictureUpdateModel
    {
        /// <summary>
        /// The user id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The picture URI
        /// </summary>
        public string PictureUri { get; set; }
    }
}