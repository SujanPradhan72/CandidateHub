using CandidateHub.Modules.DTOs.Request;
using CandidateHub.Modules.DTOs.Response;

namespace CandidateHub.Data.Services.IServices;

public interface ICandidateService
{
    Task<CandidateResponse?> GetByEmailAsync(string email);
    Task<CandidateResponse?> UpsertAsync(CandidateRequest candidate);
}