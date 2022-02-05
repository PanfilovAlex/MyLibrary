using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebApiMyLib.BLL.Services;
using Microsoft.AspNetCore.Http;

namespace WebApiMyLib.Filters
{
    public class ExceptionFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception is ValidationException validationException)
            {
                context.Result = new ObjectResult(new
                {
                    message = context.Exception.Data,
                })
                {
                    StatusCode = StatusCodes.Status400BadRequest
                };
            }
            context.ExceptionHandled = true;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {

        }
    }
}
