using AegisInt.Core;
using Common.Data.Models;
using Common.Security;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Reimbit.Application.EmployeeManagement.Employee;
using Reimbit.Contracts.Employee;

namespace Reimbit.Web.Controller;

[ApiController]
[Route("api/[controller]")]
public class EmployeeController(
    ICurrentUserProvider currentUserProvider,
    IEmployeeService employeeService
) : ApiController(currentUserProvider)
{
    [HttpPost]
    [Produces<OperationResponse<EncryptedInt>>]
    [EndpointSummary("Insert")]
    public async Task<IActionResult> Insert([FromBody] InsertEmployeeRequest request)
    {
        var result = await employeeService.Insert(request);

        return result.Match(_ => Ok(result.Value), Problem);
    }

    [HttpGet]
    [Produces<PagedResult<ListEmployeeResponse>>]
    [EndpointSummary("List all employees")]
    public async Task<IActionResult> List()
    {
        var result = await employeeService.List();

        return result.Match(_ => Ok(result.Value), Problem);
    }
}
