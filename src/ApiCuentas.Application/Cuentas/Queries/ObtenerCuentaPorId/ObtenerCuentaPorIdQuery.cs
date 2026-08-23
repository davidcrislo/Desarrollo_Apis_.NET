using ApiCuentas.Domain.Entities;
using MediatR;

namespace ApiCuentas.Application.Cuentas.Queries.ObtenerCuentaPorId
{
    public class ObtenerCuentaPorIdQuery : IRequest<Cuenta?>
    {
        public string IdCuenta { get; set; } = string.Empty;

        public ObtenerCuentaPorIdQuery(string idCuenta)
        {
            IdCuenta = idCuenta;
        }
    }
}