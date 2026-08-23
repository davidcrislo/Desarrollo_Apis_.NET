namespace ApiCuentas.Domain.Entities
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
}