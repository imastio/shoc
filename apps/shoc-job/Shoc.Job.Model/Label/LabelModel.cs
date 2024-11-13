using System;

namespace Shoc.Job.Model.Label;

/// <summary>
/// The label model
/// </summary>
public class LabelModel
{
    /// <summary>
    /// The id of the label
    /// </summary>
    public string Id { get; set; }
    
    /// <summary>
    /// The workspace id 
    /// </summary>
    public string WorkspaceId { get; set; }
    
    /// <summary>
    /// The label name
    /// </summary>
    public string Name { get; set; }
 
    /// <summary>
    /// The creation time
    /// </summary>
    public DateTime Created { get; set; }
    
    /// <summary>
    /// The modification time
    /// </summary>
    public DateTime Updated { get; set; }
}