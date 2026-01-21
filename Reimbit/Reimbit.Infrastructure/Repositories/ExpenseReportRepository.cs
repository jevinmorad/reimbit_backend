using AegisInt.Core;
using Common.Data.Models;
using ErrorOr;
using Microsoft.EntityFrameworkCore;
using Reimbit.Application.Audit;
using Reimbit.Contracts.ExpenseReports;
using Reimbit.Contracts.Expenses;
using Reimbit.Domain.Interfaces;
using Reimbit.Domain.Models;
using Reimbit.Domain.Repositories;

namespace Reimbit.Infrastructure.Repositories;

public sealed class ExpenseReportRepository(
    IApplicationDbContext context,
    IAuditLogger auditLogger,
    IDelegationRepository delegationRepository
) : IExpenseReportRepository
{
    public async Task<ErrorOr<OperationResponse<EncryptedInt>>> Create(CreateExpenseReportRequest request)
    {
        var dbContext = (DbContext)context;
        await using var tx = await dbContext.Database.BeginTransactionAsync();

        try
        {
            var report = new ExpExpenseReport
            {
                OrganizationId = request.OrganizationId,
                ProjectId = request.ProjectId,
                PeriodStart = request.PeriodStart,
                PeriodEnd = request.PeriodEnd,
                Title = request.Title,
                Status = (byte)ExpenseReportStatus.Open,
                TotalAmount = 0,
                ViewedAt = null,
                CreatedByUserId = request.CreatedByUserId,
                ModifiedByUserId = request.ModifiedByUserId,
                Created = request.Created,
                Modified = request.Modified
            };

            await context.ExpExpenseReports.AddAsync(report);
            var rowsAffected = await context.SaveChangesAsync(default);

            await auditLogger.WriteAsync(
                entityType: "EXP_ExpenseReport",
                entityId: report.ReportId,
                action: "CREATE",
                organizationId: report.OrganizationId,
                userId: request.CreatedByUserId,
                oldValue: null,
                newValue: new
                {
                    report.ReportId,
                    report.OrganizationId,
                    report.ProjectId,
                    report.PeriodStart,
                    report.PeriodEnd,
                    report.Title,
                    report.Status,
                    report.TotalAmount
                },
                ipAddress: null,
                userAgent: null);

            await tx.CommitAsync();

            return new OperationResponse<EncryptedInt> { Id = report.ReportId, RowsAffected = rowsAffected };
        }
        catch (Exception ex)
        {
            await tx.RollbackAsync();
            return Error.Failure("ExpenseReport.Create.Failed", ex.Message);
        }
    }

    public async Task<ErrorOr<OperationResponse<EncryptedInt>>> AddExpense(AddExpenseToReportRequest request)
    {
        var dbContext = (DbContext)context;
        await using var tx = await dbContext.Database.BeginTransactionAsync();

        try
        {
            var reportId = (int)request.ReportId;
            var expenseId = (int)request.ExpenseId;

            var report = await context.ExpExpenseReports.FirstOrDefaultAsync(r =>
                r.ReportId == reportId &&
                r.OrganizationId == request.OrganizationId);

            if (report == null)
            {
                return Error.NotFound("ExpenseReport.NotFound", "Report not found");
            }

            if (report.CreatedByUserId != request.UserId)
            {
                return Error.Unauthorized("ExpenseReport.AccessDenied", "Only the report owner can modify the report.");
            }

            if (report.Status != (byte)ExpenseReportStatus.Open)
            {
                return Error.Validation("ExpenseReport.NotEditable", "Only open reports can be modified.");
            }

            var expense = await context.ExpExpenses.FirstOrDefaultAsync(e =>
                e.ExpenseId == expenseId &&
                e.OrganizationId == request.OrganizationId);

            if (expense == null)
            {
                return Error.NotFound("Expense.NotFound", "Expense not found");
            }

            if (expense.EmployeeId != request.UserId)
            {
                return Error.Unauthorized("Expense.AccessDenied", "Only the expense owner can attach it to a report.");
            }

            if (expense.Status != (byte)ExpenseStatus.Draft)
            {
                return Error.Validation("Expense.NotAttachable", "Only draft expenses can be added to a report.");
            }

            var exists = await context.ExpReportExpenses.AnyAsync(x =>
                x.ReportId == reportId &&
                x.ExpenseId == expenseId);

            if (exists)
            {
                return Error.Conflict("ExpenseReportExpense.Exists", "Expense already added to report.");
            }

            await context.ExpReportExpenses.AddAsync(new ExpReportExpense
            {
                ReportId = reportId,
                ExpenseId = expenseId
            });

            report.TotalAmount += expense.Amount;
            report.ModifiedByUserId = request.UserId;
            report.Modified = DateTime.UtcNow;

            var rowsAffected = await context.SaveChangesAsync(default);

            await auditLogger.WriteAsync(
                entityType: "EXP_ReportExpense",
                entityId: null,
                action: "CREATE",
                organizationId: request.OrganizationId,
                userId: request.UserId,
                oldValue: null,
                newValue: new { ReportId = reportId, ExpenseId = expenseId },
                ipAddress: null,
                userAgent: null);

            await tx.CommitAsync();

            return new OperationResponse<EncryptedInt> { Id = reportId, RowsAffected = rowsAffected };
        }
        catch (Exception ex)
        {
            await tx.RollbackAsync();
            return Error.Failure("ExpenseReport.AddExpense.Failed", ex.Message);
        }
    }

    public async Task<ErrorOr<OperationResponse<EncryptedInt>>> RemoveExpense(RemoveExpenseFromReportRequest request)
    {
        var dbContext = (DbContext)context;
        await using var tx = await dbContext.Database.BeginTransactionAsync();

        try
        {
            var reportId = (int)request.ReportId;
            var expenseId = (int)request.ExpenseId;

            var report = await context.ExpExpenseReports.FirstOrDefaultAsync(r =>
                r.ReportId == reportId &&
                r.OrganizationId == request.OrganizationId);

            if (report == null)
            {
                return Error.NotFound("ExpenseReport.NotFound", "Report not found");
            }

            if (report.CreatedByUserId != request.UserId)
            {
                return Error.Unauthorized("ExpenseReport.AccessDenied", "Only the report owner can modify the report.");
            }

            if (report.Status != (byte)ExpenseReportStatus.Open)
            {
                return Error.Validation("ExpenseReport.NotEditable", "Only open reports can be modified.");
            }

            var link = await context.ExpReportExpenses.FirstOrDefaultAsync(x =>
                x.ReportId == reportId &&
                x.ExpenseId == expenseId);

            if (link == null)
            {
                return Error.NotFound("ExpenseReportExpense.NotFound", "Expense is not attached to this report.");
            }

            var expense = await context.ExpExpenses.FirstOrDefaultAsync(e =>
                e.ExpenseId == expenseId &&
                e.OrganizationId == request.OrganizationId);

            if (expense == null)
            {
                return Error.NotFound("Expense.NotFound", "Expense not found");
            }

            context.ExpReportExpenses.Remove(link);

            report.TotalAmount -= expense.Amount;
            if (report.TotalAmount < 0)
            {
                report.TotalAmount = 0;
            }

            report.ModifiedByUserId = request.UserId;
            report.Modified = DateTime.UtcNow;

            var rowsAffected = await context.SaveChangesAsync(default);

            await auditLogger.WriteAsync(
                entityType: "EXP_ReportExpense",
                entityId: null,
                action: "DELETE",
                organizationId: request.OrganizationId,
                userId: request.UserId,
                oldValue: new { ReportId = reportId, ExpenseId = expenseId },
                newValue: null,
                ipAddress: null,
                userAgent: null);

            await tx.CommitAsync();

            return new OperationResponse<EncryptedInt> { Id = reportId, RowsAffected = rowsAffected };
        }
        catch (Exception ex)
        {
            await tx.RollbackAsync();
            return Error.Failure("ExpenseReport.RemoveExpense.Failed", ex.Message);
        }
    }

    public async Task<ErrorOr<OperationResponse<EncryptedInt>>> Submit(SubmitExpenseReportRequest request)
    {
        var dbContext = (DbContext)context;
        await using var tx = await dbContext.Database.BeginTransactionAsync();

        try
        {
            var reportId = (int)request.ReportId;

            var report = await context.ExpExpenseReports
                .Include(r => r.ExpReportExpenses)
                .FirstOrDefaultAsync(r => r.ReportId == reportId && r.OrganizationId == request.OrganizationId);

            if (report == null)
            {
                return Error.NotFound("ExpenseReport.NotFound", "Report not found");
            }

            if (report.CreatedByUserId != request.UserId)
            {
                return Error.Unauthorized("ExpenseReport.AccessDenied", "Only the report owner can submit the report.");
            }

            if (report.Status != (byte)ExpenseReportStatus.Open)
            {
                return Error.Validation("ExpenseReport.NotSubmittable", "Only open reports can be submitted.");
            }

            var expenseIds = await context.ExpReportExpenses
                .Where(x => x.ReportId == reportId)
                .Select(x => x.ExpenseId)
                .ToListAsync();

            if (expenseIds.Count == 0)
            {
                return Error.Validation("ExpenseReport.Empty", "Cannot submit an empty report.");
            }

            var expenses = await context.ExpExpenses
                .Where(e => expenseIds.Contains(e.ExpenseId) && e.OrganizationId == request.OrganizationId)
                .ToListAsync();

            if (expenses.Any(e => e.EmployeeId != request.UserId))
            {
                return Error.Validation("ExpenseReport.InvalidExpenseOwner", "Report contains expenses not owned by the submitter.");
            }

            if (expenses.Any(e => e.Status != (byte)ExpenseStatus.Draft))
            {
                return Error.Validation("ExpenseReport.InvalidExpenseState", "All expenses must be draft to submit.");
            }

            report.TotalAmount = expenses.Sum(e => e.Amount);
            report.Status = (byte)ExpenseReportStatus.Submitted;
            report.ModifiedByUserId = request.UserId;
            report.Modified = DateTime.UtcNow;

            var rule = await context.AprApprovalRules
                .AsNoTracking()
                .Where(r =>
                    r.OrganizationId == request.OrganizationId &&
                    r.IsActive &&
                    (r.MinAmount == null || report.TotalAmount >= r.MinAmount) &&
                    (r.MaxAmount == null || report.TotalAmount <= r.MaxAmount))
                .OrderBy(r => r.Priority)
                .FirstOrDefaultAsync();

            if (rule == null)
            {
                return Error.Validation("ApprovalRule.NotFound", "No approval rule is configured for this report.");
            }

            var levels = await context.AprApprovalLevels
                .AsNoTracking()
                .Where(l => l.ApprovalRuleId == rule.ApprovalRuleId)
                .OrderBy(l => l.LevelOrder)
                .ToListAsync();

            if (levels.Count == 0)
            {
                return Error.Validation("ApprovalLevel.NotFound", "Approval rule has no levels configured.");
            }

            var existingInstances = await context.AprApprovalInstances
                .Where(i => i.ReportId == reportId)
                .ToListAsync();

            if (existingInstances.Count > 0)
            {
                context.AprApprovalInstances.RemoveRange(existingInstances);
                await context.SaveChangesAsync(default);
            }

            await auditLogger.WriteAsync(
                entityType: "APR_ApprovalRule",
                entityId: rule.ApprovalRuleId,
                action: "APPROVAL_RULE_MATCHED",
                organizationId: request.OrganizationId,
                userId: request.UserId,
                oldValue: null,
                newValue: new
                {
                    rule.ApprovalRuleId,
                    rule.RuleName,
                    ReportId = reportId,
                    report.TotalAmount
                },
                ipAddress: null,
                userAgent: null);

            foreach (var level in levels)
            {
                var approverUserId = await ResolveApproverUserIdAsync(request.OrganizationId, report, level);

                if (approverUserId == null)
                {
                    return Error.Validation("Approval.ApproverNotResolved", $"Cannot resolve approver for level {level.LevelOrder}.");
                }

                await auditLogger.WriteAsync(
                    entityType: "APR_ApprovalInstance",
                    entityId: null,
                    action: "APPROVER_ASSIGNED",
                    organizationId: request.OrganizationId,
                    userId: request.UserId,
                    oldValue: null,
                    newValue: new
                    {
                        ReportId = reportId,
                        level.ApprovalLevelId,
                        level.LevelOrder,
                        ApproverUserId = approverUserId.Value
                    },
                    ipAddress: null,
                    userAgent: null);

                if (approverUserId.Value == request.UserId)
                {
                    await auditLogger.WriteAsync(
                        entityType: "APR_ApprovalLevel",
                        entityId: level.ApprovalLevelId,
                        action: "SELF_APPROVAL_PREVENTED",
                        organizationId: request.OrganizationId,
                        userId: request.UserId,
                        oldValue: null,
                        newValue: new { ReportId = reportId, level.LevelOrder, AttemptedApproverUserId = approverUserId.Value },
                        ipAddress: null,
                        userAgent: null);

                    return Error.Validation("Approval.SelfApproval", "Self-approval is not allowed.");
                }

                await context.AprApprovalInstances.AddAsync(new AprApprovalInstance
                {
                    ReportId = reportId,
                    ApprovalLevelId = level.ApprovalLevelId,
                    ApproverUserId = approverUserId.Value,
                    Status = 1,
                    ActionAt = null
                });
            }

            var rowsAffected = await context.SaveChangesAsync(default);

            foreach (var exp in expenses)
            {
                exp.Status = (byte)ExpenseStatus.UnderApproval;
                exp.Modified = DateTime.UtcNow;

                await auditLogger.WriteAsync(
                    entityType: "EXP_Expense",
                    entityId: exp.ExpenseId,
                    action: "EXPENSE_SUBMITTED",
                    organizationId: request.OrganizationId,
                    userId: request.UserId,
                    oldValue: new { Status = (byte)ExpenseStatus.Draft },
                    newValue: new { Status = exp.Status, ReportId = reportId },
                    ipAddress: null,
                    userAgent: null);
            }

            await tx.CommitAsync();

            return new OperationResponse<EncryptedInt> { Id = reportId, RowsAffected = rowsAffected };
        }
        catch (Exception ex)
        {
            await tx.RollbackAsync();
            return Error.Failure("ExpenseReport.Submit.Failed", ex.Message);
        }
    }

    public async Task<ErrorOr<GetExpenseReportResponse>> Get(EncryptedInt reportId, int organizationId)
    {
        var id = (int)reportId;

        var report = await context.ExpExpenseReports
            .AsNoTracking()
            .Where(r => r.ReportId == id && r.OrganizationId == organizationId)
            .Select(r => new GetExpenseReportResponse
            {
                ReportId = r.ReportId,
                Title = r.Title,
                PeriodStart = r.PeriodStart,
                PeriodEnd = r.PeriodEnd,
                ProjectId = r.ProjectId,
                Status = r.Status,
                TotalAmount = r.TotalAmount,
                Created = r.Created
            })
            .FirstOrDefaultAsync();

        if (report == null)
        {
            return Error.NotFound("ExpenseReport.NotFound", "Report not found");
        }

        return report;
    }

    private async Task<int?> ResolveApproverUserIdAsync(int organizationId, ExpExpenseReport report, AprApprovalLevel level)
    {
        int? originalApproverId = level.ApproverType switch
        {
            1 => await context.SecUserManagers
                .AsNoTracking()
                .Where(m =>
                    m.UserId == report.CreatedByUserId &&
                    m.ManagerType == 1 &&
                    (m.ValidTo == null || m.ValidTo > DateTime.UtcNow))
                .OrderByDescending(m => m.IsPrimary)
                .Select(m => (int?)m.ManagerId)
                .FirstOrDefaultAsync(),

            2 => report.ProjectId == null
                ? null
                : await context.ProjProjects
                    .AsNoTracking()
                    .Where(p => p.ProjectId == report.ProjectId.Value && p.OrganizationId == organizationId)
                    .Select(p => (int?)p.ManagerId)
                    .FirstOrDefaultAsync(),

            3 or 5 or 6 => level.ApproverRoleId == null
                ? null
                : await context.SecUserRoles
                    .AsNoTracking()
                    .Where(ur =>
                        ur.RoleId == level.ApproverRoleId.Value &&
                        ur.Role.ValidFrom <= DateTime.UtcNow &&
                        (ur.Role.ValidTo == null || ur.Role.ValidTo >= DateTime.UtcNow))
                    .Select(ur => (int?)ur.UserId)
                    .FirstOrDefaultAsync(),

            4 => level.SpecificUserId,

            _ => null
        };

        if (originalApproverId == null)
        {
            return null;
        }

        var delegatedApproverId = await delegationRepository.ResolveDelegateApproverUserId(
            organizationId,
            originalApproverId.Value,
            DateTime.UtcNow);

        if (delegatedApproverId != null)
        {
            await auditLogger.WriteAsync(
                entityType: "SEC_DelegateApprover",
                entityId: null,
                action: "DELEGATE_USED_INSTEAD_OF_ORIGINAL_APPROVER",
                organizationId: organizationId,
                userId: delegatedApproverId.Value,
                oldValue: null,
                newValue: new
                {
                    ReportId = report.ReportId,
                    OriginalApproverUserId = originalApproverId.Value,
                    DelegateUserId = delegatedApproverId.Value,
                    ApprovalLevelId = level.ApprovalLevelId,
                    level.LevelOrder
                },
                ipAddress: null,
                userAgent: null);

            return delegatedApproverId.Value;
        }

        return originalApproverId;
    }
}