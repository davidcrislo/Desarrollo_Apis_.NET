namespace ApiCuentas.Models
{
    public enum EstadoCuenta
    {
        ACTIVA,
        INACTIVA,
        BLOQUEADA,
        CERRADA
    }

    public class Cuenta
    {
        public string IdCuenta { get; set; } = string.Empty;
        public string NumeroCuenta { get; set; } = string.Empty;
        public string Titular { get; set; } = string.Empty;
        public string TipoCuenta { get; set; } = string.Empty;
        public EstadoCuenta Estado { get; set; } = EstadoCuenta.ACTIVA;
        public decimal Saldo { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public DateTime FechaActualizacion { get; set; } = DateTime.UtcNow;
    }

    public class CuentaCreacion
    {
        public string NumeroCuenta { get; set; } = string.Empty;
        public string Titular { get; set; } = string.Empty;
        public string TipoCuenta { get; set; } = string.Empty;
    }

    public enum OperacionEstado
    {
        ACTIVAR,
        DESACTIVAR
    }

    public class CambioEstadoCuenta
    {
        public OperacionEstado Operacion { get; set; }
        public string? Motivo { get; set; }
    }

    public class ErrorRespuesta
    {
        public string Codigo { get; set; } = string.Empty;
        public string Mensaje { get; set; } = string.Empty;
    }
}