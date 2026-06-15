using Microwave.NET.DataStructures.DTOs;
using Microwave.NET.DataStructures.Enums.Exceptions;
using System.Net;
using System.Text.Json;

namespace Microwave.NET.API.Middleware;

public class LoggingMiddleware(RequestDelegate next, IWebHostEnvironment env)
{
    private static readonly string LogDirectory = Path.Combine(AppContext.BaseDirectory, "logs");
    private static readonly SemaphoreSlim _logLock = new(1, 1);

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (ApiExceptionBase bex)
        {
            await WriteJsonResponseAsync(context, bex.StatusCode,
                ApiResponse.Fail(bex.Message));
        }
        catch (Exception ex)
        {
            await LogToFileAsync(ex, context);

            var message = env.IsDevelopment()
                ? ex.Message
                : "Ocorreu um erro interno. Consulte os logs para mais detalhes.";

            await WriteJsonResponseAsync(context, (int)HttpStatusCode.InternalServerError,
                ApiResponse.Fail(message));
        }
    }

    private static async Task WriteJsonResponseAsync(HttpContext context, int statusCode, object body)
    {
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(JsonSerializer.Serialize(body,
            new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }));
    }

    private static async Task LogToFileAsync(Exception ex, HttpContext context)
    {
        try
        {
            Directory.CreateDirectory(LogDirectory);

            var logFile = Path.Combine(LogDirectory, $"errors-{DateTime.UtcNow:yyyy-MM-dd}.log");
            var entry = BuildLogEntry(ex, context);

            await _logLock.WaitAsync();
            try
            {
                await File.AppendAllTextAsync(logFile, entry);
            }
            finally
            {
                _logLock.Release();
            }
        }
        catch
        {

        }
    }

    private static string BuildLogEntry(Exception ex, HttpContext context)
    {
        var sb = new System.Text.StringBuilder();

        sb.AppendLine("=".PadRight(80, '='));
        sb.AppendLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} UTC]");
        sb.AppendLine($"Request : {context.Request.Method} {context.Request.Path}{context.Request.QueryString}");
        sb.AppendLine($"Exception: {ex.GetType().FullName}");
        sb.AppendLine($"Message  : {ex.Message}");

        if (ex.InnerException != null)
        {
            sb.AppendLine($"Inner Exception: {ex.InnerException.GetType().FullName}");
            sb.AppendLine($"Inner Message  : {ex.InnerException.Message}");
        }

        sb.AppendLine("StackTrace:");
        sb.AppendLine(ex.StackTrace);
        sb.AppendLine();

        return sb.ToString();
    }
}