using AegisInt.Core;
using Common.Data.Models;
using ErrorOr;
using Microsoft.EntityFrameworkCore;
using Reimbit.Contracts.Expenses;
using Reimbit.Domain.Interfaces;
using Reimbit.Domain.Models;
using Reimbit.Domain.Repositories;

namespace Reimbit.Infrastructure.Repositories;

public class ExpenseRepository(IApplicationDbContext context) : IExpenseRepository
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
                UserId = request.UserId,
                ProjectId = request.ProjectId,
                CategoryId = request.CategoryId,
                Title = request.Title,
                Amount = request.Amount,
                Currency = request.Currency ?? "INR",
                AttachmentUrl = request.AttachmentUrl ?? "",
                Description = request.Description,
                ExpenseStatus = request.ExpenseStatus,
                CreatedByUserId = request.CreatedByUserId,
                ModifiedByUserId = request.ModifiedByUserId,
                Created = request.Created,
                Modified = request.Modified
            };

            await context.ExpExpenses.AddAsync(expense);
            await context.SaveChangesAsync(default);

            var logExpense = new LogExpExpense
            {
                Iud = "I",
                IuddateTime = request.Created,
                IudbyUserId = request.CreatedByUserId,
                ExpenseId = expense.ExpenseId,
                OrganizationId = expense.OrganizationId,
                UserId = expense.UserId,
                ProjectId = expense.ProjectId,
                CategoryId = expense.CategoryId,
                Title = expense.Title,
                Amount = expense.Amount,
                Currency = expense.Currency,
                AttachmentUrl = expense.AttachmentUrl,
                Description = expense.Description,
                ExpenseStatus = expense.ExpenseStatus,
                CreatedByUserId = expense.CreatedByUserId,
                ModifiedByUserId = expense.ModifiedByUserId,
                Created = expense.Created,
                Modified = expense.Modified
            };

            await context.LogExpExpenses.AddAsync(logExpense);
            await context.SaveChangesAsync(default);

            await tx.CommitAsync();

            response.Id = expense.ExpenseId;
            return response;
        }
        catch (Exception ex)
        {
            await tx.RollbackAsync();
            return Error.Failure("Expense.Insert.Failed", ex.Message);
        }
    }

    public async Task<ErrorOr<PagedResult<ListExpensesResponse>>> ListByUserId(EncryptedInt userId)
    {
        var query = context.ExpExpenses
            .Include(x => x.Project)
            .Include(x => x.Category)
            .Where(x => x.UserId == (int)userId)
            .Select(x => new ListExpensesResponse
            {
                ExpenseId = x.ExpenseId,
                ProjectId = x.ProjectId,
                ProjectName = x.Project.ProjectName,
                CategoryId = x.CategoryId,
                CategoryName = x.Category.CategoryName,
                Title = x.Title,
                Amount = x.Amount,
                Currency = x.Currency,
                ExpenseStatus = x.ExpenseStatus,
                Created = x.Created
            });

        var data = await query.ToListAsync();

        return new PagedResult<ListExpensesResponse>
        {
            Data = data,
            Total = data.Count
        };
    }

    public async Task<ErrorOr<PagedResult<ListExpensesResponse>>> ListByProject(EncryptedInt projectId)
    {
        var query = context.ExpExpenses
            .Include(x => x.Project)
            .Include(x => x.Category)
            .Where(x => x.ProjectId == (int)projectId)
            .Select(x => new ListExpensesResponse
            {
                ExpenseId = x.ExpenseId,
                ProjectId = x.ProjectId,
                ProjectName = x.Project.ProjectName,
                CategoryId = x.CategoryId,
                CategoryName = x.Category.CategoryName,
                Title = x.Title,
                Amount = x.Amount,
                Currency = x.Currency,
                ExpenseStatus = x.ExpenseStatus,
                Created = x.Created
            });

        var data = await query.ToListAsync();

        return new PagedResult<ListExpensesResponse>
        {
            Data = data,
            Total = data.Count
        };
    }

    public async Task<ErrorOr<PagedResult<ListExpensesResponse>>> ListByOrganization(int organizationId)
    {
        var query = context.ExpExpenses
            .Include(x => x.Project)
            .Include(x => x.Category)
            .Where(x => x.OrganizationId == organizationId)
            .Select(x => new ListExpensesResponse
            {
                ExpenseId = x.ExpenseId,
                ProjectId = x.ProjectId,
                ProjectName = x.Project.ProjectName,
                CategoryId = x.CategoryId,
                CategoryName = x.Category.CategoryName,
                Title = x.Title,
                Amount = x.Amount,
                Currency = x.Currency,
                ExpenseStatus = x.ExpenseStatus,
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
            int id = request.ExpenseId;
            var expense = await context.ExpExpenses.FirstOrDefaultAsync(x => x.ExpenseId == id);

            if (expense == null)
            {
                return Error.NotFound("Expense.NotFound", "Expense not found");
            }

            if (expense.ExpenseStatus != "submitted" && expense.ExpenseStatus != "rejected")
            {
                return Error.Validation("Expense.Update.NotAllowed", "Only submitted or rejected expenses can be updated.");
            }

            expense.ProjectId = request.ProjectId;
            expense.CategoryId = request.CategoryId;
            expense.Title = request.Title;
            expense.Amount = request.Amount;
            expense.Currency = request.Currency ?? "INR";
            expense.AttachmentUrl = request.AttachmentUrl ?? "";
            expense.Description = request.Description;
            expense.ExpenseStatus = request.ExpenseStatus;
            expense.ModifiedByUserId = request.ModifiedByUserId;
            expense.Modified = request.Modified;

            var logExpense = new LogExpExpense
            {
                Iud = "U",
                IuddateTime = request.Modified,
                IudbyUserId = request.ModifiedByUserId,
                ExpenseId = expense.ExpenseId,
                OrganizationId = expense.OrganizationId,
                UserId = expense.UserId,
                ProjectId = expense.ProjectId,
                CategoryId = expense.CategoryId,
                Title = expense.Title,
                Amount = expense.Amount,
                Currency = expense.Currency,
                AttachmentUrl = expense.AttachmentUrl,
                Description = expense.Description,
                ExpenseStatus = expense.ExpenseStatus,
                CreatedByUserId = expense.CreatedByUserId,
                ModifiedByUserId = expense.ModifiedByUserId,
                Created = expense.Created,
                Modified = expense.Modified
            };

            await context.LogExpExpenses.AddAsync(logExpense);
            await context.SaveChangesAsync(default);

            await tx.CommitAsync();

            response.Id = expense.ExpenseId;
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
            int id = expenseId;
            var expense = await context.ExpExpenses.FirstOrDefaultAsync(x => x.ExpenseId == id);

            if (expense == null)
            {
                return Error.NotFound("Expense.NotFound", "Expense not found");
            }

            if (expense.ExpenseStatus != "submitted")
            {
                return Error.Validation("Expense.Delete.NotAllowed", "Only submitted expenses can be deleted.");
            }

            context.ExpExpenses.Remove(expense);

            var logExpense = new LogExpExpense
            {
                Iud = "D",
                IuddateTime = DateTime.UtcNow,
                IudbyUserId = 0,
                ExpenseId = expense.ExpenseId,
                OrganizationId = expense.OrganizationId,
                UserId = expense.UserId,
                ProjectId = expense.ProjectId,
                CategoryId = expense.CategoryId,
                Title = expense.Title,
                Amount = expense.Amount,
                Currency = expense.Currency,
                AttachmentUrl = expense.AttachmentUrl,
                Description = expense.Description,
                ExpenseStatus = expense.ExpenseStatus,
                CreatedByUserId = expense.CreatedByUserId,
                ModifiedByUserId = expense.ModifiedByUserId,
                Created = expense.Created,
                Modified = expense.Modified
            };

            await context.LogExpExpenses.AddAsync(logExpense);
            await context.SaveChangesAsync(default);

            await tx.CommitAsync();

            response.Id = expense.ExpenseId;
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
        int id = expenseId;
        var expense = await context.ExpExpenses
            .Include(x => x.Project)
            .Include(x => x.Category)
            .Where(x => x.ExpenseId == id)
            .Select(x => new GetExpenseResponse
            {
                ExpenseId = x.ExpenseId,
                ProjectId = x.ProjectId,
                ProjectName = x.Project.ProjectName,
                CategoryId = x.CategoryId,
                CategoryName = x.Category.CategoryName,
                Title = x.Title,
                Amount = x.Amount,
                Currency = x.Currency,
                AttachmentUrl = x.AttachmentUrl,
                Description = x.Description,
                ExpenseStatus = x.ExpenseStatus,
                RejectionReason = x.RejectionReason,
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
        int id = expenseId;
        var expense = await context.ExpExpenses
            .Include(x => x.Project)
            .Include(x => x.Category)
            .Include(x => x.User)
            .Include(x => x.CreatedByUser)
            .Include(x => x.ModifiedByUser)
            .Where(x => x.ExpenseId == id)
            .Select(x => new ViewExpenseResponse
            {
                ExpenseId = x.ExpenseId,
                Title = x.Title ?? string.Empty,
                Amount = x.Amount,
                Currency = x.Currency,
                Description = x.Description,
                AttachmentUrl = x.AttachmentUrl,
                ExpenseStatus = x.ExpenseStatus,
                RejectionReason = x.RejectionReason,
                ProjectId = x.ProjectId,
                ProjectName = x.Project.ProjectName,
                CategoryId = x.CategoryId,
                CategoryName = x.Category.CategoryName,
                UserDisplayName = $"{x.User.FirstName} {x.User.LastName}",
                CreatedByUserDisplayName = $"{x.CreatedByUser.FirstName} {x.CreatedByUser.LastName}",
                ModifiedByUserDisplayName = $"{x.ModifiedByUser.FirstName} {x.ModifiedByUser.LastName}",
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

    public async Task<ErrorOr<OperationResponse<EncryptedInt>>> Accept(AcceptExpenseRequest request)
    {
        var response = new OperationResponse<EncryptedInt>();
        var dbContext = (DbContext)context;

        await using var tx = await dbContext.Database.BeginTransactionAsync();

        try
        {
            int id = request.ExpenseId;
            var expense = await context.ExpExpenses.FirstOrDefaultAsync(x => x.ExpenseId == id);

            if (expense == null)
            {
                return Error.NotFound("Expense.NotFound", "Expense not found");
            }

            if (expense.ExpenseStatus != "submitted")
            {
                return Error.Validation("Expense.Accept.NotAllowed", "Only submitted expenses can be accepted.");
            }

            expense.ExpenseStatus = "accepted";
            expense.ModifiedByUserId = request.ModifiedByUserId;
            expense.Modified = DateTime.UtcNow;

            var logExpense = new LogExpExpense
            {
                Iud = "U",
                IuddateTime = DateTime.UtcNow,
                IudbyUserId = request.ModifiedByUserId,
                ExpenseId = expense.ExpenseId,
                OrganizationId = expense.OrganizationId,
                UserId = expense.UserId,
                ProjectId = expense.ProjectId,
                CategoryId = expense.CategoryId,
                Title = expense.Title,
                Amount = expense.Amount,
                Currency = expense.Currency,
                AttachmentUrl = expense.AttachmentUrl,
                Description = expense.Description,
                ExpenseStatus = expense.ExpenseStatus,
                CreatedByUserId = expense.CreatedByUserId,
                ModifiedByUserId = expense.ModifiedByUserId,
                Created = expense.Created,
                Modified = expense.Modified
            };

            await context.LogExpExpenses.AddAsync(logExpense);
            await context.SaveChangesAsync(default);

            await tx.CommitAsync();

            response.Id = expense.ExpenseId;
            return response;
        }
        catch (Exception ex)
        {
            await tx.RollbackAsync();
            return Error.Failure("Expense.Accept.Failed", ex.Message);
        }
    }

    public async Task<ErrorOr<OperationResponse<EncryptedInt>>> Reject(RejectExpenseRequest request)
    {
        var response = new OperationResponse<EncryptedInt>();
        var dbContext = (DbContext)context;

        await using var tx = await dbContext.Database.BeginTransactionAsync();

        try
        {
            int id = request.ExpenseId;
            var expense = await context.ExpExpenses.FirstOrDefaultAsync(x => x.ExpenseId == id);

            if (expense == null)
            {
                return Error.NotFound("Expense.NotFound", "Expense not found");
            }

            if (expense.ExpenseStatus != "submitted")
            {
                return Error.Validation("Expense.Reject.NotAllowed", "Only submitted expenses can be rejected.");
            }

            expense.ExpenseStatus = "rejected";
            expense.RejectionReason = request.RejectionReason;
            expense.ModifiedByUserId = request.ModifiedByUserId;
            expense.Modified = DateTime.UtcNow;

            var logExpense = new LogExpExpense
            {
                Iud = "U",
                IuddateTime = DateTime.UtcNow,
                IudbyUserId = request.ModifiedByUserId,
                ExpenseId = expense.ExpenseId,
                OrganizationId = expense.OrganizationId,
                UserId = expense.UserId,
                ProjectId = expense.ProjectId,
                CategoryId = expense.CategoryId,
                Title = expense.Title,
                Amount = expense.Amount,
                Currency = expense.Currency,
                AttachmentUrl = expense.AttachmentUrl,
                Description = expense.Description,
                ExpenseStatus = expense.ExpenseStatus,
                RejectionReason = expense.RejectionReason,
                CreatedByUserId = expense.CreatedByUserId,
                ModifiedByUserId = expense.ModifiedByUserId,
                Created = expense.Created,
                Modified = expense.Modified
            };

            await context.LogExpExpenses.AddAsync(logExpense);
            await context.SaveChangesAsync(default);

            await tx.CommitAsync();

            response.Id = expense.ExpenseId;
            return response;
        }
        catch (Exception ex)
        {
            await tx.RollbackAsync();
            return Error.Failure("Expense.Reject.Failed", ex.Message);
        }
    }
}