namespace Shoc.Identity.Model
{
    /// <summary>
    /// The definitions of known claims
    /// </summary>
    public static class KnownClaims
    {
        /// <summary>
        /// The subject claim definition
        /// </summary>
        public const string SUBJECT = "sub";

        /// <summary>
        /// The id claim definition
        /// </summary>
        public const string ID = "id";

        /// <summary>
        /// The preferred username claim definition
        /// </summary>
        public const string PREFERRED_USERNAME = "preferred_username";

        /// <summary>
        /// The email claim definition
        /// </summary>
        public const string EMAIL = "email";

        /// <summary>
        /// The email verified claim definition
        /// </summary>
        public const string EMAIL_VERIFIED = "email_verified";

        /// <summary>
        /// The phone number claim definition
        /// </summary>
        public const string PHONE_NUMBER = "phone_number";

        /// <summary>
        /// The phone number verified claim definition
        /// </summary>
        public const string PHONE_NUMBER_VERIFIED = "phone_number_verified";

        /// <summary>
        /// The type claim definition
        /// </summary>
        public const string USER_TYPE = "user_type";

        /// <summary>
        /// The name claim definition
        /// </summary>
        public const string NAME = "name";

        /// <summary>
        /// The given name claim definition
        /// </summary>
        public const string GIVEN_NAME = "given_name";

        /// <summary>
        /// The family name claim definition
        /// </summary>
        public const string FAMILY_NAME = "family_name";

        /// <summary>
        /// The zone info claim definition
        /// </summary>
        public const string ZONE_INFO = "zoneinfo";

        /// <summary>
        /// The picture claim definition
        /// </summary>
        public const string PICTURE = "picture";

        /// <summary>
        /// The scope claim definition
        /// </summary>
        public const string SCOPE = "scope";
    }
}