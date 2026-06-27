using LibraryManagement.Domain.Exceptions;
using System.Net;
using System.Text.Json;
using FluentValidation;

namespace LibraryManagement.Api.Middleware
{
    public sealed class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ValidationException exception)
            {
                await HandleValidationExceptionAsync(context, exception);
            }
            catch (DomainException exception)
            {
                await HandleExceptionAsync(
                    context,
                    exception,
                    HttpStatusCode.BadRequest,
                    exception.Message);
            }
            catch (InvalidOperationException exception)
            {
                await HandleExceptionAsync(
                    context,
                    exception,
                    HttpStatusCode.BadRequest,
                    exception.Message);
            }
            catch (UnauthorizedAccessException exception)
            {
                await HandleExceptionAsync(
                    context,
                    exception,
                    HttpStatusCode.Unauthorized,
                    exception.Message);
            }
            catch (Exception exception)
            {
                await HandleExceptionAsync(
                    context,
                    exception,
                    HttpStatusCode.InternalServerError,
                    "An unexpected error occurred.");
            }
        }

        private async Task HandleValidationExceptionAsync(
            HttpContext context,
            ValidationException exception)
        {
            _logger.LogWarning(
                exception,
                "Validation failed while processing request {Method} {Path}",
                context.Request.Method,
                context.Request.Path);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status400BadRequest;

            var response = new
            {
                statusCode = context.Response.StatusCode,
                message = "Validation failed.",
                errors = exception.Errors.Select(error => new
                {
                    field = error.PropertyName,
                    message = error.ErrorMessage
                })
            };

            var json = JsonSerializer.Serialize(response);

            await context.Response.WriteAsync(json);
        }

        private async Task HandleExceptionAsync(
            HttpContext context,
            Exception exception,
            HttpStatusCode statusCode,
            string message)
        {
            _logger.LogError(
                exception,
                "An error occurred while processing request {Method} {Path}",
                context.Request.Method,
                context.Request.Path);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var response = new
            {
                statusCode = context.Response.StatusCode,
                message
            };

            var json = JsonSerializer.Serialize(response);

            await context.Response.WriteAsync(json);
        }
    }
}

