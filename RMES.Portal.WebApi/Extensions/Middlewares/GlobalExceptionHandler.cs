using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace RMES.Portal.WebApi.Extensions.Middlewares
{
    public class GlobalExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        private readonly RequestDelegate _next;

        public GlobalExceptionHandler(RequestDelegate next, ILogger<GlobalExceptionHandler> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch(Exception ex)
            {
                context.Response.Clear();
                context.Response.StatusCode = StatusCodes.Status200OK;

                _logger.LogError(ex, ex.Message);

                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync("{\"Code\": 500, \"\": \"" + ex.Message + "\"}");
                return;
            }
        }
    }
}
