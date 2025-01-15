using CandidateHub.Data.Caching;
using CandidateHub.Data.Repos.IRepos;
using CandidateHub.Data.Services.IServices;
using CandidateHub.Modules.Entities;

namespace CandidateHub.Data.Services;

public class CandidateService(ICandidateRepository candidateRepository, ICachingService cachingService): ICandidateService
{
    public async Task<Candidate?> GetByEmailAsync(string email)
    {
        var cachedCandidate = await cachingService.GetValueAsync<Candidate>(email);
        if (cachedCandidate is not null) return cachedCandidate;
        
        //Cache candidate
        await cachingService.SetValueAsync(email, cachedCandidate);
        return await candidateRepository.GetByEmailAsync(email);
    }

    public async Task<Candidate?> UpsertAsync(Candidate candidate)
    {
        var record = await candidateRepository.UpsertCandidateAsync(candidate);
        
        //Upsert candidate Caching
        if (record is not null) 
            await cachingService.SetValueAsync(record.Email, candidate);
        
        return record;
    }
}