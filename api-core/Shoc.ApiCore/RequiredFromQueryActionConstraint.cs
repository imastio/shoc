using Microsoft.AspNetCore.Mvc.ActionConstraints;

namespace Shoc.ApiCore;

/// <summary>
/// The required query parameter constraint
/// </summary>
public class RequiredFromQueryActionConstraint : IActionConstraint
{
    /// <summary>
    /// The parameter name
    /// </summary>
    private readonly string parameter;

    /// <summary>
    /// Creates new constraint for parameter
    /// </summary>
    /// <param name="parameter">The target parameter</param>
    public RequiredFromQueryActionConstraint(string parameter)
    {
        this.parameter = parameter;
    }

    /// <summary>
    /// The order of constraint
    /// </summary>
    public int Order => 999;

    /// <summary>
    /// Define acceptance of constraint
    /// </summary>
    /// <param name="context">The context</param>
    /// <returns></returns>
    public bool Accept(ActionConstraintContext context)
    {
        return context.RouteContext.HttpContext.Request.Query.ContainsKey(this.parameter);
    }
}
