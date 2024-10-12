using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Shoc.ApiCore;

/// <summary>
/// The multipart implementation of form file
/// </summary>
public class MultipartFile : IFormFile
{
    /// <summary>
    /// The default buffer size
    /// </summary>
    private const int DEFAULT_BUFFER_SIZE = 80 * 1024;

    /// <summary>
    /// The internal stream
    /// </summary>
    private readonly Stream stream;

    /// <summary>
    /// The headers dictionary
    /// </summary>
    public IHeaderDictionary Headers { get; set; }

    /// <summary>
    /// The length of file stream
    /// </summary>
    public long Length => this.stream.Length;

    /// <summary>
    /// The name of multipart file
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// The file name
    /// </summary>
    public string FileName { get; set; }

    /// <summary>
    /// The given content type
    /// </summary>
    public string ContentType
    {
        get => this.Headers["Content-Type"];
        set => this.Headers["Content-Type"] = value;
    }

    /// <summary>
    /// The content disposition 
    /// </summary>
    public string ContentDisposition
    {
        get => this.Headers["Content-Disposition"];
        set => this.Headers["Content-Disposition"] = value;
    }

        
    /// <summary>
    /// Creates new instance of multipart file from the given stream
    /// </summary>
    /// <param name="stream">The reference to the stream</param>
    /// <param name="name">The name of the multipart file</param>
    /// <param name="filename">The file name</param>
    public MultipartFile(Stream stream, string name, string filename)
    {
        this.stream = stream;
        this.Name = name;
        this.FileName = filename;
        this.Headers = new HeaderDictionary();
    }

    /// <summary>
    /// Copy the existing stream into the target
    /// </summary>
    /// <param name="target">The target to copy to</param>
    public void CopyTo(Stream target)
    {
        // make sure target is given
        if (target == null)
        {
            throw new ArgumentNullException(nameof(target));
        }

        // do copy
        this.stream.CopyTo(target);
    }

    /// <summary>
    /// Copy the existing stream into the target async
    /// </summary>
    /// <param name="target">The target to copy to</param>
    /// <param name="cancellationToken">The cancellation token</param>
    public Task CopyToAsync(Stream target, CancellationToken cancellationToken = default)
    {
        // make sure target is given
        if (target == null)
        {
            throw new ArgumentNullException(nameof(target));
        }
            
        // do copy buffered
        return this.stream.CopyToAsync(target, DEFAULT_BUFFER_SIZE, cancellationToken);
    }

    /// <summary>
    /// Opens the read stream
    /// </summary>
    /// <returns></returns>
    public Stream OpenReadStream()
    {
        return this.stream;
    }
}