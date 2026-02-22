using AegisInt.Core;
using Common.Data.Models;
using ErrorOr;
using Reimbit.Contracts.Policies;
using Reimbit.Domain.Repositories;

namespace Reimbit.Application.Projects.Policies;

public class PoliciesService(IPoliciesRepository repository) : IPoliciesService
{
    public async Task<ErrorOr<OperationResponse<EncryptedInt>>> Insert(PolicyInsertRequest request)
    {
        return await repository.Insert(request);
    }

    public async Task<ErrorOr<PagedResult<PoliciesSelectPageResponse>>> List(EncryptedInt categoryId)
    {
        return await repository.List(categoryId);
    }

    public async Task<ErrorOr<OperationResponse<EncryptedInt>>> Update(PolicyUpdateRequest request)
    {
        return await repository.Update(request);
    }

    public async Task<ErrorOr<OperationResponse<EncryptedInt>>> Delete(EncryptedInt policyId)
    {
        return await repository.Delete(policyId);
    }

    public async Task<ErrorOr<PolicySelectPKResponse>> Get(EncryptedInt policyId)
    {
        return await repository.Get(policyId);
    }
}