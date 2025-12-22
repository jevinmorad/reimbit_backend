using AegisInt.Core;
using Common.Data.Models;
using Common.Security;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Reimbit.Application.Expenses.Expense;
using Reimbit.Contracts.Expenses;

namespace Reimbit.Web.Controller;

[ApiController]
[Route("api/[controller]")]
public class ExpenseController(
    IExpenseService service,
    ICurrentUserProvider currentUserProvider,
    IValidator<InsertExpenseRequest> insertRequestValidator,
    IValidator<UpdateExpenseRequest> updateRequestValidator
) : ApiController(currentUserProvider)
{
    [HttpPost]
    [Produces<OperationResponse<EncryptedInt>>]
    [EndpointSummary("Create expense")]
    public async Task<IActionResult> Insert([FromBody] InsertExpenseRequest request)
    {
        await insertRequestValidator.ValidateAndThrowAsync(request);
        var result = await service.Insert(request);
        return result.Match(_ => Ok(result.Value), Problem);
    }

    [HttpGet("my-expenses")]
    [Produces<PagedResult<ListExpensesResponse>>]
    [EndpointSummary("My expenses list")]
    public async Task<IActionResult> ListMyExpenses()
    {
        var result = await service.ListMyExpenses();
        return result.Match(_ => Ok(result.Value), Problem);
    }

    [HttpGet("user/{userId}")]
    [Produces<PagedResult<ListExpensesResponse>>]
    [EndpointSummary("Expenses by user")]
    public async Task<IActionResult> ListByUserId(EncryptedInt userId)
    {
        var result = await service.ListByUserId(userId);
        return result.Match(_ => Ok(result.Value), Problem);
    }

    [HttpGet("project/{projectId}")]
    [Produces<PagedResult<ListExpensesResponse>>]
    [EndpointSummary("Expenses by project")]
    public async Task<IActionResult> ListByProject(EncryptedInt projectId)
    {
        var result = await service.ListByProject(projectId);
        return result.Match(_ => Ok(result.Value), Problem);
    }

    [HttpGet("organization")]
    [Produces<PagedResult<ListExpensesResponse>>]
    [EndpointSummary("Expenses by organization")]
    public async Task<IActionResult> ListByOrganization()
    {
        var result = await service.ListByOrganization();
        return result.Match(_ => Ok(result.Value), Problem);
    }

    [HttpPut]
    [Produces<OperationResponse<EncryptedInt>>]
    [EndpointSummary("Update expense")]
    public async Task<IActionResult> Update([FromBody] UpdateExpenseRequest request)
    {
        await updateRequestValidator.ValidateAndThrowAsync(request);
        var result = await service.Update(request);
        return result.Match(_ => Ok(result.Value), Problem);
    }

    [HttpDelete("{id}")]
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
