using AegisInt.Core;
using Common.Data.Models;
using ErrorOr;
using Reimbit.Contracts.Role;

namespace Reimbit.Application.EmployeeManagement.Role;

public interface IRoleService
{
    Task<ErrorOr<OperationResponse<EncryptedInt>>> Delete(EncryptedInt RoleID);
    Task<ErrorOr<OperationResponse<EncryptedInt>>> Insert(InsertRoleRequest request);
    Task<ErrorOr<IReadOnlyList<OptionsResponse<EncryptedInt>>>> Options();
    Task<ErrorOr<PagedResult<ListRoleResponse>>> List();
    Task<ErrorOr<GetRoleResponse>> Get(EncryptedInt RoleID);
    Task<ErrorOr<ViewRoleResponse>> View(EncryptedInt RoleID);
    Task<ErrorOr<OperationResponse<EncryptedInt>>> Update(UpdateRoleRequest request);
    Task<ErrorOr<OperationResponse<EncryptedInt>>> AssignRoleToUser(UserRoleAssignmentRequest request);
}
