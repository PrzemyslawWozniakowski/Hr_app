using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using hr_app.Models;

namespace hr_app.Utilities
{
    [AttributeUsage(AttributeTargets.Property)]
    public class SalaryCorrectRangeAttributeAttribute : ValidationAttribute
    {
        public bool ValidateSalaryFrom { get; set; }
        public bool ValidateSalaryTo { get; set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var model = validationContext.ObjectInstance as JobOffer;

            if (model != null)
            {
                if (model.SalaryFrom > model.SalaryTo)
                {
                    return new ValidationResult(string.Empty);
                }
            }

            return ValidationResult.Success;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class DateCorrectRangeAttribute : ValidationAttribute
    {
     
        public bool ValidateValidUntil { get; set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var model = validationContext.ObjectInstance as JobOffer;

            if (model != null)
            {
                if ( model.ValidUntil < DateTime.Now.Date && ValidateValidUntil)
                {
                    return new ValidationResult(string.Empty);
                }
            }

            return ValidationResult.Success;
        }
    }
}


