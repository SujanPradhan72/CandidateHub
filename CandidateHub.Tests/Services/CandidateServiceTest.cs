using CandidateHub.Data.Caching;
using CandidateHub.Data.Repos.IRepos;
using CandidateHub.Data.Services;
using CandidateHub.Modules.Entities;
using Moq;

namespace CandidateHub.Tests.Services;

public class CandidateServiceTest
{
    private readonly Mock<ICandidateRepository> _mockCandidateRepository;
    private readonly Mock<ICachingService> _mockCachingService;
    private readonly CandidateService _candidateService;

    public CandidateServiceTest()
    {
        _mockCandidateRepository = new Mock<ICandidateRepository>();
        _mockCachingService = new Mock<ICachingService>();
        _candidateService = new CandidateService(
            _mockCandidateRepository.Object,
            _mockCachingService.Object);
    }

    [Fact]
    public async Task GetByEmailAsync_ShouldReturnCachedCandidate_WhenCacheIsNotNull()
    {
        // Arrange
        var email = "test@example.com";
        var cachedCandidate = new Candidate
        {
            Email = email,
            FirstName = "Existing",
            LastName = "Last",
            Comments = ""
        };
        _mockCachingService.Setup(c => c.GetValueAsync<Candidate>(email))
            .ReturnsAsync(cachedCandidate);

        // Act
        var result = await _candidateService.GetByEmailAsync(email);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(email, result.Email);
        _mockCandidateRepository.Verify(r => r.GetByEmailAsync(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task UpsertAsync_ShouldUpsertCandidateAndCacheIt()
    {
        // Arrange
        var candidate = new Candidate
        {
            Email = "test@example.com",
            FirstName = "Existing",
            LastName = "Last",
            Comments = ""
        };
        var upsertedCandidate = new Candidate
        {
            Email = "test@example.com",
            FirstName = "Existing",
            LastName = "Last",
            Comments = ""
        };
        _mockCandidateRepository.Setup(r => r.UpsertCandidateAsync(candidate))
            .ReturnsAsync(upsertedCandidate);

        // Act
        var result = await _candidateService.UpsertAsync(candidate);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(candidate.Email, result.Email);
        _mockCandidateRepository.Verify(r => r.UpsertCandidateAsync(candidate), Times.Once);
        _mockCachingService.Verify(c => c.SetValueAsync(candidate.Email, candidate, null), Times.Once);
    }

    [Fact]
    public async Task UpsertAsync_ShouldReturnNull_WhenCandidateUpsertFails()
    {
        // Arrange
        var candidate = new Candidate
        {
            Email = "test@example.com",
            FirstName = "Existing",
            LastName = "Last",
            Comments = ""
        };
        _mockCandidateRepository.Setup(r => r.UpsertCandidateAsync(candidate))
            .ReturnsAsync((Candidate?)null);

        // Act
        var result = await _candidateService.UpsertAsync(candidate);

        // Assert
        Assert.Null(result);
        _mockCandidateRepository.Verify(r => r.UpsertCandidateAsync(candidate), Times.Once);
        _mockCachingService.Verify(c => c.SetValueAsync(It.IsAny<string>(), It.IsAny<Candidate>(), null), Times.Never);
    }
}