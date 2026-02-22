using AegisInt.Core;
using Common.Data.Models;
using ErrorOr;
using Reimbit.Contracts.Policies;

namespace Reimbit.Domain.Repositories;

public interface IPoliciesRepository
{
    Task<ErrorOr<OperationResponse<EncryptedInt>>> Insert(PolicyInsertRequest request);
    Task<ErrorOr<PagedResult<PoliciesSelectPageResponse>>> List(EncryptedInt categoryId);
    Task<ErrorOr<OperationResponse<EncryptedInt>>> Update(PolicyUpdateRequest request);
    Task<ErrorOr<OperationResponse<EncryptedInt>>> Delete(EncryptedInt policyId);
    Task<ErrorOr<PolicySelectPKResponse>> Get(EncryptedInt policyId);
}
