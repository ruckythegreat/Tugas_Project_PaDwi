using System.Net;
using System.Text.Json;
using Microsoft.Data.SqlClient;
using SMKRestaurantAPI.Models;

namespace SMKRestaurantAPI.Middleware
{
    /// <summary>
    /// Menangkap semua exception yang tidak tertangani agar API
    /// tidak crash dan selalu mengembalikan JSON error yang konsisten.
    /// </summary>
    public class ExceptionHandlingMiddleware
    {
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger,
            IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
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
            var traceId = context.TraceIdentifier;
            _logger.LogError(exception, "Unhandled exception. TraceId={TraceId}", traceId);

            var (statusCode, message) = MapException(exception);

            var response = new ApiErrorResponse
            {
                Success = false,
                Message = message,
                Detail = _env.IsDevelopment() ? exception.Message : null,
                TraceId = traceId
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(response, JsonOptions));
        }

        private static (HttpStatusCode StatusCode, string Message) MapException(Exception exception)
        {
            return exception switch
            {
                SqlException => (
                    HttpStatusCode.ServiceUnavailable,
                    "Gagal terhubung ke database. Periksa SQL Server dan connection string."),

                ArgumentException => (
                    HttpStatusCode.BadRequest,
                    "Permintaan tidak valid."),

                UnauthorizedAccessException => (
                    HttpStatusCode.Unauthorized,
                    "Akses ditolak."),

                KeyNotFoundException => (
                    HttpStatusCode.NotFound,
                    "Data tidak ditemukan."),

                _ => (
                    HttpStatusCode.InternalServerError,
                    "Terjadi kesalahan pada server.")
            };
        }
    }
}
