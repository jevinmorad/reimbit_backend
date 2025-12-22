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

    public async Task<ErrorOr<OperationResponse<EncryptedInt>>> Insert(InsertEmployeeRequest request)
    {
        request.OrganizationId = currentUser.OrganizationId;
        request.CreatedByUserId = currentUser.UserId;
        request.Created = DateTime.Now;
        request.Modified = DateTime.Now;
        request.IsActive = true;

        var result = await repository.Insert(request);
        return result;
    }

    public async Task<ErrorOr<PagedResult<ListEmployeeResponse>>> List()
    {
        int OrganizationId = currentUser.OrganizationId;
        var result = await repository.List(OrganizationId);
        return result;
    }
}
