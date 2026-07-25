using ApiCuentas.Domain.Entities;
using MediatR;

namespace ApiCuentas.Application.Cuentas.Queries.ListarCuentas
{
    public class ListarCuentasQuery : IRequest<IEnumerable<Cuenta>>
    {
        public EstadoCuenta? Estado { get; set; }

        public ListarCuentasQuery(EstadoCuenta? estado)
        {
            Estado = estado;
        }
    }
}