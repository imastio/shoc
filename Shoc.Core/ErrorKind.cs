namespace Shoc.Core
{
    /// <summary>
    /// The known kinds of error
    /// </summary>
    public enum ErrorKind
    {
        /// <summary>
        /// The unknown error kind
        /// </summary>
        Unknown,

        /// <summary>
        /// The data error kind
        /// </summary>
        Data,

        /// <summary>
        /// The missing object 
        /// </summary>
        NotFound,

        /// <summary>
        /// The validation error kind
        /// </summary>
        Validation,
        
        /// <summary>
        /// The access error kind
        /// </summary>
        Access
    }
}