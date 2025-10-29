
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using Dapper;


namespace server_pra.Models
{
    public interface ILoggerService
    {
        Task LogAsync(string message, string logLevel, string date = null, string exception = null, string userName = null, string logId = null);
    }

    public class LoggerService : ILoggerService
    {
        private readonly string _connectionString;

        public LoggerService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("LogsConnection");
        }

        public async Task LogAsync(string message, string logLevel, string date = null, string exception = null, string userName = null, string logId = null)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = @"INSERT INTO Logs (Message, LogLevel, Exception, UserName, LogId, Date) 
                       VALUES (@Message, @LogLevel, @Exception, @UserName, @LogId, @Date)";
            
            await connection.ExecuteAsync(sql, new { Message = message, LogLevel = logLevel, Exception = exception, UserName = userName, LogId = logId, Date = DateTime.Now });
        }
    }
}