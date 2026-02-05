using AegisInt.Core;
using Common.Data.Models;
using Common.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Reimbit.Application.Approvals;
using Reimbit.Contracts.Approvals;
using Reimbit.Core.Common.Permissions;

namespace Reimbit.Web.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public sealed class ApprovalsController(
    IApprovalService service,
    ICurrentUserProvider currentUserProvider
) : ApiController(currentUserProvider)
{
    [HttpGet("inbox")]
    [Produces<PagedResult<ApprovalInboxItemResponse>>]
    [EndpointSummary("Approval inbox")]
    public async Task<IActionResult> Inbox()
    {
        var result = await service.Inbox();
        return result.Match(_ => Ok(result.Value), Problem);
    }

    [HttpPost("{approvalInstanceId}/approve")]
    [Produces<OperationResponse<EncryptedInt>>]
    [EndpointSummary("Approve")]
    [HasPermission(Permission.ExpenseApprove)]
    public async Task<IActionResult> Approve([FromRoute] EncryptedInt approvalInstanceId)
    {
        var result = await service.Approve(new ApproveApprovalRequest { ApprovalInstanceId = approvalInstanceId });
        return result.Match(_ => Ok(result.Value), Problem);
    }

    [HttpPost("{approvalInstanceId}/reject")]
    [Produces<OperationResponse<EncryptedInt>>]
    [EndpointSummary("Reject")]
    [HasPermission(Permission.ExpenseReject)]
    public async Task<IActionResult> Reject([FromRoute] EncryptedInt approvalInstanceId, [FromBody] RejectApprovalRequest request)
    {
        request.ApprovalInstanceId = approvalInstanceId;

        var result = await service.Reject(request);
        return result.Match(_ => Ok(result.Value), Problem);
    }
}