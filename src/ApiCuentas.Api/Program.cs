using ApiCuentas.Infrastructure;
using ApiCuentas.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Registra DbContext + Repositorios de Infrastructure (lee la cadena de conexión de appsettings.json)
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(ApiCuentas.Application.Cuentas.Commands.CrearCuenta.CrearCuentaCommand).Assembly));

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

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();