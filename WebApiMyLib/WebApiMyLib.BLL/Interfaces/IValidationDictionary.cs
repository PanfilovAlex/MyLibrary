using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiMyLib.BLL.Interfaces
{
    public interface IValidationDictionary
    {
        void AddError(string key, string error);
        bool IsValid { get; }
    }
}
