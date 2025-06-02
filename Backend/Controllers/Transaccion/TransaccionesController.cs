using Backend.Application.DTO;
using Backend.Application.Features.Transacciones.Command;
using Backend.Application.Features.Transacciones.Query;
using Backend.Common.Core.Controller;
using Backend.Common.Core.Wrapper;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers.Transaccion
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransaccionesController : ApiController
    {
        [HttpGet("ver-transacciones")]
        public async Task<Response<IEnumerable<TransaccionesDTO>>> Get ([FromQuery] TransaccionesQuery query, CancellationToken cancellationToken)
            => await Mediator.Send(query, cancellationToken);

        [HttpPost("enviar-transaccion")]
        public async Task<Response<TransaccionesDTO>> Post([FromBody] TransaccionesCommand command, CancellationToken cancellationToken)
            => await Mediator.Send(command, cancellationToken);

        [HttpDelete("eliminar-transaccion")]
        public async Task<Response<bool>> Delete([FromBody] DeleteTransaccionCommand command, CancellationToken cancellationToken)
            => await Mediator.Send(command, cancellationToken);
    }
}
