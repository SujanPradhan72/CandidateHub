using CandidateHub.Data;
using CandidateHub.Data.Repos;
using CandidateHub.Modules.Entities;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace CandidateHub.Tests.Repos;

public class BaseRepositoryTest
{
    private readonly Mock<AppDbContext> _mockContext;
    private readonly Mock<DbSet<Candidate>> _mockDbSet;
    private readonly BaseRepository<Candidate> _repository;

    public BaseRepositoryTest()
    {
        Mock<IDbOption> mockDbOption =
            new();
        mockDbOption.Setup(option => option.GetSqlLiteConnectionString()).Returns("YourConnectionString");

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite("YourConnectionString")
            .Options;

        _mockDbSet = new Mock<DbSet<Candidate>>();

        _mockContext = new Mock<AppDbContext>(options, mockDbOption.Object);
        _mockContext.Setup(m => m.Set<Candidate>()).Returns(_mockDbSet.Object);

        _repository = new BaseRepository<Candidate>(_mockContext.Object);
    }

    [Fact]
    public async Task UpsertAsync_EntityDoesNotExist_AddsEntity()
    {
        // Arrange
        var entityId = "123";
        var newCandidate = new Candidate
        {
            Email = entityId,
            FirstName = "New FirstName",
            LastName = "New LastName",
            Comments = "New Comments"
        };

        // Mock FindAsync to return null (no entity found)
        _mockDbSet.Setup(db => db.FindAsync(It.IsAny<object[]>())).ReturnsAsync((Candidate)null!);

        // Act
        var result = await _repository.UpsertAsync(newCandidate, entityId);

        // Assert
        _mockDbSet.Verify(db => db.AddAsync(It.IsAny<Candidate>(), It.IsAny<CancellationToken>()),
            Times.Once);
        _mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once);
        Assert.Equal("New FirstName", result.FirstName);
        Assert.Equal("New Comments", result.Comments); 
    }

    [Fact]
    public async Task InsertAsync_AddsEntity()
    {
        // Arrange
        var newCandidate = new Candidate
        {
            Email = "123",
            FirstName = "First",
            LastName = "Last",
            Comments = "Some comments"
        };

        // Act
        var result = await _repository.InsertAsync(newCandidate);

        // Assert
        _mockDbSet.Verify(db => db.AddAsync(It.IsAny<Candidate>(), It.IsAny<CancellationToken>()),
            Times.Once);
        _mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once); 
        Assert.Equal("First", result.FirstName);
    }
    
    [Fact]
    public async Task GetByStringIdAsync_ReturnsEntity_WhenExists()
    {
        // Arrange
        var entityId = "123";
        var existingCandidate = new Candidate
        {
            Email = entityId,
            FirstName = "Existing",
            LastName = "Last",
            Comments = ""
        };

        _mockDbSet.Setup(db => db.FindAsync(It.IsAny<object[]>())).ReturnsAsync(existingCandidate);

        var method = typeof(BaseRepository<Candidate>).GetMethod("GetByStringIdAsync",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        // Act
        var result = await (Task<Candidate>)method?.Invoke(_repository, [entityId])!;

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Existing", result.FirstName);
    }

    [Fact]
    public async Task GetByStringIdAsync_ReturnsNull_WhenNotFound()
    {
        // Arrange
        var entityId = "123";

        _mockDbSet.Setup(db => db.FindAsync(It.IsAny<object[]>())).ReturnsAsync((Candidate)null!);

        var method = typeof(BaseRepository<Candidate>).GetMethod("GetByStringIdAsync",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        // Act
        var result = await (Task<Candidate>)method?.Invoke(_repository, [entityId])!;

        // Assert
        Assert.Null(result);
    }
}