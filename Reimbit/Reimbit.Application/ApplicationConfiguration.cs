using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Reimbit.Application.Approvals;
using Reimbit.Application.Approvals.Delegation;
using Reimbit.Application.EmployeeManagement.Employee;
using Reimbit.Application.EmployeeManagement.Role;
using Reimbit.Application.Expenses.Categories;
using Reimbit.Application.Expenses.Expense;
using Reimbit.Application.Jwt;
using Reimbit.Application.Projects.Policies;
using Reimbit.Application.Projects.Project;
using Reimbit.Application.Security.Account;
using Reimbit.Application.Finance;
using System.Reflection;

namespace Reimbit.Application;

public static class ApplicationConfiguration
{
    public static IServiceCollection AddApplicationServices (
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Automatically register all validators in this assembly
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddFluentValidationAutoValidation();

        // JWT service
        services.AddScoped<IJwtTokenService, JwtTokenService>();

        // Application services
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IEmployeeService, EmployeeService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IExpenseService, ExpenseService>();
        services.AddScoped<IExpenseCategoryService, ExpenseCategoryService>();
        services.AddScoped<IPoliciesService, PoliciesService>();
        services.AddScoped<IProjectService, ProjectService>();
        services.AddScoped<IApprovalService, ApprovalService>();
        services.AddScoped<IDelegationService, DelegationService>();
        services.AddScoped<IFinanceService, FinanceService>();

        return services;
    }
}
