using Dapper;
using Questao5.Domain.Entities;
using Questao5.Infrastructure.Sqlite;

namespace Questao5.Infrastructure.Database.CommandStore
{
	public class IdempotenciaQueryService
	{
		private readonly DatabaseConnectionFactory _databaseConnectionFactory;

		public IdempotenciaQueryService(DatabaseConnectionFactory connectionFactory)
		{
			_databaseConnectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
		}

		public async Task<Idempotencia> ObterPorId(string idIdempotencia)
		{
			using (var connection = _databaseConnectionFactory.CreateConnection())
			{
				string sql = "SELECT * FROM idempotencia WHERE chave_idempotencia = @Id";
				return await connection.QueryFirstOrDefaultAsync<Idempotencia>(sql, new { Id = idIdempotencia });
			}
		}

		public async Task Salvar(Idempotencia idempotencia)
		{
			using (var connection = _databaseConnectionFactory.CreateConnection())
			{
				string sql = @"
                    INSERT INTO idempotencia (chave_idempotencia, requisicao, resultado)
                    VALUES (@ChaveIdempotencia, @Requisicao, @Resultado)";

				await connection.ExecuteAsync(sql, new
				{
					ChaveIdempotencia = idempotencia.ChaveIdempotencia,
					Requisicao = idempotencia.Requisicao,
					Resultado = idempotencia.Resultado
				});
			}
		}
	}
}