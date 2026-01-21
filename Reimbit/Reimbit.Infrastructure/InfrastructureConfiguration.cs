using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Reimbit.Application.Audit;
using Reimbit.Domain.Interfaces;
using Reimbit.Domain.Models;
using Reimbit.Domain.Repositories;
using Reimbit.Infrastructure.Audit;
using Reimbit.Infrastructure.Data;
using Reimbit.Infrastructure.Repositories;

namespace Reimbit.Infrastructure;

public static class InfrastructureConfiguration
{
    public static IServiceCollection AddInfrastructureService(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

        services.AddScoped<IApplicationDbContext>(sp => sp.GetRequiredService<ApplicationDbContext>());

        services.AddScoped<IPasswordHasher<SecUser>, PasswordHasher<SecUser>>();

        // Audit
        services.AddScoped<IAuditLogger, DbAuditLogger>();

        // Repositories
        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<IExpenseRepository, ExpenseRepository>();
        services.AddScoped<IExpenseCategoryRepository, ExpenseCategoryRepository>();
        services.AddScoped<IPoliciesRepository, PoliciesRepository>();
        services.AddScoped<IProjectRepository, ProjectRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IExpenseReportRepository, ExpenseReportRepository>();
        services.AddScoped<IApprovalRepository, ApprovalRepository>();
        services.AddScoped<IFinanceRepository, FinanceRepository>();
        services.AddScoped<IDelegationRepository, DelegationRepository>();

        return services;
    }
}
