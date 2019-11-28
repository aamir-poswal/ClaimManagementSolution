using ClaimManagement.Application.Common.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace InsuranceClaimHandler.API
{
    public class CustomExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public CustomExceptionHandlerMiddleware(RequestDelegate next, ILogger<CustomExceptionHandlerMiddleware> logger)
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
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            _logger.LogError($"Exception {exception.ToString()}");
            var code = HttpStatusCode.InternalServerError;

            var result = string.Empty;

            if (exception is NotFoundException)
            {
                var notfoundException = exception as NotFoundException;
                result = JsonConvert.SerializeObject(new { error = notfoundException.Message });
                code = HttpStatusCode.NotFound;
            }

            context.Response.StatusCode = (int)code;

            if (string.IsNullOrEmpty(result))
            {
                result = JsonConvert.SerializeObject(new { error = "There is something wrong, try again in few moments, or contact our support team." });
            }

            return context.Response.WriteAsync(result);
        }
    }


}
