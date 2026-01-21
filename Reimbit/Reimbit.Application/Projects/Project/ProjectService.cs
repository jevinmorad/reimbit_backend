using AegisInt.Core;
using Common.Data.Models;
using Common.Security;
using ErrorOr;
using Reimbit.Contracts.Project;
using Reimbit.Core.Models;
using Reimbit.Domain.Repositories;

namespace Reimbit.Application.Projects.Project;

public class ProjectService(
    ICurrentUserProvider currentUserProvider,
    IProjectRepository repository
) : IProjectService
{
    private readonly CurrentUser<TokenData> currentUser = currentUserProvider.GetCurrentUser<TokenData>();

    public async Task<ErrorOr<OperationResponse<EncryptedInt>>> Delete(EncryptedInt projectId)
    {
        var result = await repository.Delete(projectId);
        return result;
    }

    public async Task<ErrorOr<GetProjectResponse>> Get(EncryptedInt projectId)
    {
        var result = await repository.Get(projectId);
        return result;
    }

    public async Task<ErrorOr<OperationResponse<EncryptedInt>>> Insert(InsertProjectRequest request)
    {
        request.OrganizationId = currentUser.OrganizationId;
        request.CreatedByUserId = currentUser.UserId;
        request.Created = DateTime.UtcNow;
        request.IsActive = true;

        var result = await repository.Insert(request);
        return result;
    }

    public async Task<ErrorOr<PagedResult<ListProjectsResponse>>> List()
    {
        var OrganizationId = currentUser.OrganizationId;

        var result = await repository.List(OrganizationId);
        return result;
    }

    public async Task<ErrorOr<OperationResponse<EncryptedInt>>> Update(UpdateProjectRequest request)
    {
        request.OrganizationId = currentUser.OrganizationId;
        request.ModifiedByUserId = currentUser.UserId;
        request.Modified = DateTime.UtcNow;

        var result = await repository.Update(request);
        return result;
    }

    public async Task<ErrorOr<ViewProjectResponse>> View(EncryptedInt projectId)
    {
        var result = await repository.View(projectId);
        return result;
    }
}
