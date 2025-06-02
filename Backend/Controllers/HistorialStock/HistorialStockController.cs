using Backend.Application.DTO;
using Backend.Application.Features.HistorialStock.Command;
using Backend.Application.Features.HistorialStock.Query;
using Backend.Common.Core.Controller;
using Backend.Common.Core.Wrapper;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers.HistorialStock
{
    [Route("api/[controller]")]
    [ApiController]
    public class HistorialStockController : ApiController
    {
        [HttpGet("ver-historial-stock")]
        public async Task<Response<IEnumerable<HistorialStockDTO>>> Get([FromQuery] HistorialStockQuery query, CancellationToken cancellationToken)
            => await Mediator.Send(query, cancellationToken);

        [HttpPost("enviar-historial-stock")]
        public async Task<Response<HistorialStockDTO>> Post([FromBody] HistorialStockCommand command, CancellationToken cancellationToken)
            => await Mediator.Send(command, cancellationToken);

        [HttpDelete("eliminar-historial-stock")]
        public async Task<Response<bool>> Delete([FromBody] DeleteHistorialStockCommand command, CancellationToken cancellationToken)
            => await Mediator.Send(command, cancellationToken);
    }
}
