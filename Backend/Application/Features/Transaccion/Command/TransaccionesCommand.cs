using Backend.Application.DTO;
using Backend.Common.Core.Wrapper;
using Backend.Infrastructure.Persistence.Repository.Transaccion;
using MediatR;

namespace Backend.Application.Features.Transacciones.Command
{
    public class TransaccionesCommand : IRequest<Response<TransaccionesDTO>>
    {
        public int? IdTransaccion { get; set; }
        public DateTime Fecha { get; set; }
        public int IdTipoTransaccion { get; set; }
        public int IdProducto { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal? PrecioTotal { get; set; }
        public string Detalle { get; set; }
        public string NumeroDocumento { get; set; }
        public bool? Activo { get; set; }
        public DateTime FechaCreacion { get; set; }
    }

    public class TransaccionesCommandHandler : IRequestHandler<TransaccionesCommand, Response<TransaccionesDTO>>
    {
        private readonly TransaccionesRepository _repository;

        public TransaccionesCommandHandler(TransaccionesRepository repository)
        {
            _repository = repository;
        }

        public async Task<Response<TransaccionesDTO>> Handle(TransaccionesCommand request, CancellationToken cancellationToken)
        {
            return await _repository.SaveTransaccionAsync(request, cancellationToken);
        }
    }

    public class DeleteTransaccionCommand : IRequest<Response<bool>>
    {
        public int IdTransaccion { get; set; }
    }

    public class DeleteTransaccionCommandHandler : IRequestHandler<DeleteTransaccionCommand, Response<bool>>
    {
        private readonly TransaccionesRepository _repository;
        public DeleteTransaccionCommandHandler(TransaccionesRepository repository)
        {
            _repository = repository;
        }
        public async Task<Response<bool>> Handle(DeleteTransaccionCommand request, CancellationToken cancellationToken)
        {
            return await _repository.DeleteTransaccionAsync(request, cancellationToken);
        }
    }
}
