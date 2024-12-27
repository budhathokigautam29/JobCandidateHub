using JobCandidateHubAPI.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;

namespace JobCandidateHubAPI.Services
{
    public class SqlConnectionService : IDatabaseConnectionFactory
    {
        private readonly string _connectionString;

        public SqlConnectionService(string connectionString) => _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));

        public async Task<IDbConnection> CreateConnectionAsync()
        {
            var sqlConnection = new SqlConnection(_connectionString);
            await sqlConnection.OpenAsync();
            return sqlConnection;
        }
        public async Task<IDbConnection> CreateConnectionAsync(string connection)
        {
            var sqlConnection = new SqlConnection(connection);
            await sqlConnection.OpenAsync();
            return sqlConnection;
        }
    }
}
