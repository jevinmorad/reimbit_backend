using AegisInt.Core;
using Common.Data.Models;
using Common.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Reimbit.Application.Finance;
using Reimbit.Contracts.Finance;

namespace Reimbit.Web.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public sealed class FinanceController(
    IFinanceService service,
    ICurrentUserProvider currentUserProvider
) : ApiController(currentUserProvider)
{
    public sealed class ReversePayoutRequest
    {
        public required string Reason { get; set; }
    }

    [HttpGet("payable-reports")]
    [Produces<PagedResult<PayableReportResponse>>]
    [EndpointSummary("List reports ready for payout")]
    public async Task<IActionResult> PayableReports()
    {
        var result = await service.PayableReports();
        return result.Match(_ => Ok(result.Value), Problem);
    }

    [HttpPost("payouts")]
    [Produces<OperationResponse<EncryptedInt>>]
    [EndpointSummary("Process payout for a report")]
    public async Task<IActionResult> ProcessPayout([FromBody] ProcessPayoutRequest request)
    {
        var result = await service.ProcessPayout(request);
        return result.Match(_ => Ok(result.Value), Problem);
    }

    [HttpPost("payouts/{reportId}/reverse")]
    [Produces<OperationResponse<EncryptedInt>>]
    [EndpointSummary("Reverse payout for a report")]
    public async Task<IActionResult> ReversePayout([FromRoute] EncryptedInt reportId, [FromBody] ReversePayoutRequest request)
    {
        var result = await service.ReversePayout(reportId, request.Reason);
        return result.Match(_ => Ok(result.Value), Problem);
    }

    [HttpGet("payouts/{reportId}")]
    [Produces<PagedResult<PayoutHistoryResponse>>]
    [EndpointSummary("Get payout history for a report")]
    public async Task<IActionResult> PayoutHistory([FromRoute] EncryptedInt reportId)
    {
        var result = await service.PayoutHistory(reportId);
        return result.Match(_ => Ok(result.Value), Problem);
    }
}