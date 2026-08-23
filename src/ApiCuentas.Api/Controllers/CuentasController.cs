using Microsoft.AspNetCore.Mvc;
using MediatR;
using ApiCuentas.Application.Cuentas.Commands.CrearCuenta;
using ApiCuentas.Application.Cuentas.Commands.CambiarEstadoCuenta;
using ApiCuentas.Application.Cuentas.Queries.ListarCuentas;
using ApiCuentas.Application.Cuentas.Queries.ObtenerCuentaPorId;
using ApiCuentas.Domain.Entities;

namespace ApiCuentas.Api.Controllers
{
    [ApiController]
    [Route("api/v1/cuentas")]
    public class CuentasController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CuentasController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cuenta>>> Listar([FromQuery] EstadoCuenta? estado)
        {
            var resultado = await _mediator.Send(new ListarCuentasQuery(estado));
            return Ok(resultado);
        }

        [HttpGet("{idCuenta}")]
        public async Task<ActionResult<Cuenta>> ObtenerPorId(string idCuenta)
        {
            var cuenta = await _mediator.Send(new ApiCuentas.Application.Cuentas.Queries.ObtenerCuentaPorId.ObtenerCuentaPorIdQuery(idCuenta));
            if (cuenta is null)
            {
                return NotFound(new { Codigo = "CUENTA_NO_ENCONTRADA", Mensaje = $"No existe una cuenta con id {idCuenta}" });
            }
            return Ok(cuenta);
        }

        [HttpPost]
        public async Task<ActionResult<Cuenta>> Crear([FromBody] CrearCuentaCommand command)
        {
            var resultado = await _mediator.Send(command);
            if (!resultado.EsExitoso)
            {
                return BadRequest(new { Codigo = resultado.CodigoError, Mensaje = resultado.MensajeError });
            }
            return CreatedAtAction(nameof(ObtenerPorId), new { idCuenta = resultado.Cuenta!.IdCuenta }, resultado.Cuenta);
        }

        [HttpPut("{idCuenta}/estado")]
        public async Task<ActionResult<Cuenta>> CambiarEstado(string idCuenta, [FromBody] CambiarEstadoCuentaCommand command)
        {
            command.IdCuenta = idCuenta;
            var resultado = await _mediator.Send(command);
            if (!resultado.EsExitoso)
            {
                return NotFound(new { Codigo = resultado.CodigoError, Mensaje = resultado.MensajeError });
            }
            return Ok(resultado.Cuenta);
        }
    }
}