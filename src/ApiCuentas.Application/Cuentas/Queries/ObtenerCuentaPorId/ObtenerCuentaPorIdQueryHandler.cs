using ApiCuentas.Application.Interfaces;
using ApiCuentas.Domain.Entities;
using MediatR;

namespace ApiCuentas.Application.Cuentas.Queries.ObtenerCuentaPorId
{
    public class ObtenerCuentaPorIdQueryHandler : IRequestHandler<ObtenerCuentaPorIdQuery, Cuenta?>
    {
        private readonly ICuentaRepository _repositorio;

        public ObtenerCuentaPorIdQueryHandler(ICuentaRepository repositorio)
        {
            _repositorio = repositorio;
        }

        public Task<Cuenta?> Handle(ObtenerCuentaPorIdQuery request, CancellationToken cancellationToken)
        {
            var cuenta = _repositorio.ObtenerPorId(request.IdCuenta);
            return Task.FromResult(cuenta);
        }
    }
}