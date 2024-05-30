namespace Shoc.Identity.Model.OpenId
{
    /// <summary>
    /// The access grant filter
    /// </summary>
    public class AccessGrantFilter
    {
        /// <summary>
        /// Subject id of the user.
        /// </summary>
        public string SubjectId { get; set; }

        /// <summary>
        /// Session id used for the grant.
        /// </summary>
        public string SessionId { get; set; }

        /// <summary>
        /// Client id the grant was issued to.
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// The type of grant.
        /// </summary>
        public string Type { get; set; }
    }
}