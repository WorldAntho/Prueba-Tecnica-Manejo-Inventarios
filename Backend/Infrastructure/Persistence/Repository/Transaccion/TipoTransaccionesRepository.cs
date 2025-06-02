using Backend.Application.DTO;
using Backend.Application.Features.Transaccion.Command;
using Backend.Application.Features.Transaccion.Query;
using Backend.Common.Core.Helpers;
using Backend.Common.Core.Persistence;
using Backend.Common.Core.Wrapper;
using Backend.Domain;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Persistence.Repository.Transaccion
{
    public class TipoTransaccionesRepository : IRepository
    {
        private readonly gestioninventariosContext _context;

        public TipoTransaccionesRepository(gestioninventariosContext context)
        {
            _context = context;
        }

        public async Task<Response<IEnumerable<TipoTransaccionDTO>>> GetTipoTransacciones(TipoTransaccionQuery query, CancellationToken cancellationToken)
        {
            return await ExceptionHandler.HandleExceptionAsync<IEnumerable<TipoTransaccionDTO>>(async () =>
            {
                var tipoTransaccion = _context.Tipostransaccion.AsQueryable();
                // Filtros para la busqueda de los tipos de transacciones
                if (query.IdTipoTransaccion.HasValue)
                    tipoTransaccion = tipoTransaccion.Where(x => x.IdTipoTransaccion == query.IdTipoTransaccion.Value);
                if (!string.IsNullOrEmpty(query.Nombre))
                    tipoTransaccion = tipoTransaccion.Where(x => x.Nombre.Contains(query.Nombre));
                if (!string.IsNullOrEmpty(query.Descripcion))
                    tipoTransaccion = tipoTransaccion.Where(x => x.Descripcion.Contains(query.Descripcion));
                if (query.Activo.HasValue)
                    tipoTransaccion = tipoTransaccion.Where(x => x.Activo == query.Activo.Value);
                // Obtener los tipos de transacciones por medio de la consulta que se hizo en el query
                var result = await tipoTransaccion.Select(x => new TipoTransaccionDTO
                {
                    IdTipoTransaccion = x.IdTipoTransaccion,
                    Nombre = x.Nombre,
                    Descripcion = x.Descripcion,
                    Activo = x.Activo
                }).ToListAsync(cancellationToken);
                return result;
            },Mensaje.GetMessage(true,"obtenido"));
        }


        public async Task<Response<TipoTransaccionDTO>> SaveTipoTransaccionAsync(TipoTransaccionCommand request, CancellationToken cancellationToken)
        {
            return await ExceptionHandler.HandleExceptionAsync(async () =>
            {
                Tipostransaccion tipostransaccion;
                if (request.IdTipoTransaccion.HasValue)
                {
                    tipostransaccion = _context.Tipostransaccion.FirstOrDefault(x => x.IdTipoTransaccion == request.IdTipoTransaccion.Value)
                        ?? throw new InvalidOperationException($"Tipo de transacción con ID {request.IdTipoTransaccion} no encontrado");    
                    tipostransaccion.Nombre = request.Nombre;
                    tipostransaccion.Descripcion = request.Descripcion;
                    tipostransaccion.Activo = request.Activo ?? true;
                    _context.Tipostransaccion.Update(tipostransaccion);
                }
                else
                {
                    tipostransaccion = new Tipostransaccion
                    {
                        Nombre = request.Nombre,
                        Descripcion = request.Descripcion,
                        Activo = request.Activo ?? true
                    };
                    _context.Tipostransaccion.Add(tipostransaccion);
                }
                await _context.SaveChangesAsync(cancellationToken);
                return new TipoTransaccionDTO
                {
                    IdTipoTransaccion = tipostransaccion.IdTipoTransaccion,
                    Nombre = tipostransaccion.Nombre,
                    Descripcion = tipostransaccion.Descripcion,
                    Activo = tipostransaccion.Activo
                };
            }, Mensaje.GetMessage(true, request.IdTipoTransaccion.HasValue ? "actualizado" : "creado"));
        }

        public async Task<Response<bool>> DeleteTipoTransaccionAsync(DeleteTipoTransaccionCommand request, CancellationToken cancellationToken)
        {
            return await ExceptionHandler.HandleExceptionAsync<bool>(async () =>
            {
                var tipostransaccion = await _context.Tipostransaccion
                    .FirstOrDefaultAsync(x => x.IdTipoTransaccion == request.IdTipoTransaccion, cancellationToken)
                    ?? throw new InvalidOperationException($"Tipo de transacción con ID {request.IdTipoTransaccion} no encontrado");
                tipostransaccion.Activo = false;
                _context.Tipostransaccion.Update(tipostransaccion);
                return await _context.SaveChangesAsync(cancellationToken) > 0;
            }, Mensaje.GetMessage(true, "eliminado"));
        }
    }
}
