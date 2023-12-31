using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Persistence.DataBaseContexts;
using HR.LeaveManagement.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HR.LeaveManagement.Persistence;

public static class PersistenceServiceRegistration
{
    public static IServiceCollection AddPersistenceServices(
        this IServiceCollection services, IConfiguration configuration, bool isDevelopment
    )
    {
        if (isDevelopment)
        {
            services.AddDbContext<HRDatabaseContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("HRLeaveManagementConnectionStringDevelopment"));
            });
        } else
        {
            services.AddDbContext<HRDatabaseContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("HRLeaveManagementConnectionStringProduction"));
            });
        }
       
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<ILeaveTypeRepository, LeaveTypeRepository>();
        services.AddScoped<ILeaveAllocationRepository, LeaveAllocationRepository>();
        services.AddScoped<ILeaveRequestRepository, LeaveRequestsRepository>();
        return services;
    }
}