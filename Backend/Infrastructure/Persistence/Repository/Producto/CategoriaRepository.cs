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
    public class CategoriaRepository : IRepository
    {
        private readonly gestioninventariosContext _context;
        public CategoriaRepository(gestioninventariosContext context)
        {
            _context = context;
        }

        public async Task<Response<IEnumerable<CategoriasDTO>>> GetCategoriasAsync(CategoriaQuery query, CancellationToken cancellationToken)
        {
            return await ExceptionHandler.HandleExceptionAsync<IEnumerable<CategoriasDTO>>(async () =>
            {
                var categorias = _context.Categorias.AsQueryable();
                if (query.IdCategoria.HasValue)
                    categorias = categorias.Where(x => x.IdCategoria == query.IdCategoria.Value);
                if (!string.IsNullOrEmpty(query.Nombre))
                    categorias = categorias.Where(x => x.Nombre.Contains(query.Nombre));
                if (!string.IsNullOrEmpty(query.Descripcion))
                    categorias = categorias.Where(x => x.Descripcion.Contains(query.Descripcion));
                if (query.Activo.HasValue)
                    categorias = categorias.Where(x => x.Activo == query.Activo.Value);
                if (query.FechaCreacion.HasValue)
                    categorias = categorias.Where(x => x.FechaCreacion.Date == query.FechaCreacion.Value.Date);
                if (query.FechaActualizacion.HasValue)
                    categorias = categorias.Where(x => x.FechaActualizacion.HasValue &&
                                                        x.FechaActualizacion.Value.Date == query.FechaActualizacion.Value.Date);
                // Obtener las categorias por medio de la consulta que se hizo en el query
                var result = await categorias.Select(x => new CategoriasDTO
                {
                    IdCategoria = x.IdCategoria,
                    Nombre = x.Nombre,
                    Descripcion = x.Descripcion,
                    Activo = x.Activo,
                    FechaCreacion = x.FechaCreacion,
                    FechaActualizacion = x.FechaActualizacion
                }).ToListAsync(cancellationToken);
                return result;
            },Mensaje.GetMessage(true,"obtenido"));
        }

        public async Task<Response<CategoriasDTO>> SaveCategoriaAsync(CategoriaCommand request, CancellationToken cancellationToken)
        {
            return await ExceptionHandler.HandleExceptionAsync<CategoriasDTO>(async () =>
            {
                Categorias categoria;
                if(request.IdCategoria.HasValue)
                {
                    categoria = await _context.Categorias.FirstOrDefaultAsync(x => x.IdCategoria == request.IdCategoria.Value)
                    ?? throw new InvalidOperationException($"Categoria con Id {request.IdCategoria} no encontrada.");
                    categoria.Nombre = request.Nombre;
                    categoria.Descripcion = request.Descripcion;
                    categoria.Activo = request.Activo ?? true;
                    categoria.FechaActualizacion = DateTime.Now;
                    _context.Categorias.Update(categoria);
                }
                else
                {
                    categoria = new Categorias
                    {
                        Nombre = request.Nombre,
                        Descripcion = request.Descripcion,
                        Activo = request.Activo ?? true,
                        FechaCreacion = DateTime.Now,
                    };
                    _context.Categorias.Add(categoria); 
                }
                    await _context.SaveChangesAsync(cancellationToken);
                return new CategoriasDTO
                {
                    IdCategoria = categoria.IdCategoria,
                    Nombre = categoria.Nombre,
                    Descripcion = categoria.Descripcion,
                    Activo = categoria.Activo,
                    FechaCreacion = categoria.FechaCreacion,
                    FechaActualizacion = categoria.FechaActualizacion
                };
            }, Mensaje.GetMessage(true, request.IdCategoria.HasValue ? "actualizado" : "creado"));
        }

        public async Task<Response<bool>> DeleteCategoriaAsync(DeleteCategoriaCommand request, CancellationToken cancellationToken)
        {
            return await ExceptionHandler.HandleExceptionAsync<bool>(async () =>
            {
                var categoria = await _context.Categorias.FirstOrDefaultAsync(x => x.IdCategoria == request.IdCategoria, cancellationToken)
                    ?? throw new InvalidOperationException($"Categoria con Id {request.IdCategoria} no encontrada.");
                categoria.Activo = false;
                _context.Categorias.Update(categoria);
                return await _context.SaveChangesAsync(cancellationToken) > 0;
            }, Mensaje.GetMessage(true, "eliminado"));
        }
    }
}
