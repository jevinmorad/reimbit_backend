using AegisInt.Core;
using Common.Data.Models;
using ErrorOr;
using Microsoft.EntityFrameworkCore;
using Reimbit.Application.Audit;
using Reimbit.Contracts.Expenses;
using Reimbit.Domain.Interfaces;
using Reimbit.Domain.Models;
using Reimbit.Domain.Repositories;
using Reimbit.Infrastructure.Extensions;

namespace Reimbit.Infrastructure.Repositories;

public class ExpenseRepository(
    IApplicationDbContext context,
    IAuditLogger auditLogger
) : IExpenseRepository
{
    public async Task<ErrorOr<OperationResponse<EncryptedInt>>> Insert(ExpenseInsertRequest request)
    {
        var response = new OperationResponse<EncryptedInt>();
        var dbContext = (DbContext)context;

        await using var tx = await dbContext.Database.BeginTransactionAsync();

        try
        {
            var expense = new ExpExpense
            {
                OrganizationId = request.OrganizationId,
                EmployeeId = request.UserId,
                CategoryId = request.CategoryId,
                Title = request.Title,
                Amount = request.Amount,
                Currency = request.Currency ?? "INR",
                ReceiptUrl = request.ReceiptUrl ?? string.Empty,
                Description = request.Description,
                Status = (byte)ExpenseStatusEnum.Draft,
                CreatedByUserId = request.CreatedByUserId,
                Created = request.Created,
                Modified = request.Modified
            };

            await context.ExpExpenses.AddAsync(expense);
            var rowsAffected = await context.SaveChangesAsync();

            await auditLogger.WriteAsync(
                entityType: "EXP_Expense",
                entityId: expense.ExpenseId,
                action: "CREATE",
                organizationId: expense.OrganizationId,
                userId: request.CreatedByUserId,
                oldValue: null,
                newValue: new
                {
                    expense.ExpenseId,
                    expense.OrganizationId,
                    expense.EmployeeId,
                    expense.CategoryId,
                    expense.Title,
                    expense.Amount,
                    expense.Currency,
                    expense.ReceiptUrl,
                    expense.Status,
                    expense.Created,
                    expense.Modified
                },
                ipAddress: null,
                userAgent: null);

            await tx.CommitAsync();

            response.Id = expense.ExpenseId;
            response.RowsAffected = rowsAffected;
            return response;
        }
        catch (Exception ex)
        {
            await tx.RollbackAsync();
            return Error.Failure("Expense.Insert.Failed", ex.Message);
        }
    }

    public async Task<ErrorOr<PagedResult<ExpensesSelectPageResponse>>> SelectPage(ExpenseSelectPageRequest request)
    {
        var query = context.ExpExpenses.AsNoTracking();

        if (request.UserID.HasValue)
            query = query.Where(x => x.EmployeeId == request.UserID);

        if (!string.IsNullOrWhiteSpace(request.Title))
            query = query.Where(x => x.Title.Contains(request.Title));

        if (request.FromDate.HasValue)
            query = query.Where(x => x.Created >= request.FromDate.Value);

        if (request.ToDate.HasValue)
            query = query.Where(x => x.Created <= request.ToDate.Value);

        var total = await query.CountAsync();

        var data = await query
            .Select(x => new ExpensesSelectPageResponse
            {
                ExpenseId = x.ExpenseId,
                CategoryId = x.CategoryId,
                CategoryName = x.Category.CategoryName,
                Title = x.Title,
                Amount = x.Amount,
                Currency = x.Currency,
                Status = ((ExpenseStatusEnum)x.Status).ToString(),
                Created = x.Created
            })
            .ApplySorting(
                request.SortField,
                request.SortOrder,
                defaultField: nameof(ExpensesSelectPageResponse.Created)
            )
            .Skip(request.PageOffset)
            .Take(request.PageSize)
            .ToListAsync();

        return new PagedResult<ExpensesSelectPageResponse>
        {
            Data = data,
            Total = total
        };
    }
    public async Task<ErrorOr<OperationResponse<EncryptedInt>>> Update(ExpenseUpdateRequest request)
    {
        var expense = await context.ExpExpenses.FirstOrDefaultAsync(x =>
            x.OrganizationId == request.OrganizationId && x.ExpenseId == request.ExpenseId.Value);

        if (expense == null) return Error.NotFound("Expense.NotFound", "Expense not found");

        if (expense.Status != (byte)ExpenseStatusEnum.Draft && expense.Status != (byte)ExpenseStatusEnum.Rejected)
        {
            return Error.Validation("Expense.Update.NotAllowed", "Only draft/rejected expenses can be updated.");
        }

        var oldValue = new { expense.CategoryId, expense.Title, expense.Amount, expense.Currency, expense.ReceiptUrl, expense.Description, expense.Status, expense.Modified };

        expense.CategoryId = request.CategoryId.Value;
        expense.Title = request.Title;
        expense.Amount = request.Amount;
        expense.Currency = request.Currency ?? "INR";
        expense.ReceiptUrl = request.ReceiptUrl ?? string.Empty;
        expense.Description = request.Description;
        expense.Modified = DateTime.UtcNow;

        await using var tx = await ((DbContext)context).Database.BeginTransactionAsync();
        try
        {
            var rowsAffected = await context.SaveChangesAsync();

            await auditLogger.WriteAsync(
                entityType: "EXP_Expense",
                entityId: expense.ExpenseId,
                action: "UPDATE",
                organizationId: expense.OrganizationId,
                userId: request.ModifiedByUserId,
                oldValue: oldValue,
                newValue: new { expense.CategoryId, expense.Title, expense.Amount, expense.Currency, expense.ReceiptUrl, expense.Description, expense.Status, expense.Modified },
                ipAddress: null, userAgent: null);

            await tx.CommitAsync();
            return new OperationResponse<EncryptedInt> { Id = expense.ExpenseId, RowsAffected = rowsAffected };
        }
        catch (Exception ex)
        {
            await tx.RollbackAsync();
            return Error.Failure("Expense.Update.Failed", ex.Message);
        }
    }

    public async Task<ErrorOr<OperationResponse<EncryptedInt>>> Delete(EncryptedInt expenseId)
    {
        var expense = await context.ExpExpenses.FirstOrDefaultAsync(x => x.ExpenseId == expenseId.Value);

        if (expense == null) return Error.NotFound("Expense.NotFound", "Expense not found");

        if (expense.Status != (byte)ExpenseStatusEnum.Draft)
        {
            return Error.Validation("Expense.Delete.NotAllowed", "Only draft expenses can be deleted.");
        }

        await using var tx = await ((DbContext)context).Database.BeginTransactionAsync();
        try
        {
            context.ExpExpenses.Remove(expense);
            var rowsAffected = await context.SaveChangesAsync();

            await auditLogger.WriteAsync(
                entityType: "EXP_Expense",
                entityId: expense.ExpenseId,
                action: "DELETE",
                organizationId: expense.OrganizationId,
                userId: expense.EmployeeId,
                oldValue: expense,
                newValue: null,
                ipAddress: null, userAgent: null
            );

            await tx.CommitAsync();
            return new OperationResponse<EncryptedInt> { Id = expense.ExpenseId, RowsAffected = rowsAffected };
        }
        catch (Exception ex)
        {
            await tx.RollbackAsync();
            return Error.Failure("Expense.Delete.Failed", ex.Message);
        }
    }

    public async Task<ErrorOr<ExpenseSelectPkResponse>> Get(EncryptedInt expenseId)
    {
        var expense = await context.ExpExpenses
            .AsNoTracking()
            .Where(x => x.ExpenseId == expenseId.Value)
            .Select(x => new ExpenseSelectPkResponse
            {
                ExpenseId = x.ExpenseId,
                CategoryId = x.CategoryId,
                Title = x.Title,
                Amount = x.Amount,
                Currency = x.Currency,
                ReceiptUrl = x.ReceiptUrl,
                Description = x.Description,
                Status = ((ExpenseStatusEnum)x.Status).ToString(),
                Created = x.Created
            })
            .FirstOrDefaultAsync();

        if (expense == null)
        {
            return Error.NotFound("Expense.NotFound", "Expense not found");
        }

        return expense;
    }

    public async Task<ErrorOr<ExpenseSelectViewResponse>> View(EncryptedInt expenseId)
    {
        var expense = await context.ExpExpenses
            .AsNoTracking()
            .Where(x => x.ExpenseId == expenseId.Value)
            .Select(x => new ExpenseSelectViewResponse
            {
                Title = x.Title,
                Amount = x.Amount,
                Currency = x.Currency,
                Description = x.Description,
                AttachmentUrl = x.ReceiptUrl,
                ExpenseStatus = ((ExpenseStatusEnum)x.Status).ToString(),
                RejectionReason = x.ExpExpenseRejections
                    .OrderByDescending(r => r.RejectedAt)
                    .Select(r => r.Reason)
                    .FirstOrDefault(),
                CategoryName = x.Category.CategoryName,
                UserDisplayName = x.Employee.DisplayName,
                CreatedByUserDisplayName = x.CreatedByUser.DisplayName,
                Created = x.Created,
                Modified = x.Modified
            })
            .FirstOrDefaultAsync();

        if (expense == null)
        {
            return Error.NotFound("Expense.NotFound", "Expense not found");
        }

        return expense;
    }

    public Task<ErrorOr<OperationResponse<EncryptedInt>>> Accept(EncryptedInt request, int modifiedByUserId)
        => Task.FromResult<ErrorOr<OperationResponse<EncryptedInt>>>(
            Error.Validation("Expense.Accept.Disabled", "Expense approval is performed via report approval workflow."));

    public Task<ErrorOr<OperationResponse<EncryptedInt>>> Reject(ExpenseRejectRequest request)
        => Task.FromResult<ErrorOr<OperationResponse<EncryptedInt>>>(
            Error.Validation("Expense.Reject.Disabled", "Expense rejection is performed via report approval workflow."));
}