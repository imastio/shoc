using System.Collections.Generic;

namespace Shoc.Job.Model.Label;

/// <summary>
/// The label ensure model
/// </summary>
public class LabelsEnsureModel
{
    /// <summary>
    /// The workspace id 
    /// </summary>
    public string WorkspaceId { get; set; }
    
    /// <summary>
    /// The label names to ensure
    /// </summary>
    public IEnumerable<string> Names { get; set; }
}