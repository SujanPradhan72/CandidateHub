using CandidateHub.Data.Caching;
using CandidateHub.Data.Repos;
using CandidateHub.Data.Repos.IRepos;
using CandidateHub.Data.Services;
using CandidateHub.Data.Services.IServices;
using CandidateHub.Data.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace CandidateHub.Data;

public static class ServiceRegistration
{
    public static void ConfigureServices(this IServiceCollection services)
    {
        #region repos

        services.AddScoped<ICandidateRepository, CandidateRepository>();

        #endregion

        #region services

        services.AddScoped<ICachingService, LocalCachingService>();
        services.AddScoped<ICandidateService, CandidateService>();

        #endregion
        
        #region validators

        services.AddValidatorsFromAssemblyContaining<CandidateValidator>();

        #endregion
    }
}