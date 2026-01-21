using AegisInt.Core;
using Common.Data.Models;
using ErrorOr;
using Microsoft.EntityFrameworkCore;
using Reimbit.Application.Audit;
using Reimbit.Contracts.ExpenseReports;
using Reimbit.Contracts.Expenses;
using Reimbit.Contracts.Finance;
using Reimbit.Domain.Interfaces;
using Reimbit.Domain.Models;
using Reimbit.Domain.Repositories;

namespace Reimbit.Infrastructure.Repositories;

public sealed class FinanceRepository(
    IApplicationDbContext context,
    IAuditLogger auditLogger
) : IFinanceRepository
{
    public async Task<ErrorOr<PagedResult<PayableReportResponse>>> ListPayableReports(int organizationId)
    {
        var data = await context.ExpExpenseReports
            .AsNoTracking()
            .Where(r =>
                r.OrganizationId == organizationId &&
                r.Status == (byte)ExpenseReportStatus.Approved)
            .OrderBy(r => r.Created)
            .Select(r => new PayableReportResponse
            {
                ReportId = r.ReportId,
                Title = r.Title,
                PeriodStart = r.PeriodStart,
                PeriodEnd = r.PeriodEnd,
                TotalAmount = r.TotalAmount,
                Created = r.Created
            })
            .ToListAsync();

        return new PagedResult<PayableReportResponse> { Total = data.Count, Data = data };
    }

    public async Task<ErrorOr<OperationResponse<EncryptedInt>>> ProcessPayout(ProcessPayoutRequest request)
    {
        if (request.PaidAmount <= 0)
        {
            return Error.Validation("Payout.InvalidAmount", "Paid amount must be greater than 0.");
        }

        if (string.IsNullOrWhiteSpace(request.ReferenceNo))
        {
            return Error.Validation("Payout.ReferenceRequired", "Reference number is required.");
        }

        var dbContext = (DbContext)context;
        await using var tx = await dbContext.Database.BeginTransactionAsync();

        try
        {
            var reportId = (int)request.ReportId;

            var report = await context.ExpExpenseReports
                .FirstOrDefaultAsync(r => r.ReportId == reportId && r.OrganizationId == request.OrganizationId);

            if (report == null)
            {
                return Error.NotFound("ExpenseReport.NotFound", "Report not found.");
            }

            if (report.Status != (byte)ExpenseReportStatus.Approved)
            {
                return Error.Validation("Payout.NotPayable", "Only approved reports can be paid.");
            }

            var alreadyPaid = await context.PayPayouts.AnyAsync(p => p.ReportId == reportId);
            if (alreadyPaid)
            {
                return Error.Conflict("Payout.AlreadyProcessed", "Payout already processed for this report.");
            }

            var expenseIds = await context.ExpReportExpenses
                .Where(x => x.ReportId == reportId)
                .Select(x => x.ExpenseId)
                .ToListAsync();

            if (expenseIds.Count == 0)
            {
                return Error.Validation("Payout.EmptyReport", "Cannot pay a report without expenses.");
            }

            var expenses = await context.ExpExpenses
                .Where(e => expenseIds.Contains(e.ExpenseId) && e.OrganizationId == request.OrganizationId)
                .ToListAsync();

            if (expenses.Any(e => e.Status != (byte)ExpenseStatus.Approved))
            {
                return Error.Validation("Payout.InvalidExpenseState", "All expenses must be approved before payout.");
            }

            var payout = new PayPayout
            {
                ReportId = reportId,
                PaidAmount = request.PaidAmount,
                PaidOn = request.PaidOn,
                ReferenceNo = request.ReferenceNo
            };

            await context.PayPayouts.AddAsync(payout);

            report.Status = (byte)ExpenseReportStatus.Paid;
            report.ModifiedByUserId = request.ProcessedByUserId;
            report.Modified = DateTime.UtcNow;

            foreach (var exp in expenses)
            {
                exp.Status = (byte)ExpenseStatus.Paid;
                exp.Modified = DateTime.UtcNow;
            }

            var rowsAffected = await context.SaveChangesAsync(default);

            await auditLogger.WriteAsync(
                entityType: "PAY_Payout",
                entityId: payout.PayoutId,
                action: "PAYOUT_PROCESSED",
                organizationId: request.OrganizationId,
                userId: request.ProcessedByUserId,
                oldValue: null,
                newValue: new { payout.PayoutId, payout.ReportId, payout.PaidAmount, payout.PaidOn, payout.ReferenceNo },
                ipAddress: null,
                userAgent: null);

            await auditLogger.WriteAsync(
                entityType: "EXP_ExpenseReport",
                entityId: reportId,
                action: "REPORT_PAID",
                organizationId: request.OrganizationId,
                userId: request.ProcessedByUserId,
                oldValue: new { Status = (byte)ExpenseReportStatus.Approved },
                newValue: new { Status = report.Status },
                ipAddress: null,
                userAgent: null);

            foreach (var exp in expenses)
            {
                await auditLogger.WriteAsync(
                    entityType: "EXP_Expense",
                    entityId: exp.ExpenseId,
                    action: "EXPENSE_PAID",
                    organizationId: request.OrganizationId,
                    userId: request.ProcessedByUserId,
                    oldValue: new { Status = (byte)ExpenseStatus.Approved },
                    newValue: new { Status = (byte)ExpenseStatus.Paid, ReportId = reportId },
                    ipAddress: null,
                    userAgent: null);
            }

            await tx.CommitAsync();

            return new OperationResponse<EncryptedInt> { Id = payout.PayoutId, RowsAffected = rowsAffected };
        }
        catch (Exception ex)
        {
            await tx.RollbackAsync();
            return Error.Failure("Payout.Process.Failed", ex.Message);
        }
    }

    public async Task<ErrorOr<OperationResponse<EncryptedInt>>> ReversePayout(EncryptedInt reportId, int organizationId, int reversedByUserId, string reason)
    {
        if (string.IsNullOrWhiteSpace(reason))
        {
            return Error.Validation("PayoutReverse.ReasonRequired", "Reason is required.");
        }

        var dbContext = (DbContext)context;
        await using var tx = await dbContext.Database.BeginTransactionAsync();

        try
        {
            var id = (int)reportId;

            var report = await context.ExpExpenseReports
                .FirstOrDefaultAsync(r => r.ReportId == id && r.OrganizationId == organizationId);

            if (report == null)
            {
                return Error.NotFound("ExpenseReport.NotFound", "Report not found.");
            }

            if (report.Status != (byte)ExpenseReportStatus.Paid)
            {
                return Error.Validation("PayoutReverse.NotPaid", "Only paid reports can be reversed.");
            }

            var payout = await context.PayPayouts.FirstOrDefaultAsync(p => p.ReportId == id);
            if (payout == null)
            {
                return Error.NotFound("Payout.NotFound", "Payout not found.");
            }

            var expenseIds = await context.ExpReportExpenses
                .Where(x => x.ReportId == id)
                .Select(x => x.ExpenseId)
                .ToListAsync();

            var expenses = await context.ExpExpenses
                .Where(e => expenseIds.Contains(e.ExpenseId) && e.OrganizationId == organizationId)
                .ToListAsync();

            foreach (var exp in expenses)
            {
                exp.Status = (byte)ExpenseStatus.Approved;
                exp.Modified = DateTime.UtcNow;
            }

            report.Status = (byte)ExpenseReportStatus.Approved;
            report.ModifiedByUserId = reversedByUserId;
            report.Modified = DateTime.UtcNow;

            context.PayPayouts.Remove(payout);

            var rows = await context.SaveChangesAsync(default);

            await auditLogger.WriteAsync(
                entityType: "PAY_Payout",
                entityId: payout.PayoutId,
                action: "PAYOUT_REVERSED",
                organizationId: organizationId,
                userId: reversedByUserId,
                oldValue: new { payout.PayoutId, payout.ReportId, payout.PaidAmount, payout.PaidOn, payout.ReferenceNo },
                newValue: new { Reason = reason },
                ipAddress: null,
                userAgent: null);

            await auditLogger.WriteAsync(
                entityType: "EXP_ExpenseReport",
                entityId: id,
                action: "REPORT_PAYOUT_REVERSED",
                organizationId: organizationId,
                userId: reversedByUserId,
                oldValue: new { Status = (byte)ExpenseReportStatus.Paid },
                newValue: new { Status = (byte)ExpenseReportStatus.Approved, Reason = reason },
                ipAddress: null,
                userAgent: null);

            foreach (var exp in expenses)
            {
                await auditLogger.WriteAsync(
                    entityType: "EXP_Expense",
                    entityId: exp.ExpenseId,
                    action: "EXPENSE_PAYOUT_REVERSED",
                    organizationId: organizationId,
                    userId: reversedByUserId,
                    oldValue: new { Status = (byte)ExpenseStatus.Paid },
                    newValue: new { Status = (byte)ExpenseStatus.Approved, Reason = reason, ReportId = id },
                    ipAddress: null,
                    userAgent: null);
            }

            await tx.CommitAsync();

            return new OperationResponse<EncryptedInt> { Id = payout.PayoutId, RowsAffected = rows };
        }
        catch (Exception ex)
        {
            await tx.RollbackAsync();
            return Error.Failure("PayoutReverse.Failed", ex.Message);
        }
    }

    public async Task<ErrorOr<PagedResult<PayoutHistoryResponse>>> PayoutHistory(int organizationId, EncryptedInt reportId)
    {
        var id = (int)reportId;

        var reportExists = await context.ExpExpenseReports
            .AsNoTracking()
            .AnyAsync(r => r.ReportId == id && r.OrganizationId == organizationId);

        if (!reportExists)
        {
            return Error.NotFound("ExpenseReport.NotFound", "Report not found.");
        }

        var data = await context.PayPayouts
            .AsNoTracking()
            .Where(p => p.ReportId == id)
            .OrderByDescending(p => p.PaidOn)
            .Select(p => new PayoutHistoryResponse
            {
                PayoutId = p.PayoutId,
                ReportId = p.ReportId,
                PaidAmount = p.PaidAmount,
                PaidOn = p.PaidOn,
                ReferenceNo = p.ReferenceNo
            })
            .ToListAsync();

        return new PagedResult<PayoutHistoryResponse>
        {
            Total = data.Count,
            Data = data
        };
    }
}