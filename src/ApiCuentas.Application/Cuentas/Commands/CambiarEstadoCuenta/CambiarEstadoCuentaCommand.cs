using ApiCuentas.Domain.Entities;
using MediatR;

namespace ApiCuentas.Application.Cuentas.Commands.CambiarEstadoCuenta
{
    public enum OperacionEstado
    {
        ACTIVAR,
        DESACTIVAR
    }

    public class CambiarEstadoCuentaCommand : IRequest<ResultadoCambiarEstado>
    {
        public string IdCuenta { get; set; } = string.Empty;
        public OperacionEstado Operacion { get; set; }
        public string? Motivo { get; set; }
    }

    public class ResultadoCambiarEstado
    {
        public Cuenta? Cuenta { get; set; }
        public string? CodigoError { get; set; }
        public string? MensajeError { get; set; }

        public bool EsExitoso => CodigoError is null;
    }
}