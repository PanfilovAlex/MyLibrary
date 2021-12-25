using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiMyLib.BLL.Services
{
    internal class ValidationException : Exception
    {
        private ValidationResult _validationResult;

        public ValidationException() { }
        public ValidationException(ValidationResult validationResult)
        {
            _validationResult = validationResult;
        }


    }
}
