using System.Collections.Generic;
using Shoc.Core;
using Shoc.Identity.Model.Flow;
using Shoc.Identity.Model.User;

namespace Shoc.Identity.Model;

/// <summary>
/// The credentials evaluation result
/// </summary>
public class CredentialsEvaluationResult
{
    /// <summary>
    /// The valid user value
    /// </summary>
    public UserInternalModel User { get; set; }

    /// <summary>
    /// The OTP value if matched
    /// </summary>
    public OneTimePassModel Otp { get; set; }

    /// <summary>
    /// The validation errors
    /// </summary>
    public List<ErrorDefinition> Errors { get; set; }
}
