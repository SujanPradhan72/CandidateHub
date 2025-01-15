using CandidateHub.Modules.DTOs.Request;
using FluentValidation;

namespace CandidateHub.Data.Validators;

public class CandidateValidator : AbstractValidator<CandidateRequest>
{
    public CandidateValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required")
            .EmailAddress()
            .WithMessage("Invalid Email");
        
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage("FirstName is required");
        
        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage("LastName is required");
        
        RuleFor(x => x.Comments)
            .NotEmpty()
            .WithMessage("Comments is required");
        
        RuleFor(x => x.GithubProfileUrl)
            .Must(url => string.IsNullOrWhiteSpace(url) || Uri.IsWellFormedUriString(url, UriKind.Absolute))
            .WithMessage("Please enter a valid GithubProfileUrl");
        
        RuleFor(x => x.LinkedInProfileUrl)
            .Must(url => string.IsNullOrWhiteSpace(url) || Uri.IsWellFormedUriString(url, UriKind.Absolute))
            .WithMessage("Please enter a valid LinkedInProfileUrl");
    }
}