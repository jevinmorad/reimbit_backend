using AegisInt.Core;
using Common.Data.Models;
using ErrorOr;
using Reimbit.Contracts.Role;

namespace Reimbit.Domain.Repositories;

public interface IRoleRepository
{
    Task<ErrorOr<OperationResponse<EncryptedInt>>> Insert(RoleInsertRequest request);
    Task<ErrorOr<OperationResponse<EncryptedInt>>> Update(RoleUpdateRequest request);
    Task<ErrorOr<OperationResponse<EncryptedInt>>> Delete(EncryptedInt roleId);
    Task<ErrorOr<RoleSelectPKResponse>> Get(EncryptedInt roleId);
    Task<ErrorOr<RoleSelectViewResponse>> View(EncryptedInt roleId);
    Task<ErrorOr<PagedResult<RoleSelectPaegResponse>>> List(int organizationId);
    Task<ErrorOr<OperationResponse<EncryptedInt>>> AssignRoleToUser(UserRoleAssignmentRequest request);
}