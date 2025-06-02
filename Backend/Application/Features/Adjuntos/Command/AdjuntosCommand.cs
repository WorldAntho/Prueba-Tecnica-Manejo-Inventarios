using Backend.Application.DTO;
using Backend.Common.Core.Wrapper;
using Backend.Infrastructure.Persistence.Repository.Adjunto;
using MediatR;

namespace Backend.Application.Features.Adjuntos.Command
{
    public class AdjuntosCommand : IRequest<Response<AdjuntosDTO>>
    {
        public int? Idadjuntos { get; set; }
        public required IFormFile Archivo { get; set; }
    }

    public class AdjuntosCommandHandler : IRequestHandler<AdjuntosCommand, Response<AdjuntosDTO>>
    {
        private readonly AdjuntosRepository _repository;

        public AdjuntosCommandHandler(AdjuntosRepository repository)
        {
            _repository = repository;
        }

        public async Task<Response<AdjuntosDTO>> Handle(AdjuntosCommand request, CancellationToken cancellationToken)
        {
            return await _repository.SaveAdjuntoAsync(request, cancellationToken);
        }

        public class DeleteAdjuntoCommand : IRequest<Response<bool>>
        {
            public int Idadjuntos { get; set; }
        }

        public class DeleteAdjuntoCommandHandler : IRequestHandler<DeleteAdjuntoCommand, Response<bool>>
        {
            private readonly AdjuntosRepository _repository;
            public DeleteAdjuntoCommandHandler(AdjuntosRepository repository)
            {
                _repository = repository;
            }
            public async Task<Response<bool>> Handle(DeleteAdjuntoCommand request, CancellationToken cancellationToken)
            {
                return await _repository.DeleteAdjuntoAsync(request, cancellationToken);
            }
        }
    }
}
