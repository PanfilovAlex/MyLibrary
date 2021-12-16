using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace WebApiMyLib.BLL.Servicies
{
    internal class ModelStateWrapper : IValidationDictionary
    {
        private ModelStateDictionary _modelState;

        public ModelStateWrapper(ModelStateDictionary modelState)
        {
            _modelState = modelState;
        }
        public bool IsValid => throw new NotImplementedException();

        public void AddError(string key, string error)
        {
            throw new NotImplementedException();
        }
    }
}
