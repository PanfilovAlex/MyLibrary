using System.Collections.Generic;

namespace WebApiMyLib.BLL.Services
{
    public class ValidationResult
    {
        private Dictionary<string, List<string>> _errorsDictionary;
        
        public ValidationResult()
        {
            _errorsDictionary = new Dictionary<string, List<string>>();
        }
        public void AddError(string fieldName, string errorMessage)
        {
            List<string> errors;

            if (_errorsDictionary.TryGetValue(fieldName, out errors))
            {
                errors.Add(errorMessage);
            }
            else
            {
                errors = new List<string>();
                errors.Add(errorMessage);
                _errorsDictionary.Add(fieldName, errors);
            }
        }
        public bool IsValid
        {
            get
            {
                return _errorsDictionary.Count == 0;
            }
        }  

        public Dictionary<string, List<string>> Errors
        {
            get
            {
                return _errorsDictionary;
            }
        }
    }
}
