using LojaApi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace LojaApi.Controllers
{
    [Route("api/pedido")]
    [ApiController]
    public class PedidoController : ControllerBase
    {
        private readonly PedidoRepository _pedidoRepository;

        public PedidoController(PedidoRepository pedidoRepository)
        {
            _pedidoRepository = pedidoRepository;
        }

        [HttpPost("criar-pedido")]
        public async Task<IActionResult> CriarPedido(int usuarioId)
        {
            try
            {
                var pedidoId = await _pedidoRepository.CriarPedidoDB(usuarioId);
                
                return Ok(new { mensagem = "Pedido criado", pedidoId });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        [HttpGet("listar-pedidos")]
        public async Task<IActionResult> ListarPedidosUsuario(int usuarioId)
        {
            var pedidos = await _pedidoRepository.ListarPedidosUsuarioDB(usuarioId);
            
            return Ok(pedidos);
        }

        [HttpGet("consultar-status")]
        public async Task<IActionResult> ConsultarStatusPedido(int pedidoId)
        {
            var status = await _pedidoRepository.ConsultarStatusPedidoDB(pedidoId);
            
            return Ok(new { pedidoId, status });
        }

        [HttpGet("historico-pedidos")]
        public async Task<IActionResult> ConsultarHistoricoPedidos(int usuarioId)
        {
            var historicoPedidos = await _pedidoRepository.ConsultarHistoricoCompletoPedidosDB(usuarioId);

            return Ok(historicoPedidos);
        }
    }
}
