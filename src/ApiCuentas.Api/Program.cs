using ApiCuentas.Api.Middleware;
using ApiCuentas.Application.Common.Behaviors;
using ApiCuentas.Infrastructure;
using ApiCuentas.Infrastructure.Persistence;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Serilog;

// Bootstrap logger: captura errores que puedan ocurrir ANTES de que el logger
// "real" (configurado más abajo con appsettings.json) llegue a inicializarse.
Log.Logger = new LoggerConfiguration()
    .WriteTo.Async(a => a.Console())
    .CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Reemplaza el logging por defecto de ASP.NET Core por Serilog.
    builder.Host.UseSerilog((context, loggerConfiguration) =>
    {
        var applicationName = context.Configuration["APPLICATION_NAME"] ?? "ApiCuentas.Api";
        var seqUrl = context.Configuration["Seq:ServerUrl"] ?? "http://localhost:5341";

        loggerConfiguration
            .ReadFrom.Configuration(context.Configuration)
            .Enrich.FromLogContext()
            .Enrich.WithProperty("Application", applicationName)
            // Los sinks corren en un buffer en background (Serilog.Sinks.Async):
            // el hilo que escribe el log no espera a que la escritura a consola/Seq termine.
            .WriteTo.Async(a => a.Console(
                outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Application} {Message:lj}{NewLine}{Exception}"))
            .WriteTo.Async(a => a.Seq(seqUrl));
    });

    builder.Services.AddControllers();
    builder.Services.AddOpenApi();

    // Registra DbContext + Repositorios de Infrastructure (lee la cadena de conexión de appsettings.json)
    builder.Services.AddInfrastructure(builder.Configuration);

    builder.Services.AddMediatR(cfg =>
        cfg.RegisterServicesFromAssembly(typeof(ApiCuentas.Application.Cuentas.Commands.CrearCuenta.CrearCuentaCommand).Assembly));

    // Registra todos los Validators de FluentValidation que existan en el proyecto Application
    // (por ahora: CrearCuentaCommandValidator).
    builder.Services.AddValidatorsFromAssembly(
        typeof(ApiCuentas.Application.Cuentas.Commands.CrearCuenta.CrearCuentaCommand).Assembly);

    // Conecta FluentValidation al pipeline de MediatR: antes de que un Handler se ejecute,
    // el ValidationBehavior corre el Validator correspondiente y, si falla, tira ValidationException.
    builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

    var app = builder.Build();

    // Aplica migraciones pendientes de EF Core contra la base configurada.
    // Corre en cada arranque; si ya está todo aplicado, no hace nada.
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        dbContext.Database.Migrate();
    }

    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
        app.MapScalarApiReference();
    }

    // Captura ValidationException (de FluentValidation) y loguea cada request de forma
    // estructurada, con placeholders con nombre para que Seq pueda indexar y filtrar.
    app.UseSerilogRequestLogging();


    // Middleware de Serilog: loguea cada request HTTP (método, path, status code, duración)
    // de forma estructurada automáticamente, sin tener que escribirlo a mano por endpoint.
    app.UseValidationExceptionHandling();

    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "La aplicación terminó inesperadamente durante el arranque");
}
finally
{
    Log.CloseAndFlush();
}
