using AegisInt.Core;
using Common.Data.Models;
using ErrorOr;
using Reimbit.Contracts.Approvals.Delegation;

namespace Reimbit.Domain.Repositories;

public interface IDelegationRepository
{
    Task<ErrorOr<OperationResponse<EncryptedInt>>> Create(CreateDelegationRequest request);
    Task<ErrorOr<OperationResponse<EncryptedInt>>> Revoke(RevokeDelegationRequest request, string reason);
    Task<ErrorOr<PagedResult<DelegationResponse>>> MyDelegations(int organizationId, int userId);
    Task<int?> ResolveDelegateApproverUserId(int organizationId, int originalApproverUserId, DateTime atUtc);
}