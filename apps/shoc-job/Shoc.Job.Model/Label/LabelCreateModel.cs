namespace Shoc.Job.Model.Label;

/// <summary>
/// The label create model
/// </summary>
public class LabelCreateModel
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
}