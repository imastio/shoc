using System;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;

namespace Shoc.ApiCore
{
    /// <summary>
    /// The multipart request helper utilities
    /// </summary>
    public static class MultipartRequestUtility
    {
        /// <summary>
        /// Gets boundary value of the request
        /// Content-Type: multipart/form-data; boundary="----WebKitFormBoundarymx2fSWqWSd0OxQqq"
        /// The spec at https://tools.ietf.org/html/rfc2046#section-5.1 states that 70 characters is a reasonable limit.
        /// </summary>
        /// <param name="contentType">The content type</param>
        /// <param name="lengthLimit">The length limit</param>
        /// <returns></returns>
        public static string GetBoundary(MediaTypeHeaderValue contentType, int lengthLimit)
        {
            // get boundary value from content type
            var boundary = HeaderUtilities.RemoveQuotes(contentType.Boundary).Value;

            // make sure it exits
            if (string.IsNullOrWhiteSpace(boundary))
            {
                throw new InvalidDataException("Missing content-type boundary.");
            }

            // make sure length limit is not exceeded
            if (boundary.Length > lengthLimit)
            {
                throw new InvalidDataException($"Multipart boundary length limit {lengthLimit} exceeded.");
            }

            return boundary;
        }

        /// <summary>
        /// Check if content type is multipart
        /// </summary>
        /// <param name="contentType">THe content type</param>
        /// <returns></returns>
        public static bool IsMultipartContentType(string contentType)
        {
            return !string.IsNullOrEmpty(contentType)
                   && contentType.IndexOf("multipart/", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        /// <summary>
        /// Checks if content disposition header is valid
        /// </summary>
        /// <param name="contentDisposition">The content disposition header</param>
        /// <returns></returns>
        public static bool HasFormDataContentDisposition(ContentDispositionHeaderValue contentDisposition)
        {
            // Content-Disposition: form-data; name="key";
            return contentDisposition != null
                   && contentDisposition.DispositionType.Equals("form-data")
                   && string.IsNullOrEmpty(contentDisposition.FileName.Value)
                   && string.IsNullOrEmpty(contentDisposition.FileNameStar.Value);
        }

        /// <summary>
        /// Checks if file content disposition is given
        /// </summary>
        /// <param name="contentDisposition">The content disposition</param>
        /// <returns></returns>
        public static bool HasFileContentDisposition(ContentDispositionHeaderValue contentDisposition)
        {
            // Content-Disposition: form-data; name="file1"; filename="Misc 002.jpg"
            return contentDisposition != null
                   && contentDisposition.DispositionType.Equals("form-data")
                   && (!string.IsNullOrEmpty(contentDisposition.FileName.Value)
                       || !string.IsNullOrEmpty(contentDisposition.FileNameStar.Value));
        }

        /// <summary>
        /// Gets the encoding of multipart section
        /// </summary>
        /// <param name="section">The given section</param>
        /// <returns></returns>
        public static Encoding GetEncoding(MultipartSection section)
        {
            // get media type header from the section content type
            var hasMediaTypeHeader = MediaTypeHeaderValue.TryParse(section.ContentType, out var mediaType);

            // use UTF-8 by default
            return hasMediaTypeHeader ? mediaType.Encoding: Encoding.UTF8;
        }
    }
}