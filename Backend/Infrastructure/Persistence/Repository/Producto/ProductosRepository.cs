using Backend.Application.DTO;
using Backend.Application.Features.Productos.Command;
using Backend.Application.Features.Productos.Query;
using Backend.Common.Core.Helpers;
using Backend.Common.Core.Persistence;
using Backend.Common.Core.Wrapper;
using Backend.Domain;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Persistence.Repository.Producto
{
    public class ProductosRepository : IRepository
    {
        private readonly gestioninventariosContext _context;
        public ProductosRepository(gestioninventariosContext context)
        {
            _context = context;
        }

        public async Task<Response<IEnumerable<ProductosDTO>>> GetProductos(ProductosQuery query, CancellationToken cancellationToken)
        {
            return await ExceptionHandler.HandleExceptionAsync<IEnumerable<ProductosDTO>>(async () =>
            {
                var productos = _context.Productos.AsQueryable();
                // Filtros para la busqueda de los productos
                if (query.IdProducto.HasValue)
                    productos = productos.Where(x => x.IdProducto == query.IdProducto.Value);
                if (!string.IsNullOrEmpty(query.Nombre))
                    productos = productos.Where(x => x.Nombre.Contains(query.Nombre));
                if (!string.IsNullOrEmpty(query.Descripcion))
                    productos = productos.Where(x => x.Descripcion.Contains(query.Descripcion));
                if (query.IdCategoria.HasValue)
                    productos = productos.Where(x => x.IdCategoria == query.IdCategoria.Value);
                if (query.Idadjuntos.HasValue)
                    productos = productos.Where(x => x.Idadjuntos == query.Idadjuntos.Value);
                if (query.Precio.HasValue)
                    productos = productos.Where(x => x.Precio == query.Precio.Value);
                if (query.Stock.HasValue)
                    productos = productos.Where(x => x.Stock == query.Stock.Value);
                if (query.Activo.HasValue)
                    productos = productos.Where(x => x.Activo == query.Activo.Value);
                if (query.FechaCreacion.HasValue)
                    productos = productos.Where(x => x.FechaCreacion.Date == query.FechaCreacion.Value.Date);
                if (query.FechaActualizacion.HasValue)
                    productos = productos.Where(x => x.FechaActualizacion.HasValue &&
                                                      x.FechaActualizacion.Value.Date == query.FechaActualizacion.Value.Date);
                // Obtener los productos por medio de la consulta que se hizo en el query
                var result = await productos.Select(x => new ProductosDTO
                {
                    IdProducto = x.IdProducto,
                    Nombre = x.Nombre,
                    Descripcion = x.Descripcion,
                    IdCategoria = x.IdCategoria,
                    Idadjuntos = x.Idadjuntos,
                    Precio = x.Precio,
                    Stock = x.Stock,
                    Activo = x.Activo,
                    FechaCreacion = x.FechaCreacion,
                    FechaActualizacion = x.FechaActualizacion
                }).ToListAsync(cancellationToken);
                return result;
            }, Mensaje.GetMessage(true, "obtenido"));
        }

        public async Task<Response<ProductosDTO>> SaveProductoAsync(ProductosCommand request, CancellationToken cancellationToken)
        {
            return await ExceptionHandler.HandleExceptionAsync(async () =>
            {
                Productos productos;
                if (request.IdProducto.HasValue)
                {
                    productos = _context.Productos.FirstOrDefault(x => x.IdProducto == request.IdProducto.Value)
                        ?? throw new InvalidOperationException($"Producto con ID {request.IdProducto} no encontrado");
                    productos.Nombre = request.Nombre;
                    productos.Descripcion = request.Descripcion;
                    productos.IdCategoria = request.IdCategoria;
                    productos.Idadjuntos = request.Idadjuntos;
                    productos.Precio = request.Precio;
                    productos.Stock = request.Stock;
                    productos.Activo = request.Activo ?? true;
                    productos.FechaActualizacion = request.FechaActualizacion ?? DateTime.UtcNow;
                    _context.Productos.Update(productos);
                }
                else
                {
                    productos = new Productos
                    {
                        Nombre = request.Nombre,
                        Descripcion = request.Descripcion,
                        IdCategoria = request.IdCategoria,
                        Idadjuntos = request.Idadjuntos,
                        Precio = request.Precio,
                        Stock = request.Stock,
                        Activo = request.Activo ?? true,
                        FechaCreacion = DateTime.UtcNow
                    };
                    _context.Productos.Add(productos);
                }
                await _context.SaveChangesAsync(cancellationToken);
                return new ProductosDTO
                {
                    IdProducto = productos.IdProducto,
                    Nombre = productos.Nombre,
                    Descripcion = productos.Descripcion,
                    IdCategoria = productos.IdCategoria,
                    Idadjuntos = productos.Idadjuntos,
                    Precio = productos.Precio,
                    Stock = productos.Stock,
                    Activo = productos.Activo,
                    FechaCreacion = productos.FechaCreacion,
                    FechaActualizacion = productos.FechaActualizacion
                };
            }, Mensaje.GetMessage(true, "guardado"));
        }

        public async Task<Response<bool>> DeleteProductoAsync(DeleteProductoCommand request, CancellationToken cancellationToken)
        {
            return await ExceptionHandler.HandleExceptionAsync(async () =>
            {
                var producto = await _context.Productos.FirstOrDefaultAsync(x => x.IdProducto == request.IdProducto)
                ?? throw new InvalidOperationException($"Producto con ID {request.IdProducto} no encontrado");
                producto.Activo = false;
                _context.Productos.Update(producto);
                return await _context.SaveChangesAsync(cancellationToken) > 0;
            }, Mensaje.GetMessage(true, "eliminado"));
        }
    }
}
