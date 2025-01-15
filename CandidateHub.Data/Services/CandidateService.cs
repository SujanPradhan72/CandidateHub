using CandidateHub.Data.Caching;
using CandidateHub.Data.Repos.IRepos;
using CandidateHub.Data.Services.IServices;
using CandidateHub.Modules.DTOs.Request;
using CandidateHub.Modules.DTOs.Response;
using CandidateHub.Modules.Entities;
using Mapster;

namespace CandidateHub.Data.Services;

public class CandidateService(ICandidateRepository candidateRepository, ICachingService cachingService): ICandidateService
{
    public async Task<CandidateResponse?> GetByEmailAsync(string email)
    {
        var cachedCandidate = await cachingService.GetValueAsync<CandidateResponse>(email);
        if (cachedCandidate is not null) return cachedCandidate;
        
        //Cache candidate
        var res = await candidateRepository.GetByEmailAsync(email);
        await cachingService.SetValueAsync(email, res.Adapt<CandidateResponse>());
        return res.Adapt<CandidateResponse>();
    }

    public async Task<CandidateResponse?> UpsertAsync(CandidateRequest candidate)
    {
        var model = candidate.Adapt<Candidate>();
        var record = await candidateRepository.UpsertCandidateAsync(model);
        
        //Upsert candidate Caching
        if (record is not null) 
            await cachingService.SetValueAsync(record.Email, candidate.Adapt<CandidateResponse>());
        
        return candidate.Adapt<CandidateResponse>();
    }
}