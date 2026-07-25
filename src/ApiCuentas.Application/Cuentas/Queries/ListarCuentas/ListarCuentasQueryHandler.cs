using ApiCuentas.Application.Interfaces;
using ApiCuentas.Domain.Entities;
using MediatR;

namespace ApiCuentas.Application.Cuentas.Queries.ListarCuentas
{
    public class ListarCuentasQueryHandler : IRequestHandler<ListarCuentasQuery, IEnumerable<Cuenta>>
    {
        private readonly ICuentaRepository _repositorio;

        public ListarCuentasQueryHandler(ICuentaRepository repositorio)
        {
            _repositorio = repositorio;
        }

        public Task<IEnumerable<Cuenta>> Handle(ListarCuentasQuery request, CancellationToken cancellationToken)
        {
            var resultado = _repositorio.Listar(request.Estado);
            return Task.FromResult(resultado);
        }
    }
}