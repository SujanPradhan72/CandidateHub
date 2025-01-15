using CandidateHub.Controllers;
using CandidateHub.Data.Services.IServices;
using CandidateHub.Modules.Constants;
using CandidateHub.Modules.DTOs.Request;
using CandidateHub.Modules.DTOs.Response;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CandidateHub.Tests.Controllers;

public class CandidateControllerTest
{
    private readonly Mock<ICandidateService> _mockCandidateService;
    private readonly Mock<IValidator<CandidateRequest>> _mockValidator;
    private readonly CandidateController _controller;

    public CandidateControllerTest()
    {
        _mockCandidateService = new Mock<ICandidateService>();
        _mockValidator = new Mock<IValidator<CandidateRequest>>();
        _controller = new CandidateController(_mockCandidateService.Object, _mockValidator.Object);
    }

    [Fact]
    public async Task GetCandidate_ReturnsBadRequest_WhenCandidateNotFound()
    {
        // Arrange
        var email = "test@example.com";
        _mockCandidateService.Setup(service => service.GetByEmailAsync(email)).ReturnsAsync((CandidateResponse)null!);

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
        var candidate = new CandidateResponse
        {
            Email = "new@example.com", FirstName = "Jane Doe", LastName = "Smith",
            Comments = "Looking forward to the interview"
        };
        _mockCandidateService.Setup(service => service.GetByEmailAsync(email)).ReturnsAsync(candidate);

        // Act
        var result = await _controller.GetCandidate(email);

        // Assert
        var actionResult = Assert.IsType<OkObjectResult>(result);
        var returnedCandidate = Assert.IsType<CandidateResponse>(actionResult.Value);
        Assert.Equal(email, returnedCandidate.Email);
    }

    [Fact]
    public async Task Post_ReturnsOkResult_WhenCandidateUpserted()
    {
        // Arrange
        var candidate = new CandidateRequest
        {
            Email = "new@example.com", FirstName = "Jane Doe", LastName = "Smith",
            Comments = "Looking forward to the interview"
        };
        
        var candidateResponse = new CandidateResponse
        {
            Email = "new@example.com", FirstName = "Jane Doe", LastName = "Smith",
            Comments = "Looking forward to the interview"
        };
        _mockCandidateService.Setup(service => service.UpsertAsync(candidate)).ReturnsAsync(candidateResponse);

        // Act
        var result = await _controller.Post(candidate);

        // Assert
        var actionResult = Assert.IsType<OkObjectResult>(result);
        var returnedCandidate = Assert.IsType<CandidateResponse>(actionResult.Value);
        Assert.Equal(candidate.Email, returnedCandidate.Email);
    }
}