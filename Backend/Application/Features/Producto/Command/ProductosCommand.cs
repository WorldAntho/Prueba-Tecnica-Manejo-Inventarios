using Backend.Application.DTO;
using Backend.Common.Core.Wrapper;
using Backend.Infrastructure.Persistence.Repository.Producto;
using MediatR;

namespace Backend.Application.Features.Productos.Command
{
    public class ProductosCommand : IRequest<Response<ProductosDTO>>
    {
        public int? IdProducto { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int IdCategoria { get; set; }
        public int Idadjuntos { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public bool? Activo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }
    }

    public class ProductosCommandHandler : IRequestHandler<ProductosCommand, Response<ProductosDTO>>
    {
        private readonly ProductosRepository _repository;
        public ProductosCommandHandler(ProductosRepository repository)
        {
            _repository = repository;
        }
        public async Task<Response<ProductosDTO>> Handle(ProductosCommand request, CancellationToken cancellationToken)
        {
            return await _repository.SaveProductoAsync(request, cancellationToken);
        }
    }

    public class DeleteProductoCommand : IRequest<Response<bool>>
    {
        public int IdProducto { get; set; }
    }

    public class DeleteProductoCommandHandler : IRequestHandler<DeleteProductoCommand, Response<bool>>
    {
        private readonly ProductosRepository _repository;
        public DeleteProductoCommandHandler(ProductosRepository repository)
        {
            _repository = repository;
        }
        public async Task<Response<bool>> Handle(DeleteProductoCommand request, CancellationToken cancellationToken)
        {
            return await _repository.DeleteProductoAsync(request, cancellationToken);
        }
    }
}
