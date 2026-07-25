using ApiCuentas.Domain.Entities;

namespace ApiCuentas.Application.Interfaces
{
    public interface ICuentaRepository
    {
        IEnumerable<Cuenta> Listar(EstadoCuenta? estado);
        Cuenta? ObtenerPorId(string idCuenta);
        void Agregar(Cuenta cuenta);
        bool Existe(string idCuenta);
    }
}