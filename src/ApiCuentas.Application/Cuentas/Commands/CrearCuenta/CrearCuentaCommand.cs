using ApiCuentas.Domain.Entities;
using MediatR;

namespace ApiCuentas.Application.Cuentas.Commands.CrearCuenta
{
    public class CrearCuentaCommand : IRequest<ResultadoCrearCuenta>
    {
        public string NumeroCuenta { get; set; } = string.Empty;
        public string Titular { get; set; } = string.Empty;
        public string TipoCuenta { get; set; } = string.Empty;
    }

    public class ResultadoCrearCuenta
    {
        public Cuenta? Cuenta { get; set; }
        public string? CodigoError { get; set; }
        public string? MensajeError { get; set; }

        public bool EsExitoso => CodigoError is null;
    }
}