using Backend.Application.DTO;
using Backend.Application.Features.Productos.Command;
using Backend.Application.Features.Productos.Query;
using Backend.Common.Core.Controller;
using Backend.Common.Core.Wrapper;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers.Producto
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriaController : ApiController
    {
        [HttpGet("ver-categorias")]
        public async Task<Response<IEnumerable<CategoriasDTO>>> Get([FromQuery] CategoriaQuery query, CancellationToken cancellationToken)
            => await Mediator.Send(query, cancellationToken);

        [HttpPost("enviar-categoria")]
        public async Task<Response<CategoriasDTO>> Post([FromBody] CategoriaCommand command, CancellationToken cancellationToken)
            => await Mediator.Send(command, cancellationToken);

        [HttpDelete("eliminar-categoria")]
        public async Task<Response<bool>> Delete([FromBody] DeleteCategoriaCommand command, CancellationToken cancellationToken)
            => await Mediator.Send(command, cancellationToken);

    }
}
