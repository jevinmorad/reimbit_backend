using AegisInt.Core;
using Common.Data.Models;
using ErrorOr;
using Microsoft.EntityFrameworkCore;
using Reimbit.Contracts.ExpenseCategories;
using Reimbit.Domain.Models;
using Reimbit.Domain.Repositories;
using Reimbit.Infrastructure.Data;

namespace Reimbit.Infrastructure.Repositories;

public class ExpenseCategoryRepository(ApplicationDbContext context) : IExpenseCategoryRepository
{
    public async Task<ErrorOr<OperationResponse<EncryptedInt>>> Insert(InsertExpenseCategoriesRequest request)
    {
        try
        {
            var entity = new ExpCategory
            {
                OrganizationId = request.OrganizationId,
                CategoryName = request.CategoryName,
                Description = request.Description,
                IsActive = true,
                CreatedByUserId = request.CreatedByUserId,
                ModifiedByUserId = request.ModifiedByUserId,
                Created = request.Created,
                Modified = request.Modified
            };

            context.ExpCategories.Add(entity);

            var rowsAffected = await context.SaveChangesAsync(default);

            return new OperationResponse<EncryptedInt>
            {
                Id = entity.CategoryId,
                RowsAffected = rowsAffected
            };
        }
        catch (Exception ex)
        {
            return Error.Failure("ExpenseCategory.Insert.Failed", ex.Message);
        }
    }

    public async Task<ErrorOr<PagedResult<ListExpenseCategoriesResponse>>> List(int userId)
    {
        var list = await context.ExpCategories
            .AsNoTracking()
            .Select(x => new ListExpenseCategoriesResponse
            {
                CategoryId = x.CategoryId,
                CategoryName = x.CategoryName,
                Description = x.Description
            })
            .ToListAsync();

        return new PagedResult<ListExpenseCategoriesResponse>
        {
            Data = list,
            Total = list.Count
        };
    }

    public async Task<ErrorOr<OperationResponse<EncryptedInt>>> Update(UpdateExpenseCategoriesRequest request)
    {
        try
        {
            var entity = await context.ExpCategories.FindAsync((int)request.CategoryId);
            if (entity == null)
            {
                return Error.NotFound("Category.NotFound", "Category not found");
            }

            entity.CategoryName = request.CategoryName;
            entity.Description = request.Description;
            entity.ModifiedByUserId = request.ModifiedByUserId;
            entity.Modified = request.Modified;

            var rowsAffected = await context.SaveChangesAsync(default);

            return new OperationResponse<EncryptedInt>
            {
                Id = entity.CategoryId,
                RowsAffected = rowsAffected
            };
        }
        catch (Exception ex)
        {
            return Error.Failure("ExpenseCategory.Update.Failed", ex.Message);
        }
    }

    public async Task<ErrorOr<OperationResponse<EncryptedInt>>> Delete(EncryptedInt categoryId)
    {
        try
        {
            var entity = await context.ExpCategories.FindAsync((int)categoryId);
            if (entity == null)
            {
                return Error.NotFound("Category.NotFound", "Category not found");
            }

            context.ExpCategories.Remove(entity);

            var rowsAffected = await context.SaveChangesAsync(default);

            await context.SaveChangesAsync(default);

            return new OperationResponse<EncryptedInt>
            {
                Id = entity.CategoryId,
                RowsAffected = rowsAffected
            };
        }
        catch (Exception ex)
        {
            return Error.Failure("ExpenseCategory.Delete.Failed", ex.Message);
        }
    }

    public async Task<ErrorOr<GetExpenseCategoriesResponse>> Get(EncryptedInt categoryId)
    {
        var entity = await context.ExpCategories.FindAsync((int)categoryId);
        if (entity == null)
        {
            return Error.NotFound("Category.NotFound", "Category not found");
        }

        return new GetExpenseCategoriesResponse
        {
            CategoryId = entity.CategoryId,
            CategoryName = entity.CategoryName,
            Description = entity.Description
        };
    }

    public async Task<ErrorOr<List<OptionsResponse<EncryptedInt>>>> SelectComboBox(int organizationId)
    {
        var data = context.ExpCategories
            .AsNoTracking()
            .Where(x => x.OrganizationId == organizationId && x.IsActive)
            .OrderBy(x => x.CategoryName)
            .Select(x => new OptionsResponse<EncryptedInt>
            {
                Value = x.CategoryId,
                Label = x.CategoryName
            })
            .ToList();

        return data;
    }
}