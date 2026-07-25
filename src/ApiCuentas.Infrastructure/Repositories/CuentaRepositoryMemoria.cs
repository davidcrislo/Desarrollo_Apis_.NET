using ApiCuentas.Application.Interfaces;
using ApiCuentas.Domain.Entities;

namespace ApiCuentas.Infrastructure.Repositories
{
    public class CuentaRepositoryMemoria : ICuentaRepository
    {
        // Almacenamiento en memoria - más adelante se puede reemplazar
        // por Entity Framework sin tocar Application ni Api.
        private static readonly List<Cuenta> _cuentas = new();

        public IEnumerable<Cuenta> Listar(EstadoCuenta? estado)
        {
            return estado is null
                ? _cuentas
                : _cuentas.Where(c => c.Estado == estado).ToList();
        }

        public Cuenta? ObtenerPorId(string idCuenta)
        {
            return _cuentas.FirstOrDefault(c => c.IdCuenta == idCuenta);
        }

        public void Agregar(Cuenta cuenta)
        {
            _cuentas.Add(cuenta);
        }

        public bool Existe(string idCuenta)
        {
            return _cuentas.Any(c => c.IdCuenta == idCuenta);
        }
    }
}