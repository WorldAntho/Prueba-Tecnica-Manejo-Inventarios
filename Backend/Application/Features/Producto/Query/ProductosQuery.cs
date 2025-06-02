using Backend.Application.DTO;
using Backend.Common.Core.Wrapper;
using Backend.Infrastructure.Persistence.Repository.Producto;
using MediatR;

namespace Backend.Application.Features.Productos.Query
{
    public class ProductosQuery : IRequest<Response<IEnumerable<ProductosDTO>>>
    {
        public int? IdProducto { get; set; }
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        public int? IdCategoria { get; set; }
        public int? Idadjuntos { get; set; }
        public decimal? Precio { get; set; }
        public int? Stock { get; set; }
        public bool? Activo { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }
    }

    public class ProductosQueryHandler : IRequestHandler<ProductosQuery, Response<IEnumerable<ProductosDTO>>>
    {
        private readonly ProductosRepository _repository;
        public ProductosQueryHandler(ProductosRepository repository)
        {
            _repository = repository;
        }
        public async Task<Response<IEnumerable<ProductosDTO>>> Handle(ProductosQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetProductos(request, cancellationToken);
        }
    }
}
