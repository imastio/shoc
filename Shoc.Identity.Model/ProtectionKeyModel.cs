namespace Shoc.Identity.Model
{
    /// <summary>
    /// The protection key model
    /// </summary>
    public class ProtectionKeyModel
    {
        /// <summary>
        /// The key id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The friendly name of key
        /// </summary>
        public string FriendlyName { get; set; }

        /// <summary>
        /// The XML representation of key
        /// </summary>
        public string Xml { get; set; }
    }
}