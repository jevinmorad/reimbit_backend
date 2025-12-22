using AegisInt.Core;
using Common.Data.Models;
using ErrorOr;
using Microsoft.EntityFrameworkCore;
using Reimbit.Contracts.Expenses.Categories;
using Reimbit.Domain.Models;
using Reimbit.Domain.Repositories;
using Reimbit.Infrastructure.Data;

namespace Reimbit.Infrastructure.Repositories;

public class ExpenseCategoryRepository(ApplicationDbContext context) : IExpenseCategoryRepository
{
    public async Task<ErrorOr<OperationResponse<EncryptedInt>>> Insert(InsertRequest request)
    {
        var entity = new ExpCategory
        {
            OrganizationId = request.OrganizationId,
            ProjectId = request.ProjectId,
            CategoryName = request.CategoryName,
            Description = request.Description,
            CreatedByUserId = request.CreatedByUserId,
            ModifiedByUserId = request.ModifiedByUserId,
            Created = request.Created,
            Modified = request.Modified
        };

        context.ExpCategories.Add(entity);
        await context.SaveChangesAsync();

        return new OperationResponse<EncryptedInt>
        {
            Id = entity.CategoryId,
        };
    }

    public async Task<ErrorOr<PagedResult<ListResponse>>> List(int userId)
    {
        var query = from c in context.ExpCategories
                    join p in context.ProjProjects on c.ProjectId equals p.ProjectId
                    select new ListResponse
                    {
                        CategoryId = c.CategoryId,
                        ProjectId = c.ProjectId,
                        CategoryName = c.CategoryName,
                        Description = c.Description
                    };

        var list = await query.ToListAsync();
        
        return new PagedResult<ListResponse>
        {
            Data = list,
            Total= list.Count
        };
    }

    public async Task<ErrorOr<OperationResponse<EncryptedInt>>> Update(UpdateRequest request)
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

        await context.SaveChangesAsync();

        return new OperationResponse<EncryptedInt>
        {
            Id = entity.CategoryId,
        };
    }

    public async Task<ErrorOr<OperationResponse<EncryptedInt>>> Delete(EncryptedInt categoryId)
    {
        var entity = await context.ExpCategories.FindAsync((int)categoryId);
        if (entity == null)
        {
            return Error.NotFound("Category.NotFound", "Category not found");
        }

        context.ExpCategories.Remove(entity);
        await context.SaveChangesAsync();

        return new OperationResponse<EncryptedInt>
        {
            Id = entity.CategoryId,
            };
    }

    public async Task<ErrorOr<GetResponse>> Get(EncryptedInt categoryId)
    {
        var entity = await context.ExpCategories.FindAsync((int)categoryId);
        if (entity == null)
        {
            return Error.NotFound("Category.NotFound", "Category not found");
        }

        return new GetResponse
        {
            CategoryId = entity.CategoryId,
            ProjectId = entity.ProjectId,
            CategoryName = entity.CategoryName,
            Description = entity.Description
        };
    }
}