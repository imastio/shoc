using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Shoc.ApiCore
{
    /// <summary>
    /// The required "FromQuery" attribute
    /// </summary>
    public class RequiredFromQueryAttribute : FromQueryAttribute, IParameterModelConvention
    {
        /// <summary>
        /// Creates new instance of attribute
        /// </summary>
        /// <param name="name">The query parameter name</param>
        public RequiredFromQueryAttribute(string name = null)
        {
            this.Name = name;
        }

        /// <summary>
        /// Apply the attribute to parameter
        /// </summary>
        /// <param name="parameter">The target parameter</param>
        public void Apply(ParameterModel parameter)
        {
            // no any 
            if (parameter.Action.Selectors.Count == 0)
            {
                return;
            }

            // get the last selector
            var selector = parameter.Action.Selectors.Last();

            // add constraint
            selector.ActionConstraints.Add(new RequiredFromQueryActionConstraint(parameter.BindingInfo?.BinderModelName ?? parameter.ParameterName));
        }
    }
}