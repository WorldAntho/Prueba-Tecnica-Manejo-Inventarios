using Backend.Application.DTO;
using Backend.Common.Core.Wrapper;
using Backend.Infrastructure.Persistence.Repository.Producto;
using MediatR;

namespace Backend.Application.Features.Productos.Command
{
    public class CategoriaCommand : IRequest<Response<CategoriasDTO>>
    {
        public int? IdCategoria { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public bool? Activo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }
    }

    public class CategoriaCommandHandler : IRequestHandler<CategoriaCommand, Response<CategoriasDTO>>
    {
        private readonly CategoriaRepository _repository;
        public CategoriaCommandHandler(CategoriaRepository repository)
        {
            _repository = repository;
        }
        public async Task<Response<CategoriasDTO>> Handle(CategoriaCommand request, CancellationToken cancellationToken)
        {
            return await _repository.SaveCategoriaAsync(request, cancellationToken);
        }
    }

    public class DeleteCategoriaCommand : IRequest<Response<bool>>
    {
        public int IdCategoria { get; set; }
    }

    public class DeleteCategoriaCommandHandler : IRequestHandler<DeleteCategoriaCommand, Response<bool>>
    {
        private readonly CategoriaRepository _repository;
        public DeleteCategoriaCommandHandler(CategoriaRepository repository)
        {
            _repository = repository;
        }
        public async Task<Response<bool>> Handle(DeleteCategoriaCommand request, CancellationToken cancellationToken)
        {
            return await _repository.DeleteCategoriaAsync(request, cancellationToken);
        }
    }
}
