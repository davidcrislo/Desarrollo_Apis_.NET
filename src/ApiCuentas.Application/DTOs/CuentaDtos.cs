namespace ApiCuentas.Application.DTOs
{
    public class CuentaCreacionDto
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

    public class CambioEstadoDto
    {
        public OperacionEstado Operacion { get; set; }
        public string? Motivo { get; set; }
    }

    public class ErrorDto
    {
        public string Codigo { get; set; } = string.Empty;
        public string Mensaje { get; set; } = string.Empty;
    }
}