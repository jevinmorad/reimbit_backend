using AegisInt.Core;
using Common.Data.Models;
using Common.Security;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Reimbit.Application.Projects.Policies;
using Reimbit.Contracts.Policies;

namespace Reimbit.Web.Controller;

[ApiController]
[Route("/api/[controller]")]
public class PoliciesController(
    IPoliciesService service,
    ICurrentUserProvider currentUserProvider,
    IValidator<InsertPolicyRequest> insertValidator,
    IValidator<UpdatePolicyRequest> updateValidator
) : ApiController(currentUserProvider)
{
    [HttpPost]
    [Produces<OperationResponse<EncryptedInt>>]
    [EndpointSummary("Create policy")]
    public async Task<IActionResult> Insert([FromBody] InsertPolicyRequest request)
    {
        await insertValidator.ValidateAndThrowAsync(request);
        var result = await service.Insert(request);
        return result.Match(_ => Ok(result.Value), Problem);
    }

    [HttpGet("list/{projectId}")]
    [Produces<PagedResult<ListPoliciesResponse>>]
    [EndpointSummary("List policies by project")]
    public async Task<IActionResult> List(EncryptedInt projectId)
    {
        var result = await service.List(projectId);
        return result.Match(_ => Ok(result.Value), Problem);
    }

    [HttpPut]
    [Produces<OperationResponse<EncryptedInt>>]
    [EndpointSummary("Update policy")]
    public async Task<IActionResult> Update([FromBody] UpdatePolicyRequest request)
    {
        await updateValidator.ValidateAndThrowAsync(request);
        var result = await service.Update(request);
        return result.Match(_ => Ok(result.Value), Problem);
    }

    [HttpDelete("{id}")]
    [Produces<OperationResponse<EncryptedInt>>]
    [EndpointSummary("Delete policy")]
    public async Task<IActionResult> Delete(EncryptedInt id)
    {
        var result = await service.Delete(id);
        return result.Match(_ => Ok(result.Value), Problem);
    }

    [HttpGet("{id}")]
    [Produces<GetPolicyResponse>]
    [EndpointSummary("Get policy")]
    public async Task<IActionResult> Get(EncryptedInt id)
    {
        var result = await service.Get(id);
        return result.Match(_ => Ok(result.Value), Problem);
    }
}