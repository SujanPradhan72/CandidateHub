using CandidateHub.Data;
using CandidateHub.Data.Repos.IRepos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CandidateHub.Tests;

public class ServiceRegistrationTest
{
    [Fact]
    public void ConfigureServices_ShouldRegisterServicesCorrectly()
    {
        // Arrange
        var serviceCollection = new ServiceCollection();

        // Configure DbOption
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                { "ConnectionStrings:DefaultConnection", "InMemoryConnection" }
            }!)
            .Build();

        // Register IConfiguration in DI
        serviceCollection.AddSingleton<IConfiguration>(configuration);
        serviceCollection.AddSingleton<IDbOption, DbOption>();

        // Configure In-Memory Database for testing
        serviceCollection.AddDbContext<AppDbContext>(options =>
            options.UseInMemoryDatabase("TestDatabase"));

        // Act
        serviceCollection.ConfigureServices();
        var serviceProvider = serviceCollection.BuildServiceProvider();

        // Assert
        var dbContext = serviceProvider.GetService<AppDbContext>();
        Assert.NotNull(dbContext);

        var candidateRepository = serviceProvider.GetService<ICandidateRepository>();
        Assert.NotNull(candidateRepository);
    }
}