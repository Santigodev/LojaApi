using Dapper;
using System.Data;
using LojaApi.Models;
using MySql.Data.MySqlClient;

namespace LojaApi.Repositories
{
    public class ProdutoRepository
    {
        private readonly string _connectionString;

        public ProdutoRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        private IDbConnection Connection => new MySqlConnection(_connectionString);

        public async Task<int> CadastrarProdutoDB(Produto produto)
        {
            using (var conn = Connection)
            {
                var sql = "INSERT INTO Produtos (Nome, Descricao, Preco, QuantidadeEstoque) " +
                          "VALUES (@Nome, @Descricao, @Preco, @QuantidadeEstoque);" +
                          "SELECT LAST_INSERT_ID();";

                return await conn.ExecuteScalarAsync<int>(sql, produto);
            }
        }

        public async Task<IEnumerable<Produto>> ListarProdutosDB()
        {
            using (var conn = Connection)
            {
                var sql = "SELECT * FROM Produtos";
                return await conn.QueryAsync<Produto>(sql);
            }
        }

        public async Task<int> AtualizarProdutoDB(Produto produto)
        {
            using (var conn = Connection)
            {
                var sql = "UPDATE Produtos SET Nome = @Nome, Descricao = @Descricao, Preco = @Preco, " +
                          "QuantidadeEstoque = @QuantidadeEstoque WHERE Id = @Id";

                return await conn.ExecuteAsync(sql, produto);
            }
        }

        public async Task<int> ExcluirProdutoDB(int id)
        {
            using (var conn = Connection)
            {
                //Verificando se o produto está em um carrinho ativo
                var sqlVerificarCarrinho = "SELECT COUNT(*) FROM Carrinho WHERE ProdutoId = @Id";
                var carrinhoCount = await conn.ExecuteScalarAsync<int>(sqlVerificarCarrinho, new { Id = id });

                if (carrinhoCount > 0)
                {
                    throw new InvalidOperationException("O produto está em um carrinho ativo e não pode ser excluído.");
                }

                var sqlExcluirProduto = "DELETE FROM Produtos WHERE Id = @Id";
                return await conn.ExecuteAsync(sqlExcluirProduto, new { Id = id });
            }
        }
    }
}
