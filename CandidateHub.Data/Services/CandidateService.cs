using CandidateHub.Data.Repos.IRepos;
using CandidateHub.Data.Services.IServices;
using CandidateHub.Modules.Entities;

namespace CandidateHub.Data.Services;

public class CandidateService(ICandidateRepository candidateRepository): ICandidateService
{
    public async Task<Candidate?> GetByEmailAsync(string email)
    {
        //TODO implement caching check
        return await candidateRepository.GetByEmailAsync(email);
    }

    public async Task<Candidate?> UpsertAsync(Candidate candidate)
    {
        //TODO implement caching check
        return await candidateRepository.UpsertCandidateAsync(candidate);
    }
}