using System.Collections.Generic;

namespace Shoc.Mailing.Model;

/// <summary>
/// The structure of email message to send
/// </summary>
public class EmailMessage
{
    /// <summary>
    /// The sender's email
    /// </summary>
    public string FromEmail { get; set; }
    
    /// <summary>
    /// The sender's name
    /// </summary>
    public string FromSender { get; set; }
    
    /// <summary>
    /// The set of receivers
    /// </summary>
    public List<string> To { get; set; }
    
    /// <summary>
    /// The set of copy emails
    /// </summary>
    public List<string> Cc { get; set; }
    
    /// <summary>
    /// The set of hidden copy emails
    /// </summary>
    public List<string> Bcc { get; set; }
    
    /// <summary>
    /// The subject of email
    /// </summary>
    public string Subject { get; set; }
    
    /// <summary>
    /// The email content
    /// </summary>
    public string Body { get; set; }

    /// <summary>
    /// The content resources
    /// </summary>
    public List<ContentResource> Resources { get; set; }
}
