namespace CandidateHub.Modules.DTOs.Request;

public class CandidateRequest
{
    public required string Email { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? CallTimeInterval { get; set; }
    public string? LinkedInProfileUrl { get; set; }
    public string? GithubProfileUrl { get; set; }
    public required string Comments { get; set; }
}