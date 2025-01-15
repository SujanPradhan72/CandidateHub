using CandidateHub.Data.Validators;
using CandidateHub.Modules.DTOs.Request;
using FluentValidation.TestHelper;

namespace CandidateHub.Tests.Validators;

public class CandidateValidatorTest
{
    private readonly CandidateValidator _validator = new();

    [Fact]
    public void Should_Have_Error_When_Email_Is_Empty()
    {
        var candidateRequest = new CandidateRequest
        {
            Email = "",
            FirstName = "John",
            LastName = "Doe",
            Comments = "Some comments"
        };

        var result = _validator.TestValidate(candidateRequest);

        result.ShouldHaveValidationErrorFor(x => x.Email)
            .WithErrorMessage("Email is required");
    }

    [Fact]
    public void Should_Have_Error_When_FirstName_Is_Empty()
    {
        var candidateRequest = new CandidateRequest
        {
            Email = "john.doe@example.com",
            FirstName = "",
            LastName = "Doe",
            Comments = "Some comments"
        };

        var result = _validator.TestValidate(candidateRequest);

        result.ShouldHaveValidationErrorFor(x => x.FirstName)
            .WithErrorMessage("FirstName is required");
    }

    [Fact]
    public void Should_Have_Error_When_LastName_Is_Empty()
    {
        var candidateRequest = new CandidateRequest
        {
            Email = "john.doe@example.com",
            FirstName = "John",
            LastName = "",
            Comments = "Some comments"
        };

        var result = _validator.TestValidate(candidateRequest);

        result.ShouldHaveValidationErrorFor(x => x.LastName)
            .WithErrorMessage("LastName is required");
    }

    [Fact]
    public void Should_Have_Error_When_Comments_Are_Empty()
    {
        var candidateRequest = new CandidateRequest
        {
            Email = "john.doe@example.com",
            FirstName = "John",
            LastName = "Doe",
            Comments = ""
        };

        var result = _validator.TestValidate(candidateRequest);

        result.ShouldHaveValidationErrorFor(x => x.Comments)
            .WithErrorMessage("Comments is required");
    }

    [Fact]
    public void Should_Not_Have_Error_When_All_Fields_Are_Valid()
    {
        var candidateRequest = new CandidateRequest
        {
            Email = "john.doe@example.com",
            FirstName = "John",
            LastName = "Doe",
            Comments = "Some comments"
        };

        var result = _validator.TestValidate(candidateRequest);

        result.ShouldNotHaveAnyValidationErrors();
    }
}