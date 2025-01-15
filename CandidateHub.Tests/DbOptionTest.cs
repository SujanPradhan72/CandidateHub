using CandidateHub.Data;
using Microsoft.Extensions.Configuration;

namespace CandidateHub.Tests;

public class DbOptionTest
{
    [Fact]
    public void GetSqlLiteConnectionString_Returns_ValidConnectionString()
    {
        // Arrange
        var inMemorySettings = new Dictionary<string, string>
        {
            { "DbServer:Database", "test_database" }
        };

        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings!)
            .Build();

        var dbOption = new DbOption(configuration);

        // Act
        var connectionString = dbOption.GetSqlLiteConnectionString();

        // Assert
        Assert.Equal("Data Source=test_database", connectionString);
    }

    [Fact]
    public void GetSqlLiteConnectionString_UsesDefaultDatabase_WhenNotSpecified()
    {
        // Arrange
        var inMemorySettings = new Dictionary<string, string>();
        if (inMemorySettings == null) throw new ArgumentNullException(nameof(inMemorySettings));
        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings!)
            .Build();

        var dbOption = new DbOption(configuration);

        // Act
        var connectionString = dbOption.GetSqlLiteConnectionString();

        // Assert
        Assert.Equal("Data Source=candidate", connectionString);
    }
}