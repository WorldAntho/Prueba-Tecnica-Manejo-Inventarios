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
    public class ProductosController : ApiController
    {
        [HttpGet("ver-productos")]
        public async Task<Response<IEnumerable<ProductosDTO>>> Get([FromQuery] ProductosQuery query, CancellationToken cancellationToken)
            => await Mediator.Send(query, cancellationToken);

        [HttpPost("enviar-producto")]
        public async Task<Response<ProductosDTO>> Post([FromBody] ProductosCommand command, CancellationToken cancellationToken)
            => await Mediator.Send(command, cancellationToken);

        [HttpDelete("eliminar-producto")]
        public async Task<Response<bool>> Delete([FromBody] DeleteProductoCommand command, CancellationToken cancellationToken)
            => await Mediator.Send(command, cancellationToken);
    }
}
