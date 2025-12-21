using AegisInt.Core;
using Common.Data.Models;
using ErrorOr;
using Reimbit.Contracts.Policies;
using Reimbit.Domain.Repositories;

namespace Reimbit.Application.Projects.Policies;

public class PoliciesService(IPoliciesRepository repository) : IPoliciesService
{
    public async Task<ErrorOr<OperationResponse<EncryptedInt>>> Insert(InsertRequest request)
    {
        return await repository.Insert(request);
    }

    public async Task<ErrorOr<PagedResult<ListResponse>>> List(EncryptedInt projectId)
    {
        return await repository.List(projectId);
    }

    public async Task<ErrorOr<OperationResponse<EncryptedInt>>> Update(UpdateRequest request)
    {
        return await repository.Update(request);
    }

    public async Task<ErrorOr<OperationResponse<EncryptedInt>>> Delete(EncryptedInt policyId)
    {
        return await repository.Delete(policyId);
    }

    public async Task<ErrorOr<GetResponse>> Get(EncryptedInt policyId)
    {
        return await repository.Get(policyId);
    }
}