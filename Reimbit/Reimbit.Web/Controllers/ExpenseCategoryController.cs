using AegisInt.Core;
using Common.Data.Models;
using Common.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Reimbit.Application.Expenses.Categories;
using Reimbit.Contracts.ExpenseCategories;

namespace Reimbit.Web.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class ExpenseCategoryController(
    IExpenseCategoryService service,
    ICurrentUserProvider currentUserProvider
) : ApiController(currentUserProvider)
{
    [HttpPost("Insert")]
    [Produces<OperationResponse<EncryptedInt>>]
    [EndpointSummary("Create expense category")]
    public async Task<IActionResult> Insert([FromBody] InsertExpenseCategoriesRequest request)
    {
        var result = await service.Insert(request);
        return result.Match(_ => Ok(result.Value), Problem);
    }

    [HttpPost("SelectPage")]
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

    [HttpGet("SelectComboBox")]
    [Produces<List<OptionsResponse<EncryptedInt>>>]
    [EndpointSummary("Category dropdown")]
    public async Task<IActionResult> SelectComboBox()
    {
        var result = await service.SelectComboBox();
        return result.Match(_ => Ok(result.Value), Problem);
    }
}   