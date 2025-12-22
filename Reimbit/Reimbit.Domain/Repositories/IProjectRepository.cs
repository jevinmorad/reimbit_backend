using AegisInt.Core;
using Common.Data.Models;
using ErrorOr;
using Reimbit.Contracts.Project;

namespace Reimbit.Domain.Repositories;

public interface IProjectRepository
{
    Task<ErrorOr<OperationResponse<EncryptedInt>>> Insert(InsertProjectRequest request);
    Task<ErrorOr<PagedResult<ListProjectsResponse>>> List(int organizationId);
    Task<ErrorOr<OperationResponse<EncryptedInt>>> Update(UpdateProjectRequest request);
    Task<ErrorOr<OperationResponse<EncryptedInt>>> Delete(EncryptedInt projectId);
    Task<ErrorOr<GetProjectResponse>> Get(EncryptedInt projectId);
    Task<ErrorOr<ViewProjectResponse>> View(EncryptedInt projectId);
}
