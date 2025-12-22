using AegisInt.Core;
using Common.Data.Models;
using Common.Security;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Reimbit.Application.Expenses.Categories;
using Reimbit.Contracts.ExpenseCategories;

namespace Reimbit.Web.Controller;

[ApiController]
[Route("api/[controller]")]
public class ExpenseCategoryController(
    IExpenseCategoryService service,
    ICurrentUserProvider currentUserProvider,
    IValidator<InsertExpenseCategoriesRequest> insertRequestValidator,
    IValidator<UpdateExpenseCategoriesRequest> updateRequestValidator
) : ApiController(currentUserProvider)
{
    [HttpPost]
    [Produces<OperationResponse<EncryptedInt>>]
    [EndpointSummary("Create expense category")]
    public async Task<IActionResult> Insert([FromBody] InsertExpenseCategoriesRequest request)
    {
        await insertRequestValidator.ValidateAndThrowAsync(request);
        var result = await service.Insert(request);
        return result.Match(_ => Ok(result.Value), Problem);
    }

    [HttpGet]
    [Produces<PagedResult<ListExpenseCategoriesResponse>>]
    [EndpointSummary("List expense categories")]
    public async Task<IActionResult> List()
    {
        var result = await service.List();
        return result.Match(_ => Ok(result.Value), Problem);
    }

    [HttpPut]
    [Produces<OperationResponse<EncryptedInt>>]
    [EndpointSummary("Update expense category")]
    public async Task<IActionResult> Update([FromBody] UpdateExpenseCategoriesRequest request)
    {
        await updateRequestValidator.ValidateAndThrowAsync(request);
        var result = await service.Update(request);
        return result.Match(_ => Ok(result.Value), Problem);
    }

    [HttpDelete("{id}")]
    [Produces<OperationResponse<EncryptedInt>>]
    [EndpointSummary("Delete expense category")]
    public async Task<IActionResult> Delete(EncryptedInt id)
    {
        var result = await service.Delete(id);
        return result.Match(_ => Ok(result.Value), Problem);
    }

    [HttpGet("{id}")]
    [Produces<GetExpenseCategoriesResponse>]
    [EndpointSummary("Get expense category details")]
    public async Task<IActionResult> Get(EncryptedInt id)
    {
        var result = await service.Get(id);
        return result.Match(_ => Ok(result.Value), Problem);
    }
}