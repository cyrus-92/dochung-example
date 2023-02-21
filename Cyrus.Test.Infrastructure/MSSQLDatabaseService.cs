using Cyrus.Test.Shared.Configurations;
using Cyrus.Test.Shared.Extensions;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Cyrus.Test.Infrastructure
{
    public class MSSQLDatabaseService : IDatabaseService
    {
        private readonly DatabaseSetting _databaseSetting;

        public MSSQLDatabaseService(IConfiguration configuration)
        {
            _databaseSetting = configuration?.GetOptions<DatabaseSetting>() ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<bool> ExecuteAsync(string sqlQuery, CommandType commandType = CommandType.StoredProcedure, CancellationToken cancellationToken = default)
            => await ExecuteAsync(sqlQuery, commandType, null, cancellationToken).ConfigureAwait(false);

        public async Task<bool> ExecuteAsync(string sqlQuery, CommandType commandType = CommandType.StoredProcedure, Dictionary<string, object> parameters = null, CancellationToken cancellationToken = default)
        {
            using SqlConnection sqlConn = new(_databaseSetting.Default);
            SqlCommand sqlCmd = new(sqlQuery, sqlConn)
            {
                CommandText = sqlQuery,
                CommandType = commandType
            };

            if (parameters.NotNullOrEmpty())
            {
                foreach (var param in parameters)
                    sqlCmd.Parameters.AddWithValue(param.Key, param.Value);
            }

            await sqlConn.OpenAsync(cancellationToken).ConfigureAwait(false);

            int changedRows = await sqlCmd.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);

            await sqlConn.CloseAsync().ConfigureAwait(false);

            return changedRows >= 0;
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>(string sqlQuery, CommandType commandType = CommandType.StoredProcedure, CancellationToken cancellationToken = default) where T : class, new()
            => await GetAllAsync<T>(sqlQuery, commandType, null, cancellationToken).ConfigureAwait(false);

        public async Task<IEnumerable<T>> GetAllAsync<T>(string sqlQuery, CommandType commandType = CommandType.StoredProcedure, Dictionary<string, object> parameters = null, CancellationToken cancellationToken = default) where T : class, new()
        {
            using SqlConnection sqlConn = new(_databaseSetting.Default);
            SqlCommand sqlCmd = new(sqlQuery, sqlConn)
            {
                CommandText = sqlQuery,
                CommandType = commandType
            };

            if (parameters.NotNullOrEmpty())
            {
                foreach (var param in parameters)
                    sqlCmd.Parameters.AddWithValue(param.Key, param.Value);
            }

            await sqlConn.OpenAsync(cancellationToken).ConfigureAwait(false);

            SqlDataAdapter sqlDataAdapter = new(sqlCmd);
            DataTable dataTable = new();
            sqlDataAdapter.Fill(dataTable);

            await sqlConn.CloseAsync().ConfigureAwait(false);

            return dataTable.ToList<T>();
        }

        public async Task<T> GetAsync<T>(string sqlQuery, CommandType commandType = CommandType.StoredProcedure, CancellationToken cancellationToken = default) where T : class, new()
            => await GetAsync<T>(sqlQuery, commandType, null, cancellationToken).ConfigureAwait(false);

        public async Task<T> GetAsync<T>(string sqlQuery, CommandType commandType = CommandType.StoredProcedure, Dictionary<string, object> parameters = null, CancellationToken cancellationToken = default) where T : class, new()
        {
            var items = await GetAllAsync<T>(sqlQuery, commandType, parameters, cancellationToken).ConfigureAwait(false);
            if (items.NotNullOrEmpty())
                return items.FirstOrDefault();

            return default;
        }
    }
}
