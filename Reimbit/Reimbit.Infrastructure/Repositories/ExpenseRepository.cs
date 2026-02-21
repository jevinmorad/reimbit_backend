using AegisInt.Core;
using Common.Data.Models;
using ErrorOr;
using Microsoft.EntityFrameworkCore;
using Reimbit.Application.Audit;
using Reimbit.Contracts.Expenses;
using Reimbit.Domain.Interfaces;
using Reimbit.Domain.Models;
using Reimbit.Domain.Repositories;

namespace Reimbit.Infrastructure.Repositories;

public class ExpenseRepository(
    IApplicationDbContext context,
    IAuditLogger auditLogger
) : IExpenseRepository
{
    public async Task<ErrorOr<OperationResponse<EncryptedInt>>> Insert(InsertExpenseRequest request)
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
                Status = (byte)ExpenseStatus.Draft,
                CreatedByUserId = request.CreatedByUserId,
                Created = request.Created,
                Modified = request.Modified
            };

            await context.ExpExpenses.AddAsync(expense);
            var rowsAffected = await context.SaveChangesAsync(default);

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

    private static IQueryable<ListExpensesResponse> ApplySorting(
        IQueryable<ListExpensesResponse> query,
        string? sortField,
        string? sortOrder)
    {
        var desc = string.Equals(sortOrder, "desc", StringComparison.OrdinalIgnoreCase);

        return (sortField ?? string.Empty)
            switch
            {
                nameof(ListExpensesResponse.CategoryName) =>
                    desc ? query.OrderByDescending(x => x.CategoryName) : query.OrderBy(x => x.CategoryName),

                nameof(ListExpensesResponse.Title) =>
                    desc ? query.OrderByDescending(x => x.Title) : query.OrderBy(x => x.Title),

                nameof(ListExpensesResponse.Amount) =>
                    desc ? query.OrderByDescending(x => x.Amount) : query.OrderBy(x => x.Amount),

                nameof(ListExpensesResponse.Currency) =>
                    desc ? query.OrderByDescending(x => x.Currency) : query.OrderBy(x => x.Currency),

                nameof(ListExpensesResponse.Created) =>
                    desc ? query.OrderByDescending(x => x.Created) : query.OrderBy(x => x.Created),

                _ => query.OrderByDescending(x => x.Created)
            };
    }

    public async Task<ErrorOr<PagedResult<ListExpensesResponse>>> SelectPaage(ListExpenseRequest request)
    {
        var baseQuery = context.ExpExpenses
            .AsNoTracking()
            .Include(x => x.Category)
            .Where(x => (!request.UserID.HasValue || x.EmployeeId == request.UserID))
            .Select(x => new ListExpensesResponse
            {
                ExpenseId = x.ExpenseId,
                CategoryId = x.CategoryId,
                CategoryName = x.Category.CategoryName,
                Title = x.Title,
                Amount = x.Amount,
                Currency = x.Currency,
                Status = ((ExpenseStatus)x.Status).ToString(),
                Created = x.Created
            });

        var total = await baseQuery.CountAsync();

        var query = ApplySorting(baseQuery, request.SortField, request.SortOrder);

        var data = await query
            .Skip(request.PageOffset)
            .Take(request.PageSize)
            .ToListAsync();

        return new PagedResult<ListExpensesResponse>
        {
            Data = data,
            Total = total
        };
    }

    public async Task<ErrorOr<PagedResult<ListExpensesResponse>>> ListByOrganization(int organizationId)
    {
        var query = context.ExpExpenses
            .AsNoTracking()
            .Include(x => x.Category)
            .Where(x => x.OrganizationId == organizationId)
            .Select(x => new ListExpensesResponse
            {
                ExpenseId = x.ExpenseId,
                CategoryId = x.CategoryId,
                CategoryName = x.Category.CategoryName,
                Title = x.Title,
                Amount = x.Amount,
                Currency = x.Currency,
                Status = ((ExpenseStatus)x.Status).ToString(),
                Created = x.Created
            });

        var data = await query.ToListAsync();

        return new PagedResult<ListExpensesResponse>
        {
            Data = data,
            Total = data.Count
        };
    }

    public async Task<ErrorOr<OperationResponse<EncryptedInt>>> Update(UpdateExpenseRequest request)
    {
        var response = new OperationResponse<EncryptedInt>();
        var dbContext = (DbContext)context;

        await using var tx = await dbContext.Database.BeginTransactionAsync();

        try
        {

            var expense = await context.ExpExpenses.FirstOrDefaultAsync(x =>
                x.OrganizationId == request.OrganizationId && x.ExpenseId == request.ExpenseId.Value);

            if (expense == null)
            {
                return Error.NotFound("Expense.NotFound", "Expense not found");
            }

            if (expense.Status != (byte)ExpenseStatus.Draft && expense.Status != (byte)ExpenseStatus.Rejected)
            {
                return Error.Validation("Expense.Update.NotAllowed", "Only draft/rejected expenses can be updated.");
            }

            var oldValue = new
            {
                expense.CategoryId,
                expense.Title,
                expense.Amount,
                expense.Currency,
                expense.ReceiptUrl,
                expense.Description,
                expense.Status,
                expense.Modified
            };

            expense.CategoryId = request.CategoryId.Value;
            expense.Title = request.Title;
            expense.Amount = request.Amount;
            expense.Currency = request.Currency ?? "INR";
            expense.ReceiptUrl = request.ReceiptUrl ?? string.Empty;
            expense.Description = request.Description;
            expense.Modified = request.Modified;

            var rowsAffected = await context.SaveChangesAsync(default);

            await auditLogger.WriteAsync(
                entityType: "EXP_Expense",
                entityId: expense.ExpenseId,
                action: "UPDATE",
                organizationId: expense.OrganizationId,
                userId: request.ModifiedByUserId,
                oldValue: oldValue,
                newValue: new
                {
                    expense.CategoryId,
                    expense.Title,
                    expense.Amount,
                    expense.Currency,
                    expense.ReceiptUrl,
                    expense.Description,
                    expense.Status,
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
            return Error.Failure("Expense.Update.Failed", ex.Message);
        }
    }

    public async Task<ErrorOr<OperationResponse<EncryptedInt>>> Delete(EncryptedInt expenseId)
    {
        var response = new OperationResponse<EncryptedInt>();
        var dbContext = (DbContext)context;

        await using var tx = await dbContext.Database.BeginTransactionAsync();

        try
        {
            var expense = await context.ExpExpenses.FirstOrDefaultAsync(x => x.ExpenseId == expenseId.Value);

            if (expense == null)
            {
                return Error.NotFound("Expense.NotFound", "Expense not found");
            }

            if (expense.Status != (byte)ExpenseStatus.Draft)
            {
                return Error.Validation("Expense.Delete.NotAllowed", "Only draft expenses can be deleted.");
            }

            context.ExpExpenses.Remove(expense);

            var rowsAffected = await context.SaveChangesAsync(default);

            await auditLogger.WriteAsync(
                entityType: "EXP_Expense",
                entityId: expense.ExpenseId,
                action: "DELETE",
                organizationId: expense.OrganizationId,
                userId: expense.EmployeeId,
                oldValue: new
                {
                    expense.ExpenseId,
                    expense.OrganizationId,
                    expense.EmployeeId,
                    expense.CategoryId,
                    expense.Title,
                    expense.Amount,
                    expense.Currency,
                    expense.ReceiptUrl,
                    expense.Description,
                    expense.Status,
                    expense.Created,
                    expense.Modified
                },
                newValue: null,
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
            return Error.Failure("Expense.Delete.Failed", ex.Message);
        }
    }

    public async Task<ErrorOr<GetExpenseResponse>> Get(EncryptedInt expenseId)
    {
        var expense = await context.ExpExpenses
            .AsNoTracking()
            .Include(x => x.Category)
            .Where(x => x.ExpenseId == expenseId.Value)
            .Select(x => new GetExpenseResponse
            {
                ExpenseId = x.ExpenseId,
                CategoryId = x.CategoryId,
                Title = x.Title,
                Amount = x.Amount,
                Currency = x.Currency,
                ReceiptUrl = x.ReceiptUrl,
                Description = x.Description,
                Status = ((ExpenseStatus)x.Status).ToString(),
                Created = x.Created
            })
            .FirstOrDefaultAsync();

        if (expense == null)
        {
            return Error.NotFound("Expense.NotFound", "Expense not found");
        }

        return expense;
    }

    public async Task<ErrorOr<ViewExpenseResponse>> View(EncryptedInt expenseId)
    {
        var expense = await context.ExpExpenses
            .AsNoTracking()
            .Include(x => x.Category)
            .Include(x => x.Employee)
            .Include(x => x.CreatedByUser)
            .Where(x => x.ExpenseId == expenseId.Value)
            .Select(x => new ViewExpenseResponse
            {
                Title = x.Title,
                Amount = x.Amount,
                Currency = x.Currency,
                Description = x.Description,
                AttachmentUrl = x.ReceiptUrl,
                ExpenseStatus = ((ExpenseStatus)x.Status).ToString(),
                RejectionReason = x.ExpExpenseRejections
                    .OrderByDescending(r => r.RejectedAt)
                    .Select(r => r.Reason)
                    .FirstOrDefault(),
                CategoryName = x.Category.CategoryName,
                UserDisplayName = x.Employee.DisplayName,
                CreatedByUserDisplayName = x.CreatedByUser.DisplayName,
                ModifiedByUserDisplayName = null,
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

    public Task<ErrorOr<OperationResponse<EncryptedInt>>> Reject(RejectExpenseRequest request)
        => Task.FromResult<ErrorOr<OperationResponse<EncryptedInt>>>(
            Error.Validation("Expense.Reject.Disabled", "Expense rejection is performed via report approval workflow."));
}