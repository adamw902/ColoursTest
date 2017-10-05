using ColoursTest.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace ColoursTest.Web.Filters
{
    public class CustomExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public CustomExceptionFilterAttribute(ILogger<CustomExceptionFilterAttribute> logger)
        {
            this.Logger = logger;
        }

        private ILogger<CustomExceptionFilterAttribute> Logger { get; }

        public override void OnException(ExceptionContext context)
        {
            var message = context.Exception.Message;
            switch (context.Exception)
            {
                case IncorrectFormatException _:
                    message = ((IncorrectFormatException) context.Exception).CustomMessage;
                    context.Result = new NotFoundObjectResult(message);
                    break;
                case IncorrectIdException _:
                    message = ((IncorrectIdException) context.Exception).CustomMessage;
                    context.Result = new NotFoundObjectResult(message);
                    break;
                default:
                    context.Result = new StatusCodeResult(500);
                    break;
            }
            this.Logger.LogError(message);
        }
    }
}