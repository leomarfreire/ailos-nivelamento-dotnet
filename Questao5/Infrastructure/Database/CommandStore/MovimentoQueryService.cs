using Dapper;
using Questao5.Domain.Entities;
using Questao5.Infrastructure.Sqlite;

namespace Questao5.Infrastructure.Database.CommandStore
{
	public class MovimentoQueryService
	{
		private readonly DatabaseConnectionFactory _databaseConnectionFactory;

		public MovimentoQueryService(DatabaseConnectionFactory connectionFactory)
		{
			_databaseConnectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
		}

		public async Task Salvar(Movimento movimento)
		{
			using (var connection = _databaseConnectionFactory.CreateConnection())
			{
				string sql = @"
                    INSERT INTO movimento (idmovimento, idcontacorrente, datamovimento, tipomovimento, valor)
                    VALUES (@IdMovimento, @IdContaCorrente, @DataMovimento, @TipoMovimento, @Valor)";

				await connection.ExecuteAsync(sql, new
				{
					IdMovimento = movimento.IdMovimento,
					IdContaCorrente = movimento.IdContaCorrente,
					DataMovimento = movimento.DataMovimento,
					TipoMovimento = movimento.TipoMovimento,
					Valor = movimento.Valor
				});
			}
		}

		public async Task<decimal> ObterSaldoPorId(string idContaCorrente)
		{
			using (var connection = _databaseConnectionFactory.CreateConnection())
			{
				string sql = @"
                    SELECT COALESCE(SUM(CASE WHEN tipomovimento = 'C' THEN valor ELSE 0 END), 0) -
                        COALESCE(SUM(CASE WHEN tipomovimento = 'D' THEN valor ELSE 0 END), 0)
                    FROM movimento
                    WHERE idcontacorrente = @Id";

				return await connection.ExecuteScalarAsync<decimal>(sql, new { Id = idContaCorrente });
			}
		}
	}
}