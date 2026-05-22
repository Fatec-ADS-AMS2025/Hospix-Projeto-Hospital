using Hospital.Application.Alerts;
using Hospital.Application.UseCases;
using Microsoft.Extensions.DependencyInjection;

namespace Hospital.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddHospitalApplication(this IServiceCollection services)
    {
        services.AddScoped<PatientUseCases>();
        services.AddScoped<BedUseCases>();
        services.AddScoped<AdmissionUseCases>();
        services.AddScoped<CreatePrescriptionUseCase>();
        services.AddScoped<PrescriptionUseCases>();
        services.AddScoped<ExamUseCases>();
        services.AddScoped<GenerateAlertsUseCase>();
        services.AddScoped<DashboardUseCase>();
        services.AddScoped<InvoiceUseCases>();

        services.AddScoped<IAlertRule, BedOccupancyAlertRule>();
        services.AddScoped<IAlertRule, PendingExamAlertRule>();
        services.AddScoped<IAlertRule, ActivePrescriptionAlertRule>();
        services.AddScoped<IAlertRule, BlockedDischargeAlertRule>();

        return services;
    }
}
