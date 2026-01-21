using AegisInt.Core;
using Common.Data.Models;
using ErrorOr;
using Reimbit.Contracts.Role;

namespace Reimbit.Domain.Repositories;

public interface IRoleRepository
{
    Task<ErrorOr<OperationResponse<EncryptedInt>>> Insert(InsertRoleRequest request);
    Task<ErrorOr<OperationResponse<EncryptedInt>>> Update(UpdateRoleRequest request);
    Task<ErrorOr<OperationResponse<EncryptedInt>>> Delete(EncryptedInt roleId);
    Task<ErrorOr<GetRoleResponse>> Get(EncryptedInt roleId);
    Task<ErrorOr<ViewRoleResponse>> View(EncryptedInt roleId);
    Task<ErrorOr<PagedResult<ListRoleResponse>>> List(int organizationId);
    Task<ErrorOr<OperationResponse<EncryptedInt>>> AssignRoleToUser(UserRoleAssignmentRequest request);
}