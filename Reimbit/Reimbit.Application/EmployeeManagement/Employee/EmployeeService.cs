using AegisInt.Core;
using Common.Data.Models;
using Common.Security;
using ErrorOr;
using Reimbit.Contracts.Employee;
using Reimbit.Core.Models;
using Reimbit.Domain.Repositories;

namespace Reimbit.Application.EmployeeManagement.Employee;

public class EmployeeService(
    ICurrentUserProvider currentUserProvider,
    IEmployeeRepository repository
) : IEmployeeService
{
    private readonly CurrentUser<TokenData> currentUser = currentUserProvider.GetCurrentUser<TokenData>();

    public Task<ErrorOr<OperationResponse<EncryptedInt>>> AssignEmployeesToManager(AssignEmployeesToManagerRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task<ErrorOr<OperationResponse<EncryptedInt>>> Insert(EmployeeInsertRequest request)
    {
        request.OrganizationId = currentUser.OrganizationId;
        request.CreatedByUserId = currentUser.UserId;
        request.Created = DateTime.UtcNow;

        var result = await repository.Insert(request);
        return result;
    }

    public async Task<ErrorOr<PagedResult<EmployeeSelectPageResponse>>> List() => await repository.List(currentUser.OrganizationId);

    public async Task<ErrorOr<EmployeeSelecttViewResponse>> View(EncryptedInt userId) => await repository.View(userId);
}
