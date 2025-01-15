using CandidateHub.Data.Caching;
using CandidateHub.Data.Repos.IRepos;
using CandidateHub.Data.Services;
using CandidateHub.Modules.DTOs.Request;
using CandidateHub.Modules.DTOs.Response;
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
        var cachedCandidate = new CandidateResponse
        {
            Email = email,
            FirstName = "Existing",
            LastName = "Last",
            Comments = ""
        };
        _mockCachingService.Setup(c => c.GetValueAsync<CandidateResponse>(email))
            .ReturnsAsync(cachedCandidate);

        // Act
        var result = await _candidateService.GetByEmailAsync(email);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(email, result.Email);
        _mockCandidateRepository.Verify(r => r.GetByEmailAsync(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task GetByEmailAsync_CandidateNotInCache_FetchesFromRepositoryAndCachesResult()
    {
        // Arrange
        var email = "test@example.com";
        var candidateFromRepo = new Candidate
        {
            Email = email, FirstName = "Existing",
            LastName = "Last",
            Comments = ""
        };
        var candidateResponse = new CandidateResponse
        {
            Email = email, FirstName = "Existing",
            LastName = "Last",
            Comments = ""
        };

        _mockCachingService.Setup(c => c.GetValueAsync<CandidateResponse>(email))
            .ReturnsAsync((CandidateResponse)null!);
        _mockCandidateRepository.Setup(r => r.GetByEmailAsync(email))
            .ReturnsAsync(candidateFromRepo);

        // Act
        var result = await _candidateService.GetByEmailAsync(email);

        // Assert
        Assert.Equal(candidateResponse.Email, result!.Email);
        _mockCachingService.Verify(c => c.SetValueAsync(email, candidateFromRepo, null), Times.Once); // Verify caching
    }

    [Fact]
    public async Task UpsertAsync_ShouldUpsertCandidateAndCacheIt()
    {
        // Arrange
        var candidate = new Candidate()
        {
            Email = "test@example.com",
            FirstName = "Existing",
            LastName = "Last",
            Comments = ""
        };
        var candidateReq = new CandidateRequest()
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
        var result = await _candidateService.UpsertAsync(candidateReq);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(candidate.Email, result.Email);
    }
}