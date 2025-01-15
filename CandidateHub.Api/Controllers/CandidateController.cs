using CandidateHub.Data.Services.IServices;
using CandidateHub.Modules.Constants;
using CandidateHub.Modules.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CandidateHub.Controllers;

[ApiController]
[Route("[controller]")]
public class CandidateController(ICandidateService candidateService) : ControllerBase
{
    [HttpGet("{email}")]
    public async Task<IActionResult> GetCandidate(string email)
    {
        var res = await candidateService.GetByEmailAsync(email);

        if (res is null)
            return BadRequest(ExceptionConstants.CandidateNotFoundException);
        return Ok(res);
    }

    [HttpPost]
    public async Task<IActionResult> Post(Candidate candidate)
    {
        var res = await candidateService.UpsertAsync(candidate);
        return Ok(res);
    }
}