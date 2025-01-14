using CandidateHub.Data.Repos.IRepos;
using CandidateHub.Data.Services.IServices;
using CandidateHub.Modules.Entities;

namespace CandidateHub.Data.Services;

public class CandidateService(ICandidateRepository candidateRepository): ICandidateService
{
    public async Task<Candidate?> GetByEmailAsync(string email)
    {
        return await candidateRepository.GetByEmailAsync(email);
    }

    public async Task<Candidate?> UpsertAsync(Candidate candidate)
    {
        return await candidateRepository.UpsertCandidateAsync(candidate);
    }
}