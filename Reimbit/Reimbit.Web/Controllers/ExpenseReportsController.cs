using AegisInt.Core;
using Common.Data.Models;
using Common.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Reimbit.Application.ExpenseReports;
using Reimbit.Contracts.ExpenseReports;

namespace Reimbit.Web.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public sealed class ExpenseReportsController(
    IExpenseReportService service,
    ICurrentUserProvider currentUserProvider
) : ApiController(currentUserProvider)
{
    [HttpPost]
    [Produces<OperationResponse<EncryptedInt>>]
    [EndpointSummary("Create expense report")]
    public async Task<IActionResult> Create([FromBody] CreateExpenseReportRequest request)
    {
        var result = await service.Create(request);
        return result.Match(_ => Ok(result.Value), Problem);
    }

    [HttpPost("{reportId}/expenses/{expenseId}")]
    [Produces<OperationResponse<EncryptedInt>>]
    [EndpointSummary("Add expense to report")]
    public async Task<IActionResult> AddExpense([FromRoute] EncryptedInt reportId, [FromRoute] EncryptedInt expenseId)
    {
        var result = await service.AddExpense(new AddExpenseToReportRequest { ReportId = reportId, ExpenseId = expenseId });
        return result.Match(_ => Ok(result.Value), Problem);
    }

    [HttpDelete("{reportId}/expenses/{expenseId}")]
    [Produces<OperationResponse<EncryptedInt>>]
    [EndpointSummary("Remove expense from report")]
    public async Task<IActionResult> RemoveExpense([FromRoute] EncryptedInt reportId, [FromRoute] EncryptedInt expenseId)
    {
        var result = await service.RemoveExpense(new RemoveExpenseFromReportRequest { ReportId = reportId, ExpenseId = expenseId });
        return result.Match(_ => Ok(result.Value), Problem);
    }

    [HttpPost("{reportId}/submit")]
    [Produces<OperationResponse<EncryptedInt>>]
    [EndpointSummary("Submit report for approval")]
    public async Task<IActionResult> Submit([FromRoute] EncryptedInt reportId)
    {
        var result = await service.Submit(new SubmitExpenseReportRequest { ReportId = reportId });
        return result.Match(_ => Ok(result.Value), Problem);
    }

    [HttpGet("{reportId}")]
    [Produces<GetExpenseReportResponse>]
    [EndpointSummary("Get report")]
    public async Task<IActionResult> Get([FromRoute] EncryptedInt reportId)
    {
        var result = await service.Get(reportId);
        return result.Match(_ => Ok(result.Value), Problem);
    }
}