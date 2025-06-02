using Backend.Application.DTO;
using Backend.Common.Core.Wrapper;
using Backend.Infrastructure.Persistence.Repository.Producto;
using MediatR;

namespace Backend.Application.Features.Productos.Query
{
    public class CategoriaQuery : IRequest<Response<IEnumerable<CategoriasDTO>>>
    {
        public int? IdCategoria { get; set; }
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        public bool? Activo { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }
    }

    public class CategoriaQueryHandler : IRequestHandler<CategoriaQuery, Response<IEnumerable<CategoriasDTO>>>
    {
        private readonly CategoriaRepository _repository;
        public CategoriaQueryHandler(CategoriaRepository repository)
        {
            _repository = repository;
        }
        public async Task<Response<IEnumerable<CategoriasDTO>>> Handle(CategoriaQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetCategoriasAsync(request, cancellationToken);
        }
    }
}
