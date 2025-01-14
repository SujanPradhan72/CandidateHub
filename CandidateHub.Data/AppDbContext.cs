using CandidateHub.Data.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace CandidateHub.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options, DbOption dbOption): DbContext(options)
{
    private readonly string _connectionString = dbOption.GetPostgresConnectionString();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CandidateConfig());
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder
                .UseNpgsql(_connectionString)
                .UseSnakeCaseNamingConvention()
                .UseLazyLoadingProxies()
                .ConfigureWarnings(w => w.Ignore(CoreEventId.DetachedLazyLoadingWarning));
        }
    }

}