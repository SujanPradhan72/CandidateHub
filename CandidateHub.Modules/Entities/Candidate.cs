namespace CandidateHub.Modules.Entities;

public class Candidate
{
    public required string Email { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? CallTimeInterval { get; set; } // example 10am - 7pm
    public string? LinkedInProfileUrl { get; set; }
    public string? GithubProfileUrl { get; set; }
    public required string Comments { get; set; }
}