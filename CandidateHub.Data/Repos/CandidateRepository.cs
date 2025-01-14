using CandidateHub.Data.Repos.IRepos;
using CandidateHub.Modules.Entities;

namespace CandidateHub.Data.Repos;

public class CandidateRepository(AppDbContext context) : BaseRepository<Candidate>(context), ICandidateRepository
{
    public async Task<Candidate?> GetByEmailAsync(string email)
    {
        return await GetByStringIdAsync(email);
    }

    public async Task<Candidate?> UpsertCandidateAsync(Candidate candidate)
    {
        return await UpsertAsync(candidate, candidate.Email);
    }
}