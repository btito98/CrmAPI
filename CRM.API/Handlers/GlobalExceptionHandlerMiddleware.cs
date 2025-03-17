using CRM.API.Helpers;
using CRM.Application.Exceptions;
using System.Net;
using System.Text.Json;

namespace CRM.API.Handlers
{
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

        public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
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
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var response = new ErroResponse();
            var statusCode = HttpStatusCode.InternalServerError;

            switch (exception)
            {
                case NotFoundException notFoundException:
                    statusCode = HttpStatusCode.NotFound;
                    response = new ErroResponse(404, notFoundException.Message);
                    _logger.LogWarning(exception, "Recurso não encontrado: {Message}", exception.Message);
                    break;

                case ValidationException validationException:
                    statusCode = HttpStatusCode.BadRequest;
                    response = new ErroResponse(400, "Erro de validação", validationException.Errors);
                    _logger.LogWarning(exception, "Erro de validação: {@ValidationErrors}", validationException.Errors);
                    break;

                case UnauthorizedAccessException unauthorizedException:
                    statusCode = HttpStatusCode.Unauthorized;
                    response = new ErroResponse(401, "Acesso não autorizado");
                    _logger.LogWarning(exception, "Acesso não autorizado: {Message}", exception.Message);
                    break;

                default:
                    response = new ErroResponse(500, "Ocorreu um erro interno no servidor");
                    _logger.LogError(exception, "Erro não tratado: {Message}", exception.Message);
                    break;
            }

            context.Response.StatusCode = (int)statusCode;

            var result = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await context.Response.WriteAsync(result);
        }
    }

    public static class GlobalExceptionHandlerMiddlewareExtensions
    {
        public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<GlobalExceptionHandlerMiddleware>();
        }
    }
}