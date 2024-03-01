using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;

namespace AiLearner_API.Services
{
    public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        private readonly RequestDelegate _next = next;
        private readonly ILogger<ExceptionMiddleware> _logger = logger;

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError("A DbUpdateConcurrencyException was caught: {Exception}", ex);
                await HandleExceptionAsync(httpContext, ex);
            }
            catch(DbUpdateException ex)
            {
                _logger.LogError("A DbUpdateException was caught: {Exception}", ex);
                await HandleExceptionAsync(httpContext, ex);
            }
            catch (ValidationException ex)
            {
                _logger.LogError("A ValidationException was caught: {Exception}", ex);
                await HandleExceptionAsync(httpContext, ex);
            }
            
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogError("An UnauthorizedAccessException was caught: {Exception}", ex);
                await HandleExceptionAsync(httpContext, ex);
            }
            catch (Exception ex)
            {
                _logger.LogError("Something went wrong: {Exception}", ex);
                await HandleExceptionAsync(httpContext, ex);
            }       
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var errorDetails = new
            {
                errorType = exception.GetType().Name,
                errorMessage = exception.Message,
                stackTrace = exception.StackTrace
            };

            var result = JsonSerializer.Serialize(errorDetails);
            return context.Response.WriteAsync(result);
        }
    }
}