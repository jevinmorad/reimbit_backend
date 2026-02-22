using AegisInt.Core;
using Common.Data.Models;
using Common.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Reimbit.Application.Projects.Policies;
using Reimbit.Contracts.Policies;

namespace Reimbit.Web.Controllers;

[ApiController]
[Authorize]
[Route("/api/[controller]")]
public class PoliciesController(
    IPoliciesService service,
    ICurrentUserProvider currentUserProvider
) : ApiController(currentUserProvider)
{
    [HttpPost]
    [Produces<OperationResponse<EncryptedInt>>]
    [EndpointSummary("Create policy")]
    public async Task<IActionResult> Insert([FromBody] PolicyInsertRequest request)
    {
        var result = await service.Insert(request);
        return result.Match(_ => Ok(result.Value), Problem);
    }

    [HttpGet("list/{categoryId}")]
    [Produces<PagedResult<PoliciesSelectPageResponse>>]
    [EndpointSummary("List policies by category")]
    public async Task<IActionResult> List([FromRoute] EncryptedInt categoryId)
    {
        var result = await service.List(categoryId);
        return result.Match(_ => Ok(result.Value), Problem);
    }

    [HttpPut]
    [Produces<OperationResponse<EncryptedInt>>]
    [EndpointSummary("Update policy")]
    public async Task<IActionResult> Update([FromBody] PolicyUpdateRequest request)
    {
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
    [Produces<PolicySelectPKResponse>]
    [EndpointSummary("Get policy")]
    public async Task<IActionResult> Get(EncryptedInt id)
    {
        var result = await service.Get(id);
        return result.Match(_ => Ok(result.Value), Problem);
    }
}