using Backend.Application.DTO;
using Backend.Common.Core.Wrapper;
using Backend.Infrastructure.Persistence.Repository.Transaccion;
using MediatR;

namespace Backend.Application.Features.Transacciones.Query
{
    public class TransaccionesQuery : IRequest<Response<IEnumerable<TransaccionesDTO>>>
    {
        public int? IdTransaccion { get; set; }
        public DateTime? Fecha { get; set; }
        public int? IdTipoTransaccion { get; set; }
        public int? IdProducto { get; set; }
        public int? Cantidad { get; set; }
        public decimal? PrecioUnitario { get; set; }
        public decimal? PrecioTotal { get; set; }
        public string? Detalle { get; set; }
        public string? NumeroDocumento { get; set; }
        public bool? Activo { get; set; }
        public DateTime? FechaCreacion { get; set; }
    }

    public class TransaccionesQueryHandler : IRequestHandler<TransaccionesQuery, Response<IEnumerable<TransaccionesDTO>>>
    {
        private readonly TransaccionesRepository _repository;

        public TransaccionesQueryHandler(TransaccionesRepository repository)
        {
            _repository = repository;
        }

        public async Task<Response<IEnumerable<TransaccionesDTO>>> Handle(TransaccionesQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetTransacciones(request, cancellationToken);
        }
    }

}
