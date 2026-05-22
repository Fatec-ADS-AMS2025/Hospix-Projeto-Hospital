using Hospital.Application.Interfaces;
using Hospital.Infrastructure.Persistence;
using Hospital.Infrastructure.Repositories;
using Hospital.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hospital.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddHospitalInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<HospitalDbContext>(options =>
        {
            options.UseSqlite(configuration.GetConnectionString("Hospital") ?? "Data Source=hospital-engineered.db");
        });

        services.AddScoped<EfHospitalRepository>();
        services.AddScoped<IPatientRepository>(provider => provider.GetRequiredService<EfHospitalRepository>());
        services.AddScoped<IBedRepository>(provider => provider.GetRequiredService<EfHospitalRepository>());
        services.AddScoped<IAdmissionRepository>(provider => provider.GetRequiredService<EfHospitalRepository>());
        services.AddScoped<IPrescriptionRepository>(provider => provider.GetRequiredService<EfHospitalRepository>());
        services.AddScoped<IExamRepository>(provider => provider.GetRequiredService<EfHospitalRepository>());
        services.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<EfHospitalRepository>());
        services.AddScoped<IClock, SystemClock>();

        services.AddHttpClient<IInvoiceGateway, HttpBillingGateway>(client =>
        {
            client.BaseAddress = new Uri(configuration["BillingService:BaseUrl"] ?? "http://localhost:5102");
        });

        return services;
    }
}
