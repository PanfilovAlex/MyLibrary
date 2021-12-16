using WebApiMyLib.BLL.Interfaces;
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
        public bool IsValid => _modelState.IsValid;

        public void AddError(string key, string errorMessage)
        {
            _modelState.AddModelError(key, errorMessage);
        }
    }
}
