using Backend.Application.DTO;
using Backend.Common.Core.Wrapper;
using Backend.Infrastructure.Persistence.Repository.Adjunto;
using MediatR;

namespace Backend.Application.Features.Adjuntos.Query
{
    public class AdjuntosQuery : IRequest<Response<IEnumerable<AdjuntosDTO>>>
    {
        public int? Idadjuntos { get; set; }
        public string? NombreArchivo { get; set; }
        public string? Extension { get; set; }
        public string? MimeType { get; set; }
        public int? TamanioBytes { get; set; }
        public string? Ruta { get; set; }
    }

    public class AdjuntosQueryHandler : IRequestHandler<AdjuntosQuery, Response<IEnumerable<AdjuntosDTO>>>
    {
        private readonly AdjuntosRepository _repository;
        public AdjuntosQueryHandler(AdjuntosRepository repository)
        {
            _repository = repository;
        }
        public async Task<Response<IEnumerable<AdjuntosDTO>>> Handle(AdjuntosQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetAdjuntos(request, cancellationToken);
        }
    }
}
