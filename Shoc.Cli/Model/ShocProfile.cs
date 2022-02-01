namespace Shoc.Cli.Model
{
    /// <summary>
    /// The shoc profile definition
    /// </summary>
    public class ShocProfile
    {
        /// <summary>
        /// The profile name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The api root
        /// </summary>
        public string Api { get; set; }

        /// <summary>
        /// The authority root
        /// </summary>
        public string Authority { get; set; }
    }
}