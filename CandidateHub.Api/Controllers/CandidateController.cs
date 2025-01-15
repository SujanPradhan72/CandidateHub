using CandidateHub.Data.Services.IServices;
using CandidateHub.Modules.Constants;
using CandidateHub.Modules.DTOs.Request;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace CandidateHub.Controllers;

[ApiController]
[Route("[controller]")]
public class CandidateController(ICandidateService candidateService, IValidator<CandidateRequest> validator) : ControllerBase
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
    public async Task<IActionResult> Post(CandidateRequest candidate)
    {
        var validRequest = await validator.ValidateAsync(candidate);
        if (!validRequest.IsValid)
        {
            validRequest.Errors.ToList().ForEach(error => ModelState.AddModelError("Errors", error.ErrorMessage));
            return BadRequest(ModelState);
        }
        var res = await candidateService.UpsertAsync(candidate);
        return Ok(res);
    }
}