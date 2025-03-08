using System;

namespace Shoc.Job.Quartz;

/// <summary>
/// The exception indicating invalid quartz data
/// </summary>
public class FatalQuartzException : Exception
{
    /// <summary>
    /// Creates new instance of fatal quartz exception
    /// </summary>
    /// <param name="message">The message</param>
    /// <param name="innerException">The inner exception if available</param>
    public FatalQuartzException(string message, Exception innerException = null) : base(message, innerException)
    {
    }
}