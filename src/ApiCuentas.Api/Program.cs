using ApiCuentas.Application.Interfaces;
using ApiCuentas.Infrastructure.Repositories;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Inyección de dependencias
builder.Services.AddSingleton<ICuentaRepository, CuentaRepositoryMemoria>();

// Registro de MediatR: escanea el ensamblado de Application buscando todos los Handlers
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(ApiCuentas.Application.Cuentas.Commands.CrearCuenta.CrearCuentaCommand).Assembly));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();