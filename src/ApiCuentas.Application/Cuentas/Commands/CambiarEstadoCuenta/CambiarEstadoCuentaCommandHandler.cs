using ApiCuentas.Application.Interfaces;
using ApiCuentas.Domain.Entities;
using MediatR;

namespace ApiCuentas.Application.Cuentas.Commands.CambiarEstadoCuenta
{
    public class CambiarEstadoCuentaCommandHandler : IRequestHandler<CambiarEstadoCuentaCommand, ResultadoCambiarEstado>
    {
        private readonly ICuentaRepository _repositorio;

        public CambiarEstadoCuentaCommandHandler(ICuentaRepository repositorio)
        {
            _repositorio = repositorio;
        }

        public Task<ResultadoCambiarEstado> Handle(CambiarEstadoCuentaCommand request, CancellationToken cancellationToken)
        {
            var cuenta = _repositorio.ObtenerPorId(request.IdCuenta);
            if (cuenta is null)
            {
                return Task.FromResult(new ResultadoCambiarEstado
                {
                    CodigoError = "CUENTA_NO_ENCONTRADA",
                    MensajeError = $"No existe una cuenta con id {request.IdCuenta}"
                });
            }

            cuenta.Estado = request.Operacion == OperacionEstado.ACTIVAR
                ? EstadoCuenta.ACTIVA
                : EstadoCuenta.INACTIVA;

            cuenta.FechaActualizacion = DateTime.UtcNow;

            return Task.FromResult(new ResultadoCambiarEstado { Cuenta = cuenta });
        }
    }
}