﻿#region License
//
// Copyright (c) 2008-2015, Dolittle (http://www.dolittle.com)
//
// Licensed under the MIT License (http://opensource.org/licenses/MIT)
//
// You may not use this file except in compliance with the License.
// You may obtain a copy of the license at 
//
//   http://github.com/dolittle/Bifrost/blob/master/MIT-LICENSE.txt
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Bifrost.Commands;
using Bifrost.Validation;
using FluentValidation;
using ValidationResult = Bifrost.Validation.ValidationResult;

namespace Bifrost.FluentValidation.Commands
{
    /// <summary>
    /// Represents a command business validator that is constructed from discovered rules.
    /// </summary>
    public class ComposedCommandBusinessValidator<T> : BusinessValidator<T>, ICanValidate<T>, ICommandInputValidator where T : class, ICommand
    {
        /// <summary>
        /// Instantiates an Instance of a <see cref="ComposedCommandBusinessValidator{T}"/>
        /// </summary>
        /// <param name="propertyTypesAndValidators">A collection of dynamically discovered validators to use</param>
        public ComposedCommandBusinessValidator(IDictionary<Type, IEnumerable<IValidator>> propertyTypesAndValidators)
        {
            foreach (var propertyType in propertyTypesAndValidators.Keys)
            {
                var validators = propertyTypesAndValidators[propertyType];

                if (validators == null || !validators.Any()) 
                    continue;

                var validator = GetValidator(validators);

                var properties = GetPropertiesWithType(propertyType);
                foreach (var property in properties)
                {
                    var expression = BuildGetExpression(property);
                    RuleFor(expression)
                        .DynamicValidationRule(validator, property.Name);
                }
            }
        }

        IValidator GetValidator(IEnumerable<IValidator> propertyTypesAndValidator)
        {
            return new ComposedValidator<IAmValidatable>(propertyTypesAndValidator);
        }

        IEnumerable<PropertyInfo> GetPropertiesWithType(Type propertyType)
        {
            var commandType = typeof(T);
            var properties = commandType.GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(p => p.PropertyType == propertyType);
            return properties;
        }

#pragma warning disable 1591 // Xml Comments
        public IEnumerable<ValidationResult> ValidateFor(ICommand command)
        {
            return ValidateFor(command as T);
        }

        public virtual IEnumerable<ValidationResult> ValidateFor(T command)
        {
            var result = Validate(command as T);
            return from error in result.Errors
                   select new ValidationResult(error.ErrorMessage, new[] { error.PropertyName });
        }

        IEnumerable<ValidationResult> ICanValidate.ValidateFor(object target)
        {
            return ValidateFor((T)target);
        }
#pragma warning restore 1591 // Xml Comments

        static Expression<Func<T, IAmValidatable>> BuildGetExpression(PropertyInfo propertyInfo)
        {
            var type = typeof(T);
            var arg = Expression.Parameter(type, "x");
            Expression expr = arg;

            expr = Expression.Property(expr, propertyInfo);

            return Expression.Lambda<Func<T, IAmValidatable>>(expr, arg);
        }
    }
}