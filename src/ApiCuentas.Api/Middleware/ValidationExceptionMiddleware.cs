using System.Diagnostics;
using System.Net;
using FluentValidation;

namespace ApiCuentas.Api.Middleware;

public static class ValidationExceptionMiddlewareExtensions
{
    public static IApplicationBuilder UseValidationExceptionHandling(this IApplicationBuilder app)
    {
        return app.Use(async (context, next) =>
        {
            var logger = context.RequestServices.GetRequiredService<ILoggerFactory>()
                .CreateLogger("ApiCuentas.Api.Middleware.ValidationExceptionMiddleware");

            var stopwatch = Stopwatch.StartNew();

            try
            {
                await next(context);
                stopwatch.Stop();

                // Forma correcta de loguear: placeholders con nombre (structured logging),
                // nunca interpolar strings ($"..."). Así Seq puede indexar y filtrar
                // por cada propiedad (Method, Path, StatusCode, etc.) en vez de tratar
                // todo el mensaje como texto plano.
                logger.LogInformation(
                    "HTTP {Method} {Path} respondió {StatusCode} en {ElapsedMilliseconds} ms",
                    context.Request.Method,
                    context.Request.Path,
                    context.Response.StatusCode,
                    stopwatch.ElapsedMilliseconds);
            }
            catch (ValidationException ex)
            {
                stopwatch.Stop();

                var errors = ex.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());

                logger.LogWarning(
                    "HTTP {Method} {Path} falló validación en {ElapsedMilliseconds} ms: {@Errors}",
                    context.Request.Method,
                    context.Request.Path,
                    stopwatch.ElapsedMilliseconds,
                    errors);

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                await context.Response.WriteAsJsonAsync(new
                {
                    error = "Uno o más campos no son válidos.",
                    detalles = errors,
                });
            }
        });
    }
}
