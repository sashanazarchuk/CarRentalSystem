using CarRentalSystem.Application.Exceptions;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Text.Json;

namespace CarRentalSystem.API.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
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
                _logger.LogError(ex, ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var (code, message) = exception switch
            {
                NotFoundException notFoundEx => (HttpStatusCode.NotFound, notFoundEx.Message),
                BadRequestException badRequestEx => (HttpStatusCode.BadRequest, badRequestEx.Message),
                ForbiddenException forbiddenEx => (HttpStatusCode.Forbidden, forbiddenEx.Message),
                InvalidLoginException loginEx => (HttpStatusCode.BadRequest, loginEx.Message),
                SecurityTokenException securityEx => (HttpStatusCode.Unauthorized, securityEx.Message),
                RefreshTokenUpdateException refreshTokenEx => (HttpStatusCode.BadRequest, refreshTokenEx.Message),
                _ => (HttpStatusCode.InternalServerError, "An unexpected error occurred.")
            };

            var result = JsonSerializer.Serialize(new
            {
                error = message
            });

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            return context.Response.WriteAsync(result);
        }
    }
}