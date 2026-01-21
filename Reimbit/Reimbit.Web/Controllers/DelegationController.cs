using AegisInt.Core;
using Common.Data.Models;
using Common.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Reimbit.Application.Approvals.Delegation;
using Reimbit.Contracts.Approvals.Delegation;

namespace Reimbit.Web.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public sealed class DelegationController(
    ICurrentUserProvider currentUserProvider,
    IDelegationService service
) : ApiController(currentUserProvider)
{
    public sealed class RevokeBody
    {
        public required string Reason { get; set; }
    }

    [HttpGet("my")]
    [Produces<PagedResult<DelegationResponse>>]
    [EndpointSummary("My Delegations")]
    public async Task<IActionResult> My()
    {
        var result = await service.MyDelegations();
        return result.Match(_ => Ok(result.Value), Problem);
    }

    [HttpPost]
    [Produces<OperationResponse<EncryptedInt>>]
    [EndpointSummary("Create Delegation (self-service)")]
    public async Task<IActionResult> Create([FromBody] CreateDelegationRequest request)
    {
        var result = await service.Create(request);
        return result.Match(_ => Ok(result.Value), Problem);
    }

    [HttpPost("{delegateId}/revoke")]
    [Produces<OperationResponse<EncryptedInt>>]
    [EndpointSummary("Revoke Delegation (self-service)")]
    public async Task<IActionResult> Revoke([FromRoute] EncryptedInt delegateId, [FromBody] RevokeBody body)
    {
        var result = await service.Revoke(new RevokeDelegationRequest { DelegateId = delegateId }, body.Reason);
        return result.Match(_ => Ok(result.Value), Problem);
    }

    [HttpPost("{delegateId}/force-revoke")]
    [Produces<OperationResponse<EncryptedInt>>]
    [EndpointSummary("Force revoke delegation (admin emergency)")]
    public async Task<IActionResult> ForceRevoke([FromRoute] EncryptedInt delegateId, [FromBody] RevokeBody body)
    {
        var result = await service.ForceRevoke(new RevokeDelegationRequest { DelegateId = delegateId }, body.Reason);
        return result.Match(_ => Ok(result.Value), Problem);
    }
}