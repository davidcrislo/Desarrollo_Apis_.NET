using ApiCuentas.Application.Interfaces;
using ApiCuentas.Domain.Entities;
using MediatR;

namespace ApiCuentas.Application.Cuentas.Commands.CrearCuenta
{
    public class CrearCuentaCommandHandler : IRequestHandler<CrearCuentaCommand, ResultadoCrearCuenta>
    {
        private readonly ICuentaRepository _repositorio;

        public CrearCuentaCommandHandler(ICuentaRepository repositorio)
        {
            _repositorio = repositorio;
        }

        public Task<ResultadoCrearCuenta> Handle(CrearCuentaCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.NumeroCuenta) || string.IsNullOrWhiteSpace(request.Titular))
            {
                return Task.FromResult(new ResultadoCrearCuenta
                {
                    CodigoError = "DATOS_INVALIDOS",
                    MensajeError = "NumeroCuenta y Titular son obligatorios"
                });
            }

            var nuevaCuenta = new Cuenta
            {
                IdCuenta = Guid.NewGuid().ToString(),
                NumeroCuenta = request.NumeroCuenta,
                Titular = request.Titular,
                TipoCuenta = request.TipoCuenta,
                Estado = EstadoCuenta.ACTIVA
            };

            _repositorio.Agregar(nuevaCuenta);

            return Task.FromResult(new ResultadoCrearCuenta { Cuenta = nuevaCuenta });
        }
    }
}