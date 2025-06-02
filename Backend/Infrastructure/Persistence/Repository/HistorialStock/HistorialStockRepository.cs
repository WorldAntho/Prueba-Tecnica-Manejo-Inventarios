using Backend.Application.DTO;
using Backend.Application.Features.HistorialStock.Command;
using Backend.Application.Features.HistorialStock.Query;
using Backend.Common.Core.Helpers;
using Backend.Common.Core.Persistence;
using Backend.Common.Core.Wrapper;
using Backend.Domain;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Persistence.Repository.HistorialStock
{
    public class HistorialStockRepository : IRepository
    {
        private readonly gestioninventariosContext _context;

        public HistorialStockRepository(gestioninventariosContext context)
        {
            _context = context;
        }

        public async Task<Response<IEnumerable<HistorialStockDTO>>> GetHistorialStockAsync(HistorialStockQuery query, CancellationToken cancellationToken)
        {
            return await ExceptionHandler.HandleExceptionAsync<IEnumerable<HistorialStockDTO>>(async () =>
            {
                var historial = _context.Historialstock.AsQueryable();
                if (query.IdHistorial.HasValue)
                    historial = historial.Where(x => x.IdHistorial == query.IdHistorial.Value);
                if (query.IdProducto.HasValue)
                    historial = historial.Where(x => x.IdProducto == query.IdProducto.Value);
                if (query.StockAnterior.HasValue)
                    historial = historial.Where(x => x.StockAnterior == query.StockAnterior.Value);
                if (query.StockNuevo.HasValue)
                    historial = historial.Where(x => x.StockNuevo == query.StockNuevo.Value);
                if (query.Diferencia.HasValue)
                    historial = historial.Where(x => x.Diferencia == query.Diferencia.Value);
                if (query.IdTransaccion.HasValue)
                    historial = historial.Where(x => x.IdTransaccion == query.IdTransaccion.Value);
                if (!string.IsNullOrEmpty(query.Motivo))
                    historial = historial.Where(x => x.Motivo.Contains(query.Motivo));
                if (query.Fecha.HasValue)
                    historial = historial.Where(x => x.Fecha.Date == query.Fecha.Value.Date);
                var result = await historial.Select(x => new HistorialStockDTO
                {
                    IdHistorial = x.IdHistorial,
                    IdProducto = x.IdProducto,
                    StockAnterior = x.StockAnterior,
                    StockNuevo = x.StockNuevo,
                    Diferencia = x.Diferencia,
                    IdTransaccion = x.IdTransaccion,
                    Motivo = x.Motivo,
                    Fecha = x.Fecha
                }).ToListAsync(cancellationToken);
                return result;
            },Mensaje.GetMessage(true,"obtenido"));
        }

        public async Task<Response<HistorialStockDTO>> SaveHistorialStockAsync(HistorialStockCommand request, CancellationToken cancellationToken)
        {
            return await ExceptionHandler.HandleExceptionAsync(async () =>
            {
                Historialstock historial;
                if (request.IdHistorial.HasValue && request.IdProducto.HasValue)
                {
                    historial = await _context.Historialstock.FirstOrDefaultAsync(x => x.IdHistorial == request.IdHistorial.Value && x.IdProducto == request.IdProducto)
                        ?? throw new InvalidOperationException($"Historial with ID {request.IdHistorial} not found para el producto {request.IdProducto}");
                    historial.StockAnterior = request.StockAnterior;
                    historial.StockNuevo = request.StockNuevo;
                    historial.Diferencia = request.StockNuevo - request.StockAnterior;
                    historial.IdTransaccion = request.IdTransaccion;
                    historial.Motivo = request.Motivo;
                    historial.Fecha = request.Fecha;
                    _context.Historialstock.Update(historial);
                }
                else
                {
                    historial = new Historialstock
                    {
                        IdProducto = request.IdProducto.Value,
                        StockAnterior = request.StockAnterior,
                        StockNuevo = request.StockNuevo,
                        Diferencia = request.StockNuevo - request.StockAnterior,
                        IdTransaccion = request.IdTransaccion,
                        Motivo = request.Motivo,
                        Fecha = request.Fecha
                    };
                     _context.Historialstock.AddAsync(historial);
                }
                await _context.SaveChangesAsync(cancellationToken);
                return new HistorialStockDTO
                {
                    IdHistorial = historial.IdHistorial,
                    IdProducto = historial.IdProducto,
                    StockAnterior = historial.StockAnterior,
                    StockNuevo = historial.StockNuevo,
                    Diferencia = historial.Diferencia,
                    IdTransaccion = historial.IdTransaccion,
                    Motivo = historial.Motivo,
                    Fecha = historial.Fecha
                };
            },Mensaje.GetMessage(true, request.IdTransaccion.HasValue ? "actualizado" : "creado"));
        }

        public async Task<Response<bool>> DeleteHistorialStockAsync(DeleteHistorialStockCommand request, CancellationToken cancellationToken)
        {
            return await ExceptionHandler.HandleExceptionAsync<bool>(async () =>
            {
                var historial = await _context.Historialstock.FirstOrDefaultAsync(x => x.IdHistorial == request.IdHistorial)
                ?? throw new InvalidOperationException($"Historial with ID {request.IdHistorial} not found");
                _context.Historialstock.Remove(historial);
               return  await _context.SaveChangesAsync(cancellationToken)> 0;
            },Mensaje.GetMessage(true, "eliminado"));
        }
    }
}
