using Backend.Application.DTO;
using Backend.Common.Core.Wrapper;
using Backend.Infrastructure.Persistence.Repository.Transaccion;
using MediatR;

namespace Backend.Application.Features.Transaccion.Command
{
    public class TipoTransaccionCommand : IRequest<Response<TipoTransaccionDTO>>
    {
        public int? IdTipoTransaccion { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public bool? Activo { get; set; }
    }

    public class TipoTransaccionCommandHandler : IRequestHandler<TipoTransaccionCommand, Response<TipoTransaccionDTO>>
    {
        private readonly TipoTransaccionesRepository _repository;
        public TipoTransaccionCommandHandler(TipoTransaccionesRepository repository)
        {
            _repository = repository;
        }
        public async Task<Response<TipoTransaccionDTO>> Handle(TipoTransaccionCommand request, CancellationToken cancellationToken)
        {
            return await _repository.SaveTipoTransaccionAsync(request, cancellationToken);
        }
    }

    public class DeleteTipoTransaccionCommand : IRequest<Response<bool>>
    {
        public int IdTipoTransaccion { get; set; }
    }

    public class DeleteTipoTransaccionCommandHandler : IRequestHandler<DeleteTipoTransaccionCommand, Response<bool>>
    {
        private readonly TipoTransaccionesRepository _repository;
        public DeleteTipoTransaccionCommandHandler(TipoTransaccionesRepository repository)
        {
            _repository = repository;
        }
        public async Task<Response<bool>> Handle(DeleteTipoTransaccionCommand request, CancellationToken cancellationToken)
        {
            return await _repository.DeleteTipoTransaccionAsync(request, cancellationToken);
        }
    }
}
