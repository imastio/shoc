using System.Collections.Generic;

namespace Shoc.ObjectAccess.Model;

/// <summary>
/// Access evaluation result
/// </summary>
public class AccessEvaluationResult
{
    /// <summary>
    /// The evaluation result code
    /// </summary>
    public string Result { get; set; }
    
    /// <summary>
    /// The list of rejected permissions if any
    /// </summary>
    public ISet<string> RejectedPermissions { get; set; }
}