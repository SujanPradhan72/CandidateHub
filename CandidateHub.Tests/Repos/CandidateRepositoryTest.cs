using CandidateHub.Data;
using CandidateHub.Data.Repos;
using CandidateHub.Modules.Entities;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace CandidateHub.Tests.Repos;

public class CandidateRepositoryTest
{
    private readonly AppDbContext _context;
    private readonly CandidateRepository _candidateRepository;

    public CandidateRepositoryTest()
    {
        Mock<IDbOption> mockDbOption =
            new();
        mockDbOption.Setup(option => option.GetSqlLiteConnectionString()).Returns("YourConnectionString");

        // Set up an in-memory database for testing
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "CandidateDbTest") // Use a unique database name for each test run
            .Options;

        _context = new AppDbContext(options,
            mockDbOption.Object); // Instantiate the AppDbContext with in-memory options
        _candidateRepository = new CandidateRepository(_context); // Use in-memory context
    }

    [Fact]
    public async Task GetByEmailAsync_ReturnsCandidate_WhenExists()
    {
        // Arrange
        var candidateEmail = "test@example.com";
        var candidate = new Candidate
        {
            Email = candidateEmail, FirstName = candidateEmail, LastName = candidateEmail, Comments = "",
            PhoneNumber = "", CallTimeInterval = "", GithubProfileUrl = "", LinkedInProfileUrl = ""
        };

        // Add candidate to in-memory database
        await _context.AddAsync(candidate);
        await _context.SaveChangesAsync();

        // Act
        var result = await _candidateRepository.GetByEmailAsync(candidateEmail);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(candidateEmail, result.Email);
    }

    [Fact]
    public async Task GetByEmailAsync_ReturnsNull_WhenNotFound()
    {
        // Arrange
        var candidateEmail = "notfound@example.com";

        // Act
        var result = await _candidateRepository.GetByEmailAsync(candidateEmail);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task UpsertCandidateAsync_CreatesNewCandidate_WhenNotExists()
    {
        // Arrange
        var candidate = new Candidate { Email = "new@example.com", FirstName = "new", LastName = "new", Comments = "" };

        // Act
        var result = await _candidateRepository.UpsertCandidateAsync(candidate);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(candidate.Email, result.Email);
    }

    [Fact]
    public async Task UpsertCandidateAsync_UpdatesCandidate_WhenExists()
    {
        // Arrange
        var existingCandidate = new Candidate
            { Email = "existing@example.com", FirstName = "existing", LastName = "existing", Comments = "" };

        // Add existing candidate to in-memory database
        await _context.AddAsync(existingCandidate);
        await _context.SaveChangesAsync();

        // Modify candidate properties
        existingCandidate.Email = "updated@example.com";

        // Act
        var result = await _candidateRepository.UpsertCandidateAsync(existingCandidate);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(existingCandidate.Email, result.Email);
    }
}