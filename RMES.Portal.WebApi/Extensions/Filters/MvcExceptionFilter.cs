using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace RMES.Portal.WebApi.Extensions.Filters
{
    public class MvcExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<MvcExceptionFilter> _logger;

        public MvcExceptionFilter(ILogger<MvcExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            _logger.LogError(context.Exception, context.Exception.Message);
            context.ExceptionHandled = true;
            context.Result = new JsonResult(new { Code = 500, Message = context.Exception.Message });
        }
    }
}
