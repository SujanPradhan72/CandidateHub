using CandidateHub.Controllers;
using CandidateHub.Data.Services.IServices;
using CandidateHub.Modules.Constants;
using CandidateHub.Modules.Entities;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CandidateHub.Tests.Controllers;

public class CandidateControllerTest
{
    private readonly Mock<ICandidateService> _mockCandidateService;
    private readonly CandidateController _controller;

    public CandidateControllerTest()
    {
        _mockCandidateService = new Mock<ICandidateService>();
        _controller = new CandidateController(_mockCandidateService.Object);
    }

    [Fact]
    public async Task GetCandidate_ReturnsBadRequest_WhenCandidateNotFound()
    {
        // Arrange
        var email = "test@example.com";
        _mockCandidateService.Setup(service => service.GetByEmailAsync(email)).ReturnsAsync((Candidate)null!);

        // Act
        var result = await _controller.GetCandidate(email);

        // Assert
        var actionResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(ExceptionConstants.CandidateNotFoundException, actionResult.Value);
    }

    [Fact]
    public async Task GetCandidate_ReturnsOkResult_WhenCandidateFound()
    {
        // Arrange
        var email = "new@example.com";
        var candidate = new Candidate
        {
            Email = "new@example.com", FirstName = "Jane Doe", LastName = "Smith",
            Comments = "Looking forward to the interview"
        };
        _mockCandidateService.Setup(service => service.GetByEmailAsync(email)).ReturnsAsync(candidate);

        // Act
        var result = await _controller.GetCandidate(email);

        // Assert
        var actionResult = Assert.IsType<OkObjectResult>(result);
        var returnedCandidate = Assert.IsType<Candidate>(actionResult.Value);
        Assert.Equal(email, returnedCandidate.Email);
    }

    [Fact]
    public async Task Post_ReturnsOkResult_WhenCandidateUpserted()
    {
        // Arrange
        var candidate = new Candidate
        {
            Email = "new@example.com", FirstName = "Jane Doe", LastName = "Smith",
            Comments = "Looking forward to the interview"
        };
        _mockCandidateService.Setup(service => service.UpsertAsync(candidate)).ReturnsAsync(candidate);

        // Act
        var result = await _controller.Post(candidate);

        // Assert
        var actionResult = Assert.IsType<OkObjectResult>(result);
        var returnedCandidate = Assert.IsType<Candidate>(actionResult.Value);
        Assert.Equal(candidate.Email, returnedCandidate.Email);
    }
}