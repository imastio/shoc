namespace Shoc.Package.Model.Command;

/// <summary>
/// The result of run command
/// </summary>
public class CommandRunResult
{
    /// <summary>
    /// The success indicator
    /// </summary>
    public bool Success { get; set; }
    
    /// <summary>
    /// The standard output of the command
    /// </summary>
    public string Output { get; set; }
    
    /// <summary>
    /// The standard error of the command
    /// </summary>
    public string Error { get; set; }
}