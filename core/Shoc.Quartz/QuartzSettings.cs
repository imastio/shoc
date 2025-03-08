namespace Shoc.Quartz;

/// <summary>
/// The quartz settings descriptor
/// </summary>
public class QuartzSettings
{
    /// <summary>
    /// The scheduler name
    /// </summary>
    public string SchedulerName { get; set; }
    
    /// <summary>
    /// The number of threads in the thread pool
    /// </summary>
    public int MaxConcurrency { get; set; }
}