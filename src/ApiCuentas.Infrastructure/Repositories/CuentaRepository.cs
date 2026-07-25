using ApiCuentas.Application.Interfaces;
using ApiCuentas.Domain.Entities;
using ApiCuentas.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ApiCuentas.Infrastructure.Repositories
{
    public class CuentaRepository : ICuentaRepository
    {
        private readonly AppDbContext _context;

        public CuentaRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Cuenta> Listar(EstadoCuenta? estado)
        {
            var query = _context.Cuentas.AsQueryable();

            if (estado is not null)
            {
                query = query.Where(c => c.Estado == estado);
            }

            return query.ToList();
        }

        public Cuenta? ObtenerPorId(string idCuenta)
        {
            return _context.Cuentas.FirstOrDefault(c => c.IdCuenta == idCuenta);
        }

        public void Agregar(Cuenta cuenta)
        {
            _context.Cuentas.Add(cuenta);
            _context.SaveChanges();
        }

        public void Actualizar(Cuenta cuenta)
        {
            _context.Cuentas.Update(cuenta);
            _context.SaveChanges();
        }
        public bool Existe(string idCuenta)
        {
            return _context.Cuentas.Any(c => c.IdCuenta == idCuenta);
        }
    }
}