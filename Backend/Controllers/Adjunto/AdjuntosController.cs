using Backend.Application.DTO;
using Backend.Application.Features.Adjuntos.Command;
using Backend.Application.Features.Adjuntos.Query;
using Backend.Common.Core.Controller;
using Backend.Common.Core.Wrapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Backend.Application.Features.Adjuntos.Command.AdjuntosCommandHandler;

namespace Backend.Controllers.Adjunto
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdjuntosController : ApiController
    {
        [HttpGet("ver-adjuntos")]
        public async Task<Response<IEnumerable<AdjuntosDTO>>> Get([FromQuery] AdjuntosQuery query, CancellationToken cancellationToken)
            => await Mediator.Send(query, cancellationToken);

        [HttpPost("enviar-adjuntos")]
        public async Task<Response<AdjuntosDTO>> Post([FromBody] AdjuntosCommand command, CancellationToken cancellationToken)
            => await Mediator.Send(command, cancellationToken);

        [HttpDelete("eliminar-adjunto")]
        public async Task<Response<bool>> Delete([FromBody] DeleteAdjuntoCommand command, CancellationToken cancellationToken)
            => await Mediator.Send(command, cancellationToken);
    }
}