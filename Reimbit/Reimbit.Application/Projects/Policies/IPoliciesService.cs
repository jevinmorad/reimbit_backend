using AegisInt.Core;
using Common.Data.Models;
using ErrorOr;
using Reimbit.Contracts.Policies;

namespace Reimbit.Application.Projects.Policies;

public interface IPoliciesService
{
    Task<ErrorOr<OperationResponse<EncryptedInt>>> Insert(InsertPolicyRequest request);
    Task<ErrorOr<PagedResult<ListPoliciesResponse>>> List(EncryptedInt projectId);
    Task<ErrorOr<OperationResponse<EncryptedInt>>> Update(UpdatePolicyRequest request);
    Task<ErrorOr<OperationResponse<EncryptedInt>>> Delete(EncryptedInt policyId);
    Task<ErrorOr<GetPolicyResponse>> Get(EncryptedInt policyId);
}
