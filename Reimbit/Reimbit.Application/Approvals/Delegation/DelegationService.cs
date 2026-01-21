using AegisInt.Core;
using Common.Data.Models;
using Common.Security;
using ErrorOr;
using Reimbit.Contracts.Approvals.Delegation;
using Reimbit.Core.Models;
using Reimbit.Domain.Repositories;

namespace Reimbit.Application.Approvals.Delegation;

public sealed class DelegationService(
    ICurrentUserProvider currentUserProvider,
    IDelegationRepository repository
) : IDelegationService
{
    private readonly CurrentUser<TokenData> currentUser = currentUserProvider.GetCurrentUser<TokenData>();

    public Task<ErrorOr<PagedResult<DelegationResponse>>> MyDelegations()
        => repository.MyDelegations(currentUser.OrganizationId, currentUser.UserId);

    public Task<ErrorOr<OperationResponse<EncryptedInt>>> Create(CreateDelegationRequest request)
    {
        request.OrganizationId = currentUser.OrganizationId;
        request.UserId = currentUser.UserId;

        return repository.Create(request);
    }

    public Task<ErrorOr<OperationResponse<EncryptedInt>>> Revoke(RevokeDelegationRequest request, string reason)
    {
        if (string.IsNullOrWhiteSpace(reason))
        {
            return Task.FromResult<ErrorOr<OperationResponse<EncryptedInt>>>(
                Error.Validation("Delegation.Revoke.ReasonRequired", "Revoke reason is required."));
        }

        request.OrganizationId = currentUser.OrganizationId;
        request.UserId = currentUser.UserId;
        request.IsForce = false;

        return repository.Revoke(request, reason);
    }

    public Task<ErrorOr<OperationResponse<EncryptedInt>>> ForceRevoke(RevokeDelegationRequest request, string reason)
    {
        if (string.IsNullOrWhiteSpace(reason))
        {
            return Task.FromResult<ErrorOr<OperationResponse<EncryptedInt>>>(
                Error.Validation("Delegation.Revoke.ReasonRequired", "Revoke reason is required."));
        }

        request.OrganizationId = currentUser.OrganizationId;
        request.UserId = currentUser.UserId;
        request.IsForce = true;

        return repository.Revoke(request, reason);
    }
}