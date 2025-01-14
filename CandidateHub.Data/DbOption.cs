using Microsoft.Extensions.Configuration;
using Npgsql;

namespace CandidateHub.Data;

public class DbOption(IConfiguration configuration)
{
    public string GetPostgresConnectionString()
    {
        var builder = new NpgsqlConnectionStringBuilder()
        {
            Host = Host,
            Port = Port,
            Database = Database,
            Username = Username,
            Password = Password,
            IncludeErrorDetail = true
        };
        return builder.ConnectionString;
    }

    private string Host { get; } = configuration.GetSection($"DbServer:Host").Value ??
                                   throw new SystemException("Database server not specified");

    private int Port { get; } = 5445; // Get from configuration
    private string? Database { get; } = configuration.GetSection($"DbServer:Database").Value ?? "candidate";
    private string? Username { get; } = configuration.GetSection($"DbServer:User").Value ?? "postgres";
    private string? Password { get; } = configuration.GetSection($"DbServer:Password").Value;
}