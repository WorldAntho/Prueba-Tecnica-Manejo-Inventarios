using Backend.Application.DTO;
using Backend.Application.Features.Transacciones.Command;
using Backend.Application.Features.Transacciones.Query;
using Backend.Common.Core.Helpers;
using Backend.Common.Core.Persistence;
using Backend.Common.Core.Wrapper;
using Backend.Domain;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Persistence.Repository.Transaccion
{
    public class TransaccionesRepository : IRepository
    {
        private readonly gestioninventariosContext _context;

        public TransaccionesRepository(gestioninventariosContext context)
        {
            _context = context;
        }

        public async Task<Response<IEnumerable<TransaccionesDTO>>> GetTransacciones(TransaccionesQuery query, CancellationToken cancellationToken)
        {
            return await ExceptionHandler.HandleExceptionAsync<IEnumerable<TransaccionesDTO>>(async () =>
            {
                var transaccion = _context.Transacciones.AsQueryable();
                // Filtros para la busqueda de las transacciones
                if (query.IdTransaccion.HasValue)
                    transaccion = transaccion.Where(x => x.IdTransaccion == query.IdTransaccion.Value);
                if (query.Fecha.HasValue)
                    transaccion = transaccion.Where(x => x.Fecha.Date == query.Fecha.Value.Date);
                if (query.IdTipoTransaccion.HasValue)
                    transaccion = transaccion.Where(x => x.IdTipoTransaccion == query.IdTipoTransaccion.Value);
                if (query.IdProducto.HasValue)
                    transaccion = transaccion.Where(x => x.IdProducto == query.IdProducto.Value);
                if (query.Cantidad.HasValue)
                    transaccion = transaccion.Where(x => x.Cantidad == query.Cantidad.Value);
                if (query.PrecioUnitario.HasValue)
                    transaccion = transaccion.Where(x => x.PrecioUnitario == query.PrecioUnitario.Value);
                if (!string.IsNullOrEmpty(query.Detalle))
                    transaccion = transaccion.Where(x => x.Detalle.Contains(query.Detalle));
                if (!string.IsNullOrEmpty(query.NumeroDocumento))
                    transaccion = transaccion.Where(x => x.NumeroDocumento.Contains(query.NumeroDocumento));
                if (query.Activo.HasValue)
                    transaccion = transaccion.Where(x => x.Activo == query.Activo.Value);
                if (query.FechaCreacion.HasValue)
                    transaccion = transaccion.Where(x => x.FechaCreacion.Date == query.FechaCreacion.Value.Date);
                //Obtener las transacciones por medio de la consulta que se hizo en el query
                var result = await transaccion.Select(x => new TransaccionesDTO
                {
                    IdTransaccion = x.IdTransaccion,
                    Fecha = x.Fecha,
                    IdTipoTransaccion = x.IdTipoTransaccion,
                    IdProducto = x.IdProducto,
                    Cantidad = x.Cantidad,
                    PrecioUnitario = x.PrecioUnitario,
                    PrecioTotal = x.PrecioTotal,
                    Detalle = x.Detalle,
                    NumeroDocumento = x.NumeroDocumento,
                    Activo = x.Activo,
                    FechaCreacion = x.FechaCreacion
                }).ToListAsync(cancellationToken);
                return result;
            }, Mensaje.GetMessage(true, "obtenido"));
        }

        public async Task<Response<TransaccionesDTO>> SaveTransaccionAsync(TransaccionesCommand request, CancellationToken cancellationToken)
        {
            return await ExceptionHandler.HandleExceptionAsync(async () =>
            {
                if (request == null)
                    throw new ArgumentNullException(nameof(request), "La solicitud de transacción no puede ser nula");

                if (request.Cantidad <= 0)
                    throw new ArgumentException("La cantidad debe ser mayor que cero", nameof(request.Cantidad));

                if (request.PrecioUnitario <= 0)
                    throw new ArgumentException("El precio unitario debe ser mayor que cero", nameof(request.PrecioUnitario));

                var tipoTransaccion = await _context.Tipostransaccion
                    .FirstOrDefaultAsync(t => t.IdTipoTransaccion == request.IdTipoTransaccion, cancellationToken)
                    ?? throw new InvalidOperationException($"Tipo de transacción con ID {request.IdTipoTransaccion} no encontrado");
                
                var producto = await _context.Productos
                    .FirstOrDefaultAsync(p => p.IdProducto == request.IdProducto, cancellationToken)
                    ?? throw new InvalidOperationException($"Producto con ID {request.IdProducto} no encontrado");

                if (tipoTransaccion.Nombre.Equals("Venta", StringComparison.OrdinalIgnoreCase))
                {
                    if (producto.Stock < request.Cantidad)
                    {
                        throw new InvalidOperationException(
                            $"Stock insuficiente para el producto {producto.Nombre}. " +
                            $"Stock disponible: {producto.Stock}, Cantidad solicitada: {request.Cantidad}");
                    }
                }

                decimal precioTotal = request.Cantidad * request.PrecioUnitario;

                var transaccion = new Transacciones
                {
                    Fecha = request.Fecha,
                    IdTipoTransaccion = request.IdTipoTransaccion,
                    IdProducto = request.IdProducto,
                    Cantidad = request.Cantidad,
                    PrecioUnitario = request.PrecioUnitario,
                    PrecioTotal = precioTotal,
                    Detalle = request.Detalle,
                    NumeroDocumento = request.NumeroDocumento,
                    Activo = true,
                    FechaCreacion = DateTime.Now
                };

                await AjustarStock(producto, tipoTransaccion, request.Cantidad);

                _context.Transacciones.Add(transaccion);
                await _context.SaveChangesAsync(cancellationToken);

                return new TransaccionesDTO
                {
                    IdTransaccion = transaccion.IdTransaccion,
                    Fecha = transaccion.Fecha,
                    IdTipoTransaccion = transaccion.IdTipoTransaccion,
                    IdProducto = transaccion.IdProducto,
                    Cantidad = transaccion.Cantidad,
                    PrecioUnitario = transaccion.PrecioUnitario,
                    PrecioTotal = transaccion.PrecioTotal,
                    Detalle = transaccion.Detalle,
                    NumeroDocumento = transaccion.NumeroDocumento,
                    Activo = transaccion.Activo,
                    FechaCreacion = transaccion.FechaCreacion
                };
            }, Mensaje.GetMessage(true, request.IdTransaccion.HasValue ? "actualizado" : "creado"));
        }

        private async Task AjustarStock(Productos producto, Tipostransaccion tipoTransaccion, int cantidad)
        {
            int ajuste = tipoTransaccion.Nombre.Equals("Venta", StringComparison.OrdinalIgnoreCase)
                ? -cantidad 
                : cantidad;

            if (producto.Stock + ajuste < 0)
            {
                throw new InvalidOperationException(
                    $"La operación resultaría en stock negativo para el producto {producto.Nombre}");
            }

            producto.Stock += ajuste;
            producto.FechaActualizacion = DateTime.Now;

            _context.Productos.Update(producto);
        }
        public async Task<Response<bool>> DeleteTransaccionAsync(DeleteTransaccionCommand request, CancellationToken cancellationToken)
        {
            return await ExceptionHandler.HandleExceptionAsync(async () =>
            {
                var transaccion = await _context.Transacciones.FirstOrDefaultAsync(x => x.IdTransaccion == request.IdTransaccion, cancellationToken)
                   ?? throw new InvalidOperationException($"No se encontró la transacción con Id {request.IdTransaccion}");
                transaccion.Activo = false;
                _context.Transacciones.Update(transaccion);
                return await _context.SaveChangesAsync(cancellationToken) > 0;
            }, Mensaje.GetMessage(true, "eliminado"));
        }
    }
}
