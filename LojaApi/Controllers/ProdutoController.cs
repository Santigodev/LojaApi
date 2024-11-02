using LojaApi.Models;
using LojaApi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace LojaApi.Controllers
{
    [Route("api/produto")]
    [ApiController]
    public class ProdutoController : ControllerBase
    {
        private readonly ProdutoRepository _produtoRepository;

        public ProdutoController(ProdutoRepository produtoRepository)
        {
            _produtoRepository = produtoRepository;
        }

        [HttpPost("cadastrar-produto")]
        public async Task<IActionResult> CadastrarProduto([FromBody] Produto produto)
        {
            var produtoId = await _produtoRepository.CadastrarProdutoDB(produto);

            return Ok(new { mensagem = "Produto cadastrado!", produtoId });
        }

        [HttpGet("listar-produtos")]
        public async Task<IActionResult> ListarProdutos()
        {
            var produtos = await _produtoRepository.ListarProdutosDB();

            return Ok(produtos);
        }

        [HttpPut("atualizar-produto/{id}")]
        public async Task<IActionResult> AtualizarProduto(int id, [FromBody] Produto produto)
        {
            produto.Id = id;
            await _produtoRepository.AtualizarProdutoDB(produto);

            return Ok(new { mensagem = "Produto atualizado!" });
        }

        [HttpDelete("excluir-produto/{id}")]
        public async Task<IActionResult> ExcluirProduto(int id)
        {
            try
            {
                await _produtoRepository.ExcluirProdutoDB(id);

                return Ok(new { mensagem = "Produto excluído!" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        [HttpGet("buscar-produtos-filtro")]
        public async Task<IActionResult> BuscarProdutosPorFiltro([FromQuery] string? nome, [FromQuery] string? descricao, [FromQuery] decimal? precoMin, [FromQuery] decimal? precoMax)
        {
            var produtos = await _produtoRepository.BuscarProdutosPorFiltroDB(nome, descricao, precoMin, precoMax);

            return Ok(produtos);
        }
    }
}
