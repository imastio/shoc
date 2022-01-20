using System;
using System.Security.Cryptography;
using System.Text;

namespace Shoc.Core
{
    /// <summary>
    /// The set of crypto extensions
    /// </summary>
    public static class Crypto
    {
        /// <summary>
        /// Creates a safe SHA256 hash of the specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>A hash</returns>
        public static string ToSafeSha256(this string input)
        {
            // check if empty
            if (string.IsNullOrWhiteSpace(input))
            {
                return string.Empty;
            }

            // create sha hasher
            using var sha = SHA256.Create();

            // get bytes
            var bytes = Encoding.UTF8.GetBytes(input);

            // compute hash of bytes
            var hash = sha.ComputeHash(bytes);

            // convert as base64
            return Convert.ToBase64String(hash).Replace("+", "-").Replace("/", "_");
        }

        /// <summary>
        /// Creates a safe SHA512 hash of the specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>A hash</returns>
        public static string ToSafeSha512(this string input)
        {
            // check if empty
            if (string.IsNullOrWhiteSpace(input))
            {
                return string.Empty;
            }

            // create sha hasher
            using var sha = SHA512.Create();

            // get bytes
            var bytes = Encoding.UTF8.GetBytes(input);

            // compute hash of bytes
            var hash = sha.ComputeHash(bytes);

            // convert as base64
            return Convert.ToBase64String(hash).Replace("+", "-").Replace("/", "_");
        }
    }
}