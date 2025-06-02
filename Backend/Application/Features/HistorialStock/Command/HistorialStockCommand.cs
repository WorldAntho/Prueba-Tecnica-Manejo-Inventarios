using Backend.Application.DTO;
using Backend.Common.Core.Wrapper;
using Backend.Infrastructure.Persistence.Repository.HistorialStock;
using MediatR;

namespace Backend.Application.Features.HistorialStock.Command
{
    public class HistorialStockCommand : IRequest<Response<HistorialStockDTO>>
    {
        public int? IdHistorial { get; set; }
        public int? IdProducto { get; set; }
        public int StockAnterior { get; set; }
        public int StockNuevo { get; set; }
        public int? Diferencia { get; set; }
        public int? IdTransaccion { get; set; }
        public string Motivo { get; set; }
        public DateTime Fecha { get; set; }
    }

    public class HistorialStockCommandHandler : IRequestHandler<HistorialStockCommand, Response<HistorialStockDTO>>
    {
        private readonly HistorialStockRepository _repository;
        public HistorialStockCommandHandler(HistorialStockRepository repository)
        {
            _repository = repository;
        }
        public async Task<Response<HistorialStockDTO>> Handle(HistorialStockCommand request, CancellationToken cancellationToken)
        {
            return await _repository.SaveHistorialStockAsync(request, cancellationToken);
        }
    }

    public class DeleteHistorialStockCommand : IRequest<Response<bool>>
    {
        public int IdHistorial { get; set; }
    }

    public class DeleteHistorialStockCommandHandler : IRequestHandler<DeleteHistorialStockCommand, Response<bool>>
    {
        private readonly HistorialStockRepository _repository;
        public DeleteHistorialStockCommandHandler(HistorialStockRepository repository)
        {
            _repository = repository;
        }
        public async Task<Response<bool>> Handle(DeleteHistorialStockCommand request, CancellationToken cancellationToken)
        {
            return await _repository.DeleteHistorialStockAsync(request, cancellationToken);
        }
    }
}
