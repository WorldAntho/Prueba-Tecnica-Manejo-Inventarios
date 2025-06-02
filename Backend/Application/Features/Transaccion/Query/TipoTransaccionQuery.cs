using Backend.Application.DTO;
using Backend.Common.Core.Wrapper;
using Backend.Infrastructure.Persistence.Repository.Transaccion;
using MediatR;

namespace Backend.Application.Features.Transaccion.Query
{
    public class TipoTransaccionQuery : IRequest<Response<IEnumerable<TipoTransaccionDTO>>>
    {
        public int? IdTipoTransaccion { get; set; }
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        public bool? Activo { get; set; }
    }

    public class TipoTransaccionQueryHandler : IRequestHandler<TipoTransaccionQuery, Response<IEnumerable<TipoTransaccionDTO>>>
    {
        private readonly TipoTransaccionesRepository _repository;
        public TipoTransaccionQueryHandler(TipoTransaccionesRepository repository)
        {
            _repository = repository;
        }
        public async Task<Response<IEnumerable<TipoTransaccionDTO>>> Handle(TipoTransaccionQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetTipoTransacciones(request, cancellationToken);
        }
    }
}
