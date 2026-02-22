using AegisInt.Core;
using Common.Data.Models;
using ErrorOr;
using Reimbit.Contracts.Role;

namespace Reimbit.Application.EmployeeManagement.Role;

public interface IRoleService
{
    Task<ErrorOr<OperationResponse<EncryptedInt>>> Delete(EncryptedInt RoleID);
    Task<ErrorOr<OperationResponse<EncryptedInt>>> Insert(RoleInsertRequest request);
    Task<ErrorOr<IReadOnlyList<OptionsResponse<EncryptedInt>>>> Options();
    Task<ErrorOr<PagedResult<RoleSelectPaegResponse>>> List();
    Task<ErrorOr<RoleSelectPKResponse>> Get(EncryptedInt RoleID);
    Task<ErrorOr<RoleSelectViewResponse>> View(EncryptedInt RoleID);
    Task<ErrorOr<OperationResponse<EncryptedInt>>> Update(RoleUpdateRequest request);
    Task<ErrorOr<OperationResponse<EncryptedInt>>> AssignRoleToUser(UserRoleAssignmentRequest request);
}
