using CIDERS.Domain.Core.Repository.Cider;

namespace CIDERS.Domain.Injector;

public static class CiderRepositoryInjector
{
    public static IServiceCollection RegisterDependenciesCider(this IServiceCollection services, int i = 0)
    {
        services.AddScoped<IAuditLogRepository, AuditLogRepository>();
        services.AddScoped<IApiUserRepository, ApiUserRepository>();
        services.AddScoped<IApiPermissionRepository, ApiPermissionRepository>();
        services.AddScoped<IApiUserPermissionRepository, ApiUserPermissionRepository>();
        services.AddScoped<IBranchRepository, BranchRepository>();
        services.AddScoped<IRankRepository, ApiRankRepository>();
        services.AddScoped<IDepartmentRepository, ApiDepartmentRepository>();
        services.AddScoped<ILocationRespository, ApiLocationRepository>();
        services.AddScoped<IEmployeeRepository, ApiEmployeeRepository>();

        return services;
    }
}
