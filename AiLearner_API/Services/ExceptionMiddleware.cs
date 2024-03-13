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
        private readonly string _logFilePath = "logs/log.txt";
        private readonly JsonSerializerOptions _jsonSerializerOptions = new()
        {
            WriteIndented = true,
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

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
            catch (DbUpdateException ex)
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
            catch (InvalidOperationException ex)
            {
                _logger.LogError("An InvalidOperationException was caught: {Exception}", ex);
                await HandleExceptionAsync(httpContext, ex);
            } 
            catch (Exception ex)
            {
                _logger.LogError("Something went wrong: {Exception}", ex);
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var errorDetails = new
            {
                ErrorType = exception.GetType().Name,
                ErrorMessage = exception.Message,
                InnerException = exception.InnerException?.Message,
                exception.StackTrace,
                context.Request.Path,
                context.Request.Method,
                context.Request.QueryString, 
            };

            var result = JsonSerializer.Serialize(errorDetails, _jsonSerializerOptions);

            result = result.Replace("\\r\\n", "\r\n");
            result = result.Replace("\"stackTrace\": \"", "\"stackTrace\": \"\n");

            var logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {result}{Environment.NewLine}{Environment.NewLine}";
            await File.AppendAllTextAsync(_logFilePath, logEntry);


            var responseResult = JsonSerializer.Serialize(new
            {
                errorDetails.ErrorType,
                errorDetails.ErrorMessage
            }, _jsonSerializerOptions);

            await context.Response.WriteAsync(responseResult);
        }
    }
}