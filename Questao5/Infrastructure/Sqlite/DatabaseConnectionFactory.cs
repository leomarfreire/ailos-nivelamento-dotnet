using System.Data;
using Microsoft.Data.Sqlite;

namespace Questao5.Infrastructure.Sqlite
{
    public class DatabaseConnectionFactory
    {
        private readonly string _connectionString;

        public DatabaseConnectionFactory(DatabaseConfig config)
        {
            _connectionString = config.Name;
        }

        public IDbConnection CreateConnection() => new SqliteConnection(_connectionString);
    }
}