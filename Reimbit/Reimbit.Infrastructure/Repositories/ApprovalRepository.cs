using AegisInt.Core;
using Common.Data.Models;
using ErrorOr;
using Microsoft.EntityFrameworkCore;
using Reimbit.Application.Audit;
using Reimbit.Contracts.Approvals;
using Reimbit.Contracts.ExpenseReports;
using Reimbit.Contracts.Expenses;
using Reimbit.Domain.Interfaces;
using Reimbit.Domain.Models;
using Reimbit.Domain.Repositories;

namespace Reimbit.Infrastructure.Repositories;

public sealed class ApprovalRepository(
    IApplicationDbContext context,
    IAuditLogger auditLogger,
    IDelegationRepository delegationRepository
) : IApprovalRepository
{
    public async Task<ErrorOr<PagedResult<ApprovalInboxItemResponse>>> Inbox(int approverUserId, int organizationId)
    {
        var query = context.AprApprovalInstances
            .AsNoTracking()
            .Include(i => i.ApprovalLevel)
            .Include(i => i.Report)
            .Where(i =>
                i.ApproverUserId == approverUserId &&
                i.Status == (byte)ApprovalInstanceStatus.Pending &&
                i.Report.OrganizationId == organizationId)
            .Select(i => new ApprovalInboxItemResponse
            {
                ApprovalInstanceId = i.ApprovalInstanceId,
                ReportId = i.ReportId,
                ReportTitle = i.Report.Title,
                TotalAmount = i.Report.TotalAmount,
                ReportStatus = i.Report.Status,
                LevelOrder = i.ApprovalLevel.LevelOrder,
                ActionAt = i.ActionAt
            });

        var data = await query
            .OrderBy(x => x.LevelOrder)
            .ToListAsync();

        return new PagedResult<ApprovalInboxItemResponse>
        {
            Total = data.Count,
            Data = data
        };
    }

    public async Task<ErrorOr<OperationResponse<EncryptedInt>>> Approve(EncryptedInt approvalInstanceId, int approverUserId, int organizationId)
    {
        var dbContext = (DbContext)context;
        await using var tx = await dbContext.Database.BeginTransactionAsync();

        try
        {
            var id = (int)approvalInstanceId;

            var instance = await context.AprApprovalInstances
                .Include(i => i.ApprovalLevel)
                .Include(i => i.Report)
                .FirstOrDefaultAsync(i => i.ApprovalInstanceId == id);

            if (instance == null)
            {
                return Error.NotFound("ApprovalInstance.NotFound", "Approval instance not found.");
            }

            if (instance.Report.OrganizationId != organizationId)
            {
                return Error.Unauthorized("ApprovalInstance.AccessDenied", "Invalid organization scope.");
            }

            var actingApprover = approverUserId;

            if (instance.ApproverUserId != actingApprover)
            {
                var delegateFor = await delegationRepository.ResolveDelegateApproverUserId(
                    organizationId,
                    instance.ApproverUserId,
                    DateTime.UtcNow);

                if (delegateFor == null || delegateFor.Value != actingApprover)
                {
                    return Error.Unauthorized("ApprovalInstance.NotAssignee", "Approval instance is not assigned to the current user.");
                }

                await auditLogger.WriteAsync(
                    entityType: "APR_ApprovalInstance",
                    entityId: instance.ApprovalInstanceId,
                    action: "APPROVED_BY_DELEGATE_ON_BEHALF_OF",
                    organizationId: organizationId,
                    userId: actingApprover,
                    oldValue: null,
                    newValue: new
                    {
                        instance.ReportId,
                        OriginalApproverUserId = instance.ApproverUserId,
                        DelegateUserId = actingApprover
                    },
                    ipAddress: null,
                    userAgent: null);
            }

            if (instance.Status != (byte)ApprovalInstanceStatus.Pending)
            {
                return Error.Validation("ApprovalInstance.NotPending", "Only pending approvals can be approved.");
            }

            // When self approval is detected:
            if (instance.Report.CreatedByUserId == approverUserId)
            {
                await auditLogger.WriteAsync(
                    entityType: "APR_ApprovalInstance",
                    entityId: instance.ApprovalInstanceId,
                    action: "SELF_APPROVAL_PREVENTED",
                    organizationId: organizationId,
                    userId: approverUserId,
                    oldValue: null,
                    newValue: new { instance.ReportId, instance.ApprovalLevelId, ApproverUserId = approverUserId },
                    ipAddress: null,
                    userAgent: null);

                return Error.Validation("Approval.SelfApproval", "Self-approval is not allowed.");
            }

            var hasEarlierPending = await context.AprApprovalInstances
                .Include(i => i.ApprovalLevel)
                .AnyAsync(i =>
                    i.ReportId == instance.ReportId &&
                    i.Status == (byte)ApprovalInstanceStatus.Pending &&
                    i.ApprovalLevel.LevelOrder < instance.ApprovalLevel.LevelOrder);

            if (hasEarlierPending)
            {
                await auditLogger.WriteAsync(
                    entityType: "APR_ApprovalInstance",
                    entityId: instance.ApprovalInstanceId,
                    action: "APPROVAL_OUT_OF_ORDER_PREVENTED",
                    organizationId: organizationId,
                    userId: approverUserId,
                    oldValue: null,
                    newValue: new { instance.ReportId, instance.ApprovalLevelId },
                    ipAddress: null,
                    userAgent: null);

                return Error.Validation("ApprovalInstance.OutOfOrder", "A prior approval level is still pending.");
            }

            instance.Status = (byte)ApprovalInstanceStatus.Approved;
            instance.ActionAt = DateTime.UtcNow;

            await context.SaveChangesAsync(default);

            var anyPending = await context.AprApprovalInstances
                .AnyAsync(i => i.ReportId == instance.ReportId && i.Status == (byte)ApprovalInstanceStatus.Pending);

            if (!anyPending)
            {
                instance.Report.Status = (byte)ExpenseReportStatus.Approved;
                instance.Report.ModifiedByUserId = approverUserId;
                instance.Report.Modified = DateTime.UtcNow;

                var expenseIds = await context.ExpReportExpenses
                    .Where(x => x.ReportId == instance.ReportId)
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

                await context.SaveChangesAsync(default);

                foreach (var exp in expenses)
                {
                    await auditLogger.WriteAsync(
                        entityType: "EXP_Expense",
                        entityId: exp.ExpenseId,
                        action: "EXPENSE_APPROVED",
                        organizationId: organizationId,
                        userId: approverUserId,
                        oldValue: new { Status = (byte)ExpenseStatus.UnderApproval },
                        newValue: new { Status = (byte)ExpenseStatus.Approved, ReportId = instance.ReportId },
                        ipAddress: null,
                        userAgent: null);
                }
            }

            await auditLogger.WriteAsync(
                entityType: "APR_ApprovalInstance",
                entityId: instance.ApprovalInstanceId,
                action: "APPROVE",
                organizationId: organizationId,
                userId: approverUserId,
                oldValue: new { Status = (byte)ApprovalInstanceStatus.Pending },
                newValue: new { Status = instance.Status, instance.ActionAt },
                ipAddress: null,
                userAgent: null);

            await tx.CommitAsync();

            return new OperationResponse<EncryptedInt> { Id = instance.ApprovalInstanceId, RowsAffected = 1 };
        }
        catch (Exception ex)
        {
            await tx.RollbackAsync();
            return Error.Failure("Approval.Approve.Failed", ex.Message);
        }
    }

    public async Task<ErrorOr<OperationResponse<EncryptedInt>>> Reject(EncryptedInt approvalInstanceId, string reason, int approverUserId, int organizationId)
    {
        var dbContext = (DbContext)context;
        await using var tx = await dbContext.Database.BeginTransactionAsync();

        try
        {
            if (string.IsNullOrWhiteSpace(reason))
            {
                return Error.Validation("Approval.Reject.ReasonRequired", "Rejection reason is required.");
            }

            var id = (int)approvalInstanceId;

            var instance = await context.AprApprovalInstances
                .Include(i => i.ApprovalLevel)
                .Include(i => i.Report)
                .FirstOrDefaultAsync(i => i.ApprovalInstanceId == id);

            if (instance == null)
            {
                return Error.NotFound("ApprovalInstance.NotFound", "Approval instance not found.");
            }

            if (instance.Report.OrganizationId != organizationId)
            {
                return Error.Unauthorized("ApprovalInstance.AccessDenied", "Invalid organization scope.");
            }

            if (instance.ApproverUserId != approverUserId)
            {
                return Error.Unauthorized("ApprovalInstance.NotAssignee", "Approval instance is not assigned to the current user.");
            }

            if (instance.Status != (byte)ApprovalInstanceStatus.Pending)
            {
                return Error.Validation("ApprovalInstance.NotPending", "Only pending approvals can be rejected.");
            }

            if (instance.Report.CreatedByUserId == approverUserId)
            {
                return Error.Validation("Approval.SelfApproval", "Self-approval is not allowed.");
            }

            // Enforce ordering
            var hasEarlierPending = await context.AprApprovalInstances
                .Include(i => i.ApprovalLevel)
                .AnyAsync(i =>
                    i.ReportId == instance.ReportId &&
                    i.Status == (byte)ApprovalInstanceStatus.Pending &&
                    i.ApprovalLevel.LevelOrder < instance.ApprovalLevel.LevelOrder);

            if (hasEarlierPending)
            {
                return Error.Validation("ApprovalInstance.OutOfOrder", "A prior approval level is still pending.");
            }

            instance.Status = (byte)ApprovalInstanceStatus.Rejected;
            instance.ActionAt = DateTime.UtcNow;

            instance.Report.Status = (byte)ExpenseReportStatus.Open;
            instance.Report.ModifiedByUserId = approverUserId;
            instance.Report.Modified = DateTime.UtcNow;

            // Create immutable rejection record
            await context.ExpReportRejections.AddAsync(new ExpReportRejection
            {
                ReportId = instance.ReportId,
                RejectedByUserId = approverUserId,
                Reason = reason,
                RejectedAt = DateTime.UtcNow
            });

            // Return expenses to Draft and detach from report (this matches your rule: rejected expenses returned)
            var reportExpenseLinks = await context.ExpReportExpenses
                .Where(x => x.ReportId == instance.ReportId)
                .ToListAsync();

            var expenseIds = reportExpenseLinks.Select(x => x.ExpenseId).ToList();

            var expenses = await context.ExpExpenses
                .Where(e => expenseIds.Contains(e.ExpenseId) && e.OrganizationId == organizationId)
                .ToListAsync();

            foreach (var exp in expenses)
            {
                exp.Status = (byte)ExpenseStatus.Draft;
                exp.Modified = DateTime.UtcNow;
            }

            context.ExpReportExpenses.RemoveRange(reportExpenseLinks);
            instance.Report.TotalAmount = 0;

            // Cancel other pending instances for this report (end current cycle)
            var otherPending = await context.AprApprovalInstances
                .Where(i => i.ReportId == instance.ReportId && i.Status == (byte)ApprovalInstanceStatus.Pending)
                .ToListAsync();

            foreach (var p in otherPending)
            {
                p.Status = (byte)ApprovalInstanceStatus.Rejected;
                p.ActionAt = DateTime.UtcNow;
            }

            await context.SaveChangesAsync(default);

            await auditLogger.WriteAsync(
                entityType: "APR_ApprovalInstance",
                entityId: instance.ApprovalInstanceId,
                action: "REJECT",
                organizationId: organizationId,
                userId: approverUserId,
                oldValue: new { Status = (byte)ApprovalInstanceStatus.Pending },
                newValue: new { Status = instance.Status, instance.ActionAt, Reason = reason },
                ipAddress: null,
                userAgent: null);

            await auditLogger.WriteAsync(
                entityType: "EXP_ExpenseReport",
                entityId: instance.ReportId,
                action: "REJECT",
                organizationId: organizationId,
                userId: approverUserId,
                oldValue: null,
                newValue: new { Status = instance.Report.Status },
                ipAddress: null,
                userAgent: null);

            foreach (var exp in expenses)
            {
                await auditLogger.WriteAsync(
                    entityType: "EXP_Expense",
                    entityId: exp.ExpenseId,
                    action: "EXPENSE_REJECTED",
                    organizationId: organizationId,
                    userId: approverUserId,
                    oldValue: new { Status = (byte)ExpenseStatus.UnderApproval },
                    newValue: new { Status = (byte)ExpenseStatus.Draft, ReportId = instance.ReportId, Reason = reason },
                    ipAddress: null,
                    userAgent: null);
            }

            await tx.CommitAsync();

            return new OperationResponse<EncryptedInt> { Id = instance.ApprovalInstanceId, RowsAffected = 1 };
        }
        catch (Exception ex)
        {
            await tx.RollbackAsync();
            return Error.Failure("Approval.Reject.Failed", ex.Message);
        }
    }
}