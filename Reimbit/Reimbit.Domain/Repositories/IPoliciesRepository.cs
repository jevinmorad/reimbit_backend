using AegisInt.Core;
using Common.Data.Models;
using ErrorOr;
using Reimbit.Contracts.Policies;

namespace Reimbit.Domain.Repositories;

public interface IPoliciesRepository
{
    Task<ErrorOr<OperationResponse<EncryptedInt>>> Insert(InsertPolicyRequest request);
    Task<ErrorOr<PagedResult<ListPoliciesResponse>>> List(EncryptedInt categoryId);
    Task<ErrorOr<OperationResponse<EncryptedInt>>> Update(UpdatePolicyRequest request);
    Task<ErrorOr<OperationResponse<EncryptedInt>>> Delete(EncryptedInt policyId);
    Task<ErrorOr<GetPolicyResponse>> Get(EncryptedInt policyId);
}
