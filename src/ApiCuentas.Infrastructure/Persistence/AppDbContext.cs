using ApiCuentas.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApiCuentas.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Cuenta> Cuentas => Set<Cuenta>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cuenta>(entity =>
            {
                entity.HasKey(c => c.IdCuenta);
                entity.Property(c => c.NumeroCuenta).IsRequired();
                entity.Property(c => c.Titular).IsRequired();
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}