using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Component.Infrastructure.Validation
{
    public static class ValidationExtension​
    {

        public static ValidationResultInfo Validate(this object obj)
        {
            var validationResultInfo = new ValidationResultInfo();

            var context = new ValidationContext(obj, null, null);

            var validationResults = new Dictionary<string, List<ValidationResult>>();
            foreach (var prop in obj.GetType().GetProperties())
            {
                var attrs = prop
                    .GetCustomAttributes(false)
                    .OfType<ValidationAttribute>()
                    .ToArray();
                if (attrs.Count() > 0)
                {
                    var results = new List<ValidationResult>();
                    var displayAttribute = prop.GetCustomAttribute<DisplayAttribute>();
                    if (displayAttribute != null)
                    {
                        context.DisplayName = displayAttribute.Name;
                    }
                    if (!Validator.TryValidateValue(prop.GetValue(obj), context, results, attrs))
                    {
                        validationResults.Add(prop.Name, results);
                    }
                }
            }
            validationResultInfo.ValidationResults = validationResults;
            return validationResultInfo;
        }

        public static ValidationBaseInfo ValidateBaseInfo(this object obj)
        {
            var validationResultInfo = new ValidationBaseInfo();

            var context = new ValidationContext(obj, null, null);

            foreach (var prop in obj.GetType().GetProperties())
            {
                var attrs = prop
                    .GetCustomAttributes(false)
                    .OfType<ValidationAttribute>()
                    .ToArray();
                if (attrs.Count() > 0)
                {
                    var displayAttribute = prop.GetCustomAttribute<DisplayAttribute>();
                    if (displayAttribute != null)
                    {
                        context.DisplayName = displayAttribute.Name;
                    }
                    var results = new List<ValidationResult>();
                    if (!Validator.TryValidateValue(prop.GetValue(obj), context, results, attrs))
                    {
                        validationResultInfo.ErrorMessages.AddRange(results.Select(x => x.ErrorMessage));
                    }
                }
            }
            return validationResultInfo;
        }

        public static List<ValidationResult> ValidateProperty(this object obj, string name)
        {
            var context = new ValidationContext(obj, null, null);
            var prop = obj.GetType().GetProperty(name);
            var attrs = prop
                    .GetCustomAttributes(false)
                    .OfType<ValidationAttribute>()
                    .ToArray();

            var results = new List<ValidationResult>();

            Validator.TryValidateProperty(prop.GetValue(obj), context, results);

            return results;
        }
    }
}
