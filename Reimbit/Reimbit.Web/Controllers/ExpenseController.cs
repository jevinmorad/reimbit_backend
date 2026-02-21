using AegisInt.Core;
using Common.Data.Models;
using Common.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Reimbit.Application.Expenses.Expense;
using Reimbit.Contracts.Expenses;
using Reimbit.Core.Common.Permissions;

namespace Reimbit.Web.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class ExpenseController(
    IExpenseService service,
    ICurrentUserProvider currentUserProvider
) : ApiController(currentUserProvider)
{
    [HttpPost("Insert")]
    [Produces<OperationResponse<EncryptedInt>>]
    [EndpointSummary("Create expense")]
    [HasPermission(Permission.ExpenseCreate)]
    public async Task<IActionResult> Insert([FromBody] InsertExpenseRequest request)
    {
        var result = await service.Insert(request);
        return result.Match(_ => Ok(result.Value), Problem);
    }

    [HttpPost("SelectPage")]
    [Produces<PagedResult<ListExpensesResponse>>]
    [EndpointSummary("My expenses list")]
    public async Task<IActionResult> SelectPaage(ListExpenseRequest request)
    {
        var result = await service.SelectPaage(request);
        return result.Match(_ => Ok(result.Value), Problem);
    }

    [HttpPut("Update")]
    [Produces<OperationResponse<EncryptedInt>>]
    [EndpointSummary("Update expense")]
    public async Task<IActionResult> Update([FromBody] UpdateExpenseRequest request)
    {
        var result = await service.Update(request);
        return result.Match(_ => Ok(result.Value), Problem);
    }

    [HttpDelete("Delete/{id}")]
    [Produces<OperationResponse<EncryptedInt>>]
    [EndpointSummary("Delete expense")]
    public async Task<IActionResult> Delete(EncryptedInt id)
    {
        var result = await service.Delete(id);
        return result.Match(_ => Ok(result.Value), Problem);
    }

    [HttpGet("{id}")]
    [Produces<GetExpenseResponse>]
    [EndpointSummary("Get expense details")]
    public async Task<IActionResult> Get(EncryptedInt id)
    {
        var result = await service.Get(id);
        return result.Match(_ => Ok(result.Value), Problem);
    }

    [HttpGet("view/{id}")]
    [Produces<ViewExpenseResponse>]
    [EndpointSummary("View full expense details")]
    public async Task<IActionResult> View(EncryptedInt id)
    {
        var result = await service.View(id);
        return result.Match(_ => Ok(result.Value), Problem);
    }
}
