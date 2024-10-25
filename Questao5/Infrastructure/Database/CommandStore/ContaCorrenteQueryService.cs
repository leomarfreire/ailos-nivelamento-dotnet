using Dapper;
using Questao5.Domain.Entities;
using Questao5.Infrastructure.Sqlite;

namespace Questao5.Infrastructure.Database.CommandStore
{
	public class ContaCorrenteQueryService
	{
		private readonly DatabaseConnectionFactory _databaseConnectionFactory;

		public ContaCorrenteQueryService(DatabaseConnectionFactory connectionFactory)
		{
			_databaseConnectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
		}

		public async Task<ContaCorrente> ObterPorId(string idContaCorrente)
		{
			using (var connection = _databaseConnectionFactory.CreateConnection())
			{
				string sql = "SELECT * FROM contacorrente WHERE idcontacorrente = @Id";
				return await connection.QueryFirstOrDefaultAsync<ContaCorrente>(sql, new { Id = idContaCorrente });
			}
		}
	}
}