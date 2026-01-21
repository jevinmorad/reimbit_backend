using AegisInt.Core;
using Common.Data.Models;
using ErrorOr;
using Reimbit.Contracts.Approvals.Delegation;

namespace Reimbit.Application.Approvals.Delegation;

public interface IDelegationService
{
    Task<ErrorOr<PagedResult<DelegationResponse>>> MyDelegations();
    Task<ErrorOr<OperationResponse<EncryptedInt>>> Create(CreateDelegationRequest request);
    Task<ErrorOr<OperationResponse<EncryptedInt>>> Revoke(RevokeDelegationRequest request, string reason);
    Task<ErrorOr<OperationResponse<EncryptedInt>>> ForceRevoke(RevokeDelegationRequest request, string reason);
}