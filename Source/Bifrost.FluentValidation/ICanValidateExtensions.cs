﻿using Bifrost.Validation;
using System.Collections.Generic;
using System.Linq;

namespace Bifrost.FluentValidation
{
    /// <summary>
    /// Extensions to the <see cref="ICanValidate"/> interface to support the FluentValidation concept of RuleSets.
    /// </summary>
    public static class ICanValidateExtensions
    {
        /// <summary>
        /// Allows a ruleset to be specified for the validation
        /// </summary>
        /// <param name="validator">Instance of the validator being extended</param>
        /// <param name="target">Instance of the object to be validated</param>
        /// <param name="ruleSet"></param>
        /// <param name="includeDefaultRuleset">Optional parameter indicating whether the default ruleset should be used.  True by default.</param>
        /// <returns>An enumeration of ValidationResults.  An entry for each validation error or an empty enumeration for Valid.</returns>
        public static IEnumerable<ValidationResult> ValidateFor(this ICanValidate validator, object target, string ruleSet, bool includeDefaultRuleset = true)
        {
            var rulesets = includeDefaultRuleset ? "default," + ruleSet : ruleSet;
            var method = validator.GetType().GetMethods().First(m => m.Name == "ValidateFor" && m.GetParameters().Count() == 2);
            //var genericMethod = method.MakeGenericMethod(target.GetType());
            return (IEnumerable<ValidationResult>)method.Invoke(validator, new[] { target, rulesets });
        }
    }
}
