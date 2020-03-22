using System.Linq;
using Microsoft.AspNetCore.Mvc.Filters;
using TgStickers.Application.Exceptions;

namespace TgStickers.Api.Configuration
{
    public class ModelValidationFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            // noop
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ModelState.IsValid)
            {
                return;
            }

            var messages = context.ModelState.Values
                .SelectMany(state => state.Errors.Select(err => err.ErrorMessage));

            throw new ValidationException(string.Join(", ", messages));
        }
    }

    public class ValidationException : AbstractHandledException
    {
        public ValidationException(string message) : base(message)
        {
        }
    }
}