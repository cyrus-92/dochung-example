using System.Data;

namespace Cyrus.Test.Infrastructure
{
    public interface IDatabaseService
    {
        Task<IEnumerable<T>> GetAllAsync<T>(string sqlQuery, CommandType commandType = CommandType.StoredProcedure, CancellationToken cancellationToken = default) where T : class, new();

        Task<IEnumerable<T>> GetAllAsync<T>(string sqlQuery, CommandType commandType = CommandType.StoredProcedure, Dictionary<string, object> parameters = null, CancellationToken cancellationToken = default) where T : class, new();

        Task<T> GetAsync<T>(string sqlQuery, CommandType commandType = CommandType.StoredProcedure, CancellationToken cancellationToken = default) where T : class, new();

        Task<T> GetAsync<T>(string sqlQuery, CommandType commandType = CommandType.StoredProcedure, Dictionary<string, object> parameters = null, CancellationToken cancellationToken = default) where T : class, new();

        Task<bool> ExecuteAsync(string sqlQuery, CommandType commandType = CommandType.StoredProcedure, CancellationToken cancellationToken = default);

        Task<bool> ExecuteAsync(string sqlQuery, CommandType commandType = CommandType.StoredProcedure, Dictionary<string, object> parameters = null, CancellationToken cancellationToken = default);
    }
}
