using Backend.Application.DTO;
using Backend.Common.Core.Wrapper;
using Backend.Infrastructure.Persistence.Repository.HistorialStock;
using MediatR;

namespace Backend.Application.Features.HistorialStock.Query
{
    public class HistorialStockQuery : IRequest<Response<IEnumerable<HistorialStockDTO>>>   
    {
        public int? IdHistorial { get; set; }
        public int? IdProducto { get; set; }
        public int? StockAnterior { get; set; }
        public int? StockNuevo { get; set; }
        public int? Diferencia { get; set; }
        public int? IdTransaccion { get; set; }
        public string? Motivo { get; set; }
        public DateTime? Fecha { get; set; }
    }

    public class HistorialStockQueryHandler : IRequestHandler<HistorialStockQuery, Response<IEnumerable<HistorialStockDTO>>>
    {
        private readonly HistorialStockRepository _repository;
        public HistorialStockQueryHandler(HistorialStockRepository repository)
        {
            _repository = repository;
        }
        public async Task<Response<IEnumerable<HistorialStockDTO>>> Handle(HistorialStockQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetHistorialStockAsync(request, cancellationToken);
        }
    }
}
