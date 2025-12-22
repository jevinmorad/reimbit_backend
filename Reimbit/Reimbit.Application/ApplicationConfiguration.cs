using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Reimbit.Application.EmployeeManagement.Employee;
using Reimbit.Application.Expenses.Categories;
using Reimbit.Application.Expenses.Expense;
using Reimbit.Application.Jwt;
using Reimbit.Application.Projects.Policies;
using Reimbit.Application.Projects.Project;
using Reimbit.Application.Security.Account;
using System.Reflection;

namespace Reimbit.Application;

public static class ApplicationConfiguration
{
    public static IServiceCollection AddApplicationServices (
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddAutoMapper(typeof(ApplicationConfiguration).Assembly);

        // Automatically register all validators in this assembly
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddFluentValidationAutoValidation();

        // JWT service
        services.AddScoped<IJwtTokenService, JwtTokenService>();

        // Application services
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IEmployeeService, EmployeeService>();
        services.AddScoped<IExpenseService, ExpenseService>();
        services.AddScoped<IExpenseCategoryService, ExpenseCategoryService>();
        services.AddScoped<IPoliciesService, PoliciesService>();
        services.AddScoped<IProjectService, ProjectService>();

        return services;
    }
}
