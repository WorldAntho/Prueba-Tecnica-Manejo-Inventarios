using Backend.Application.DTO;
using Backend.Application.Features.Adjuntos.Command;
using Backend.Application.Features.Adjuntos.Query;
using Backend.Common.Core.Helpers;
using Backend.Common.Core.Persistence;
using Backend.Common.Core.Wrapper;
using Backend.Domain;
using Microsoft.EntityFrameworkCore;
using static Backend.Application.Features.Adjuntos.Command.AdjuntosCommandHandler;

namespace Backend.Infrastructure.Persistence.Repository.Adjunto
{
    public class AdjuntosRepository : IRepository
    {

        private readonly gestioninventariosContext _context;

        public AdjuntosRepository(gestioninventariosContext context)
        {
            _context = context;
        }

        public async Task<Response<IEnumerable<AdjuntosDTO>>> GetAdjuntos(AdjuntosQuery query, CancellationToken cancellationToken)
        {
            return await ExceptionHandler.HandleExceptionAsync<IEnumerable<AdjuntosDTO>>(async () =>
            {
                var adjuntos = _context.Adjuntos.AsQueryable();
                //Filtros para la busqueda de los adjuntos
                if (query.Idadjuntos.HasValue)
                    adjuntos = adjuntos.Where(x => x.Idadjuntos == query.Idadjuntos.Value);
                if (!string.IsNullOrEmpty(query.NombreArchivo))
                    adjuntos = adjuntos.Where(x => x.NombreArchivo.Contains(query.NombreArchivo));
                if (!string.IsNullOrEmpty(query.Extension))
                    adjuntos = adjuntos.Where(x => x.Extension.Contains(query.Extension));
                if (!string.IsNullOrEmpty(query.MimeType))
                    adjuntos = adjuntos.Where(x => x.MimeType.Contains(query.MimeType));
                if (query.TamanioBytes.HasValue)
                    adjuntos = adjuntos.Where(x => x.TamanioBytes == query.TamanioBytes.Value);
                if (!string.IsNullOrEmpty(query.Ruta))
                    adjuntos = adjuntos.Where(x => x.Ruta.Contains(query.Ruta));
                //Obtener los adjuntos por medio de la consulta que se hizo en el query
                var result = await adjuntos.Select(x => new AdjuntosDTO
                {
                    Idadjuntos = x.Idadjuntos,
                    NombreArchivo = x.NombreArchivo,
                    Extension = x.Extension,
                    MimeType = x.MimeType,
                    TamanioBytes = x.TamanioBytes,
                    Ruta = x.Ruta
                }).ToListAsync(cancellationToken);
                return result;
            }, Mensaje.GetMessage(true, "obtenido"));
        }

        public async Task<Response<AdjuntosDTO>> SaveAdjuntoAsync(AdjuntosCommand request, CancellationToken cancellationToken)
        {
            return await ExceptionHandler.HandleExceptionAsync(async () =>
            {
                Adjuntos adjunto;
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(request.Archivo.FileName);
                string filePath = Path.Combine("Files", fileName);
                string fullPath = Path.Combine(Directory.GetCurrentDirectory(), filePath);

                // Guardar el nuevo archivo en el sistema
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await request.Archivo.CopyToAsync(stream, cancellationToken);
                }

                if (request.Idadjuntos.HasValue)
                {
                    // Buscar el adjunto existente
                    adjunto = await _context.Adjuntos.FirstOrDefaultAsync(x => x.Idadjuntos == request.Idadjuntos.Value, cancellationToken)
                        ?? throw new InvalidOperationException(Mensaje.NotFound);

                    // Ruta completa del archivo anterior
                    string oldFullPath = Path.Combine(Directory.GetCurrentDirectory(), adjunto.Ruta);

                    // Actualizar las propiedades del adjunto
                    adjunto.NombreArchivo = fileName;
                    adjunto.Extension = Path.GetExtension(request.Archivo.FileName);
                    adjunto.MimeType = request.Archivo.ContentType;
                    adjunto.TamanioBytes = request.Archivo.Length;
                    adjunto.Ruta = filePath;

                    // Eliminar el archivo anterior si existe
                    if (File.Exists(oldFullPath))
                    {
                        File.Delete(oldFullPath);
                    }

                    // Asegurar que EF Core rastree los cambios
                    _context.Adjuntos.Update(adjunto);
                }
                else
                {
                    // Crear un nuevo adjunto
                    adjunto = new Adjuntos
                    {
                        NombreArchivo = fileName,
                        Extension = Path.GetExtension(request.Archivo.FileName),
                        MimeType = request.Archivo.ContentType,
                        TamanioBytes = request.Archivo.Length,
                        Ruta = filePath
                    };

                    // Agregar el nuevo adjunto al contexto
                    await _context.Adjuntos.AddAsync(adjunto, cancellationToken);
                }

                // Guardar los cambios en la base de datos
                await _context.SaveChangesAsync(cancellationToken);

                return new AdjuntosDTO
                {
                    Idadjuntos = adjunto.Idadjuntos,
                    NombreArchivo = adjunto.NombreArchivo,
                    Extension = adjunto.Extension,
                    MimeType = adjunto.MimeType,
                    TamanioBytes = adjunto.TamanioBytes,
                    Ruta = adjunto.Ruta
                };
            }, Mensaje.GetMessage(true, request.Idadjuntos.HasValue ? "actualizado" : "creado"));
        }


        public async Task<Response<bool>> DeleteAdjuntoAsync(DeleteAdjuntoCommand request, CancellationToken cancellationToken)
        {
            return await ExceptionHandler.HandleExceptionAsync<bool>(async () =>
            {
                var adjunto = await _context.Adjuntos.FirstOrDefaultAsync(x => x.Idadjuntos == request.Idadjuntos)
                    ?? throw new InvalidOperationException(Mensaje.NotFound);
                _context.Adjuntos.Remove(adjunto);
                return await _context.SaveChangesAsync(cancellationToken) > 0;
            }, Mensaje.GetMessage(true, "eliminado"));
        }
    }
}
