using Backend.Application.DTO;
using Backend.Application.Features.Transaccion.Command;
using Backend.Application.Features.Transaccion.Query;
using Backend.Common.Core.Controller;
using Backend.Common.Core.Wrapper;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers.Transaccion
{
    [Route("api/[controller]")]
    [ApiController]
    public class TipoTransaccionController : ApiController
    {
        [HttpGet("ver-tipos-transaccion")]
        public async Task<Response<IEnumerable<TipoTransaccionDTO>>> Get([FromQuery] TipoTransaccionQuery query, CancellationToken cancellationToken)
            => await Mediator.Send(query, cancellationToken);

        [HttpPost("enviar-tipo-transaccion")]
        public async Task<Response<TipoTransaccionDTO>> Post([FromBody] TipoTransaccionCommand command, CancellationToken cancellationToken)
            => await Mediator.Send(command, cancellationToken);

        [HttpDelete("eliminar-tipo-transaccion")]
        public async Task<Response<bool>> Delete([FromBody] DeleteTipoTransaccionCommand command, CancellationToken cancellationToken)
            => await Mediator.Send(command, cancellationToken);
    }
}
