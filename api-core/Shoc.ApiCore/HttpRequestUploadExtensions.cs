using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;

namespace Shoc.ApiCore;

/// <summary>
/// The HTTP Request extensions for file upload
/// </summary>
public static class HttpRequestUploadExtensions
{
    /// <summary>
    /// The default form options
    /// </summary>
    private static readonly FormOptions DEFAULT_FORM_OPTIONS = new();

    /// <summary>
    /// Stream the file
    /// </summary>
    /// <param name="request">The request</param>
    /// <param name="func">The consumer function</param>
    /// <returns></returns>
    public static async Task<FormValueProvider> StreamFiles(this HttpRequest request, Func<IFormFile, Task> func)
    {
        // make sure multipart request is being handled
        if (!MultipartRequestUtility.IsMultipartContentType(request.ContentType))
        {
            throw new Exception($"Expected a multipart request, but got {request.ContentType}");

        }

        // Used to accumulate all the form url encoded key value pairs in the request.
        var formAccumulator = new KeyValueAccumulator();

        // get boundary of multipart request
        var boundary = MultipartRequestUtility.GetBoundary(MediaTypeHeaderValue.Parse(request.ContentType), DEFAULT_FORM_OPTIONS.MultipartBoundaryLengthLimit);
            
        // create multipart reader
        var reader = new MultipartReader(boundary, request.Body);

        // for the section
        MultipartSection section;

        try
        {
            // section may have already been read (if for example model binding is not disabled)
            section = await reader.ReadNextSectionAsync();
        }
        catch (IOException)
        {
            section = null;
        }

        // while there is any section available 
        while (section != null)
        {
            var hasContentDispositionHeader = ContentDispositionHeaderValue.TryParse(section.ContentDisposition, out var contentDispositionHeader);

            if (hasContentDispositionHeader)
            {
                if (contentDispositionHeader.IsFileDisposition())
                {
                    var fileSection = section.AsFileSection();

                    if (fileSection == null)
                    {
                        throw new NullReferenceException("Section is null");
                    }

                    // process file stream
                    await func(new MultipartFile(fileSection.FileStream, fileSection.Name, fileSection.FileName)
                    {
                        ContentType = fileSection.Section.ContentType,
                        ContentDisposition = fileSection.Section.ContentDisposition
                    });
                }
                else if (contentDispositionHeader.IsFormDisposition())
                {
                    // Content-Disposition: form-data; name="key"
                    // Do not limit the key name length here because the multipart headers length limit is already in effect.
                    var key = HeaderUtilities.RemoveQuotes(contentDispositionHeader.Name);

                    // get encoding
                    var encoding = MultipartRequestUtility.GetEncoding(section);

                    // make sure encoding is given
                    if (encoding == null)
                    {
                        throw new NullReferenceException("Null encoding");
                    }

                    // create stream reader with buffer size 1024 and leaving it open
                    using var streamReader = new StreamReader(section.Body, encoding, true, 1024, true);

                    // The value length limit is enforced by MultipartBodyLengthLimit
                    var value = await streamReader.ReadToEndAsync();
                        
                    // check undefined values
                    if (string.Equals(value, "undefined", StringComparison.OrdinalIgnoreCase))
                    {
                        value = string.Empty;
                    }

                    // add to form accumulator
                    formAccumulator.Append(key.Value, value);

                    // make sure value is not exceeded
                    if (formAccumulator.ValueCount > DEFAULT_FORM_OPTIONS.ValueCountLimit)
                    {
                        throw new InvalidDataException($"Form key count limit {DEFAULT_FORM_OPTIONS.ValueCountLimit} exceeded.");
                    }
                }
            }

            // Drains any remaining section body that has not been consumed and reads the headers for the next section.
            //section = request.Body.CanSeek && request.Body.Position == request.Body.Length ? null : await reader.ReadNextSectionAsync();
            try
            {
                section = await reader.ReadNextSectionAsync();
            }
            catch (IOException)
            {
                section = null;
            }
        }

        // bind form data to a model
        var formValueProvider = new FormValueProvider(BindingSource.Form, new FormCollection(formAccumulator.GetResults()), CultureInfo.CurrentCulture);

        return formValueProvider;
    }
}