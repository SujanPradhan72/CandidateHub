using CandidateHub.Modules.DTOs.Request;

namespace CandidateHub.Tests.Modules.DTOs.Request;

public class CandidateRequestTest
{
    [Fact]
    public void Candidate_Should_Have_Required_Properties_Set()
    {
        // Arrange
        var candidate = new CandidateRequest
        {
            Email = "test@example.com",
            FirstName = "John",
            LastName = "Doe",
            Comments = "Looking forward to the interview",
            CallTimeInterval = "1",
            GithubProfileUrl = "2",
            LinkedInProfileUrl = "3"
        };

        // Act & Assert
        Assert.Equal("test@example.com", candidate.Email);
        Assert.Equal("John", candidate.FirstName);
        Assert.Equal("Doe", candidate.LastName);
        Assert.Equal("Looking forward to the interview", candidate.Comments);
        Assert.Equal("1", candidate.CallTimeInterval);
        Assert.Equal("2", candidate.GithubProfileUrl);
        Assert.Equal("3", candidate.LinkedInProfileUrl);
    }

    [Fact]
    public void Candidate_Should_Accept_Nullable_Properties()
    {
        // Arrange
        var candidate = new CandidateRequest
        {
            Email = "test@example.com",
            FirstName = "John",
            LastName = "Doe",
            Comments = "Looking forward to the interview",
            PhoneNumber = null, // nullable property
            CallTimeInterval = null // nullable property
        };

        // Act & Assert
        Assert.Null(candidate.PhoneNumber);
        Assert.Null(candidate.CallTimeInterval);
    }
}