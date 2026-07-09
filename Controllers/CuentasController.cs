using Microsoft.AspNetCore.Mvc;
using ApiCuentas.Models;

namespace ApiCuentas.Controllers
{
    [ApiController]
    [Route("api/v1/cuentas")]
    public class CuentasController : ControllerBase
    {
        // Almacenamiento en memoria solo para fines didácticos del bootcamp.
        // En un proyecto real esto sería un servicio + repositorio + base de datos.
        private static readonly List<Cuenta> _cuentas = new();

        [HttpGet]
        public ActionResult<IEnumerable<Cuenta>> Listar([FromQuery] EstadoCuenta? estado)
        {
            var resultado = estado is null
                ? _cuentas
                : _cuentas.Where(c => c.Estado == estado).ToList();

            return Ok(resultado);
        }

        [HttpGet("{idCuenta}")]
        public ActionResult<Cuenta> ObtenerPorId(string idCuenta)
        {
            var cuenta = _cuentas.FirstOrDefault(c => c.IdCuenta == idCuenta);
            if (cuenta is null)
            {
                return NotFound(new ErrorRespuesta
                {
                    Codigo = "CUENTA_NO_ENCONTRADA",
                    Mensaje = $"No existe una cuenta con id {idCuenta}"
                });
            }

            return Ok(cuenta);
        }

        [HttpPost]
        public ActionResult<Cuenta> Crear([FromBody] CuentaCreacion datos)
        {
            if (string.IsNullOrWhiteSpace(datos.NumeroCuenta) ||
                string.IsNullOrWhiteSpace(datos.Titular))
            {
                return BadRequest(new ErrorRespuesta
                {
                    Codigo = "DATOS_INVALIDOS",
                    Mensaje = "NumeroCuenta y Titular son obligatorios"
                });
            }

            var nuevaCuenta = new Cuenta
            {
                IdCuenta = Guid.NewGuid().ToString(),
                NumeroCuenta = datos.NumeroCuenta,
                Titular = datos.Titular,
                TipoCuenta = datos.TipoCuenta,
                Estado = EstadoCuenta.ACTIVA
            };

            _cuentas.Add(nuevaCuenta);

            return CreatedAtAction(nameof(ObtenerPorId), new { idCuenta = nuevaCuenta.IdCuenta }, nuevaCuenta);
        }

        [HttpPut("{idCuenta}/estado")]
        public ActionResult<Cuenta> CambiarEstado(string idCuenta, [FromBody] CambioEstadoCuenta cambio)
        {
            var cuenta = _cuentas.FirstOrDefault(c => c.IdCuenta == idCuenta);
            if (cuenta is null)
            {
                return NotFound(new ErrorRespuesta
                {
                    Codigo = "CUENTA_NO_ENCONTRADA",
                    Mensaje = $"No existe una cuenta con id {idCuenta}"
                });
            }

            cuenta.Estado = cambio.Operacion == OperacionEstado.ACTIVAR
                ? EstadoCuenta.ACTIVA
                : EstadoCuenta.INACTIVA;

            cuenta.FechaActualizacion = DateTime.UtcNow;

            return Ok(cuenta);
        }
    }
}