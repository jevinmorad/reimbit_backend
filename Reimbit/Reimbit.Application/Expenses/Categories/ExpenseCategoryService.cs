using AegisInt.Core;
using Common.Data.Models;
using Common.Security;
using ErrorOr;
using Reimbit.Contracts.Expenses.Categories;
using Reimbit.Core.Models;
using Reimbit.Domain.Repositories;

namespace Reimbit.Application.Expenses.Categories;

public class ExpenseCategoryService(
    ICurrentUserProvider currentUserProvider,
    IExpenseCategoryRepository repository
) : IExpenseCategoryService
{
    private readonly CurrentUser<TokenData> currentUser = currentUserProvider.GetCurrentUser<TokenData>();

    public async Task<ErrorOr<OperationResponse<EncryptedInt>>> Insert(InsertRequest request)
    {
        request.OrganizationId = currentUser.OrganizationId;
        request.CreatedByUserId = currentUser.UserId;
        request.ModifiedByUserId = currentUser.UserId;
        request.Created = DateTime.UtcNow;
        request.Modified = DateTime.UtcNow;

        return await repository.Insert(request);
    }

    public async Task<ErrorOr<PagedResult<ListResponse>>> List()
    {
        return await repository.List(currentUser.UserId);
    }

    public async Task<ErrorOr<OperationResponse<EncryptedInt>>> Update(UpdateRequest request)
    {
        request.OrganizationId = currentUser.OrganizationId;
        request.ModifiedByUserId = currentUser.UserId;
        request.Modified = DateTime.UtcNow;

        return await repository.Update(request);
    }

    public async Task<ErrorOr<OperationResponse<EncryptedInt>>> Delete(EncryptedInt categoryId)
    {
        return await repository.Delete(categoryId);
    }

    public async Task<ErrorOr<GetResponse>> Get(EncryptedInt categoryId)
    {
        return await repository.Get(categoryId);
    }
}