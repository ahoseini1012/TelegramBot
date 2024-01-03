
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using exhibition_bot.Models;

namespace exhibition_bot
{
    public static class DBContext
    {
        public static IDbConnection CreateConnection()
        {
            var configuration = new ConfigurationBuilder().AddJsonFile($"appsettings.json");

            var config = configuration.Build();
            var connectionString = config.GetSection("DB:ConnectionString");
            var connection = new SqlConnection(connectionString.Value!);
            return connection;
        }

    }
}