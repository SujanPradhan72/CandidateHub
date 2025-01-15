using CandidateHub.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CandidateHub.Tests;

public class AppDbContextTest
{
    [Fact]
    public void EnsureDatabaseCreated_ShouldCreateDatabase()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite("DataSource=:memory:")
            .Options;
        
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                { "ConnectionStrings:DefaultConnection", "InMemoryConnection" }
            }!)
            .Build();

        using var context = new AppDbContext(options, new DbOption(configuration));

        // Act
        context.EnsureDatabaseCreated();
        var canConnect = context.Database.CanConnect();

        // Assert
        Assert.True(canConnect, "Database should be able to connect.");
    }
}