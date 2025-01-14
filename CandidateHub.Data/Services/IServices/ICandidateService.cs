using CandidateHub.Modules.Entities;

namespace CandidateHub.Data.Services.IServices;

public interface ICandidateService
{
    Task<Candidate?> GetByEmailAsync(string email);
    Task<Candidate?> UpsertAsync(Candidate candidate);
}