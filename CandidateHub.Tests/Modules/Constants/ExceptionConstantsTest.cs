using CandidateHub.Modules.Constants;

namespace CandidateHub.Tests.Modules.Constants;

public class ExceptionConstantsTest
{
    [Fact]
    public void CandidateNotFoundException_ShouldReturnExpectedString()
    {
        // Arrange
        var expected = "Candidate Not Found";

        // Act
        var actual = ExceptionConstants.CandidateNotFoundException;

        // Assert
        Assert.Equal(expected, actual);
    }
}