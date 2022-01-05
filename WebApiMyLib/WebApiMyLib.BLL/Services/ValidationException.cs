using System;
using System.Collections;
using System.Collections.Generic;

namespace WebApiMyLib.BLL.Services
{
    public class ValidationException : Exception
    {
        private Dictionary<string, List<string>> exceptions; 
        public ValidationException(ValidationResult validationResult)
        {
            exceptions = validationResult.Errors;
        }
        public override IDictionary Data
        {
            get
            {
                return exceptions;
            }
        }
    }
}
