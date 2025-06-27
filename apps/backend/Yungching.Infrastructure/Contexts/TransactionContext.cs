using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yungching.Infrastructure.Contexts
{
    public class TransactionContext : IDisposable
    {
        private readonly DatabaseSettings _settings;
        private readonly ILogger<TransactionContext>? _logger;

        public IDbConnection Connection { get; private set; }
        public IDbTransaction Transaction { get; private set; }

        public TransactionContext(
            DatabaseSettings settings, 
            ILogger<TransactionContext>? logger = null
        )
        {
            _settings = settings;
            _logger = logger;
        }

        public IDbConnection CreateConnection()
        {
            var provider = Enum.TryParse<DbProvider>(_settings.Provider, ignoreCase: true, out var parsedProvider)
                ? parsedProvider
                : throw new NotSupportedException($"Invalid provider: {_settings.Provider}");

            var connectionString = _settings.ConnectionString;

            switch (provider)
            {
                case DbProvider.SqlServer:
                    return new SqlConnection(connectionString);
                default:
                    throw new NotSupportedException($"Provider {provider} is not supported.");
            }
        }

        /// <summary>
        /// Executes a database operation inside a transaction.
        /// This method opens a connection, begins a transaction, executes the operation, and commits the transaction.
        /// If an error occurs, it rolls back the transaction.
        /// </summary>
        public async Task ExecuteAsync(Action<IDbConnection, IDbTransaction> operation)
        {
            try
            {
                await OpenConnectionAsync();
                BeginTransaction();

                operation(Connection, Transaction);

                CommitTransaction();
            }
            catch (Exception e)
            {
                RollbackTransaction();
                _logger?.LogError(e, "Transaction failed");
                throw;
            }
            finally
            {
                CloseConnection();
            }
        }

        public void Begin()
        {
            OpenConnectionAsync();
            BeginTransaction();
        }

        public void Commit()
        {
            CommitTransaction();
        }

        public void Rollback()
        {
            RollbackTransaction();
        }

        public void Dispose()
        {
            CloseConnection();
        }

        private async Task OpenConnectionAsync()
        {
            Connection = CreateConnection();
            Connection.Open();
        }

        private void BeginTransaction()
        {
            Transaction = Connection.BeginTransaction();
        }

        private void CommitTransaction()
        {
            Transaction?.Commit();
        }

        private void RollbackTransaction()
        {
            Transaction?.Rollback();
        }

        private void CloseConnection()
        {
            Transaction?.Dispose();
            Connection?.Close();
            Connection?.Dispose();
        }
    }
}
