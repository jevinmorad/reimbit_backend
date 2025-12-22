using AegisInt.Core;
using Common.Data.Models;
using Common.Security;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Reimbit.Application.Expenses;
using Reimbit.Contracts.Expenses;

namespace Reimbit.Web.Controller;

[ApiController]
[Route("api/[controller]")]
public class ExpenseController(
    IExpenseService service,
    ICurrentUserProvider currentUserProvider,
    IValidator<InsertRequest> insertRequestValidator,
    IValidator<UpdateRequest> updateRequestValidator
) : ApiController(currentUserProvider)
{
    [HttpPost]
    [Produces<OperationResponse<EncryptedInt>>]
    [EndpointSummary("Create expense")]
    public async Task<IActionResult> Insert([FromBody] InsertRequest request)
    {
        await insertRequestValidator.ValidateAndThrowAsync(request);
        var result = await service.Insert(request);
        return result.Match(_ => Ok(result.Value), Problem);
    }

    [HttpGet]
    [Produces<PagedResult<ListResponse>>]
    [EndpointSummary("My expenses list")]
    public async Task<IActionResult> List()
    {
        var result = await service.List();
        return result.Match(_ => Ok(result.Value), Problem);
    }

    [HttpPut]
    [Produces<OperationResponse<EncryptedInt>>]
    [EndpointSummary("Update expense")]
    public async Task<IActionResult> Update([FromBody] UpdateRequest request)
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
    [Produces<GetResponse>]
    [EndpointSummary("Get expense details")]
    public async Task<IActionResult> Get(EncryptedInt id)
    {
        var result = await service.Get(id);
        return result.Match(_ => Ok(result.Value), Problem);
    }
}
