using AegisInt.Core;
using Common.Data.Models;
using Common.Security;
using ErrorOr;
using Reimbit.Domain.Repositories;
using Reimbit.Core.Models;
using Reimbit.Contracts.Role;

namespace Reimbit.Application.EmployeeManagement.Role;

public class RoleService(
    ICurrentUserProvider currentUserProvider,
    IRoleRepository repository
) : IRoleService
{
    private readonly CurrentUser<TokenData> currentUser = currentUserProvider.GetCurrentUser<TokenData>();

    public async Task<ErrorOr<OperationResponse<EncryptedInt>>> Delete(EncryptedInt RoleID)
        => await repository.Delete(RoleID);

    public async Task<ErrorOr<GetRoleResponse>> Get(EncryptedInt RoleID)
        => await repository.Get(RoleID);

    public async Task<ErrorOr<OperationResponse<EncryptedInt>>> Insert(InsertRoleRequest request)
    {
        request.OrganizationID = currentUser.OrganizationId;
        request.CreatedByUserID = currentUser.UserId;
        request.Created = DateTime.UtcNow;

        return await repository.Insert(request);
    }

    public async Task<ErrorOr<PagedResult<ListRoleResponse>>> List()
        => await repository.List(currentUser.OrganizationId);

    public Task<ErrorOr<IReadOnlyList<OptionsResponse<EncryptedInt>>>> Options()
        => throw new NotImplementedException();

    public async Task<ErrorOr<OperationResponse<EncryptedInt>>> Update(UpdateRoleRequest request)
    {
        request.ModifiedByUserID = currentUser.UserId;
        request.Modified = DateTime.UtcNow;
        return await repository.Update(request);
    }

    public async Task<ErrorOr<ViewRoleResponse>> View(EncryptedInt RoleID)
        => await repository.View(RoleID);

    public async Task<ErrorOr<OperationResponse<EncryptedInt>>> AssignRoleToUser(UserRoleAssignmentRequest request)
    {
        request.CreatedByUserId = currentUser.UserId;
        request.Created = DateTime.UtcNow;
        return await repository.AssignRoleToUser(request);
    }
}
