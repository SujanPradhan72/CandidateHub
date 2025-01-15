using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;

namespace CandidateHub.Data;

public class DbOption(IConfiguration configuration) : IDbOption
{
    // private string Host { get; } = configuration.GetSection($"DbServer:Host").Value ??
    //                                throw new SystemException("Database server not specified");
    //
    // private int Port { get; } = 5432; // Get from configuration
    private string? Database { get; } = configuration.GetSection($"DbServer:Database").Value ?? "candidate";
    // private string? Username { get; } = configuration.GetSection($"DbServer:User").Value ?? "postgres";
    // private string? Password { get; } = configuration.GetSection($"DbServer:Password").Value;
    public string GetSqlLiteConnectionString()
    {
        var builder = new SqliteConnectionStringBuilder()
        {
            DataSource = Database,
        };
        return builder.ConnectionString;
    }
    
    // For postgre db
    // public string GetPostgresConnectionString()
    // {
    //     var builder = new NpgsqlConnectionStringBuilder()
    //     {
    //         Host = Host,
    //         Port = Port,
    //         Database = Database,
    //         Username = Username,
    //         Password = Password,
    //         IncludeErrorDetail = true
    //     };
    //     return builder.ConnectionString;
    // }
}