using CandidateHub.Modules.Entities;

namespace CandidateHub.Data.Repos.IRepos;

public interface ICandidateRepository
{
    Task<Candidate?> GetByEmailAsync(string email);
    Task<Candidate?> UpsertCandidateAsync(Candidate candidate);
}