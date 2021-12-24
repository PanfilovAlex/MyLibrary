using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiMyLib.BLL.Servicies
{
    internal class CategoryExceptions:Exception
    {
        private ValidationResult _validationResult;

        public CategoryExceptions() { }
        public CategoryExceptions(ValidationResult validationResult)
        {
            _validationResult = validationResult;
        }


    }
}
