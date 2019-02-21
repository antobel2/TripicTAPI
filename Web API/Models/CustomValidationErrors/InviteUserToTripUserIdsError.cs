using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Web_API.Models.CustomValidationErrors
{
    public class InviteUserToTripUserIdsError : ValidationAttribute
    {
        public InviteUserToTripUserIdsError() { }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            InviteUserToTripDTO model = (InviteUserToTripDTO)validationContext.ObjectInstance;
            List<String> ids = model.UserIds;

            if (ids == null)
            {
                return new ValidationResult(ErrorMessage);
            }

            if (ids.Count < 1)
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}