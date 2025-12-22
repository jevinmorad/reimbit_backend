using AegisInt.Core;
using Common.Data.Models;
using ErrorOr;
using Reimbit.Contracts.Project;

namespace Reimbit.Application.Projects.Project;

public interface IProjectService
{
    Task<ErrorOr<OperationResponse<EncryptedInt>>> Insert(InsertProjectRequest request);
    Task<ErrorOr<PagedResult<ListProjectsResponse>>> List();
    Task<ErrorOr<OperationResponse<EncryptedInt>>> Update(UpdateProjectRequest request);
    Task<ErrorOr<GetProjectResponse>> Get(EncryptedInt projectId);
    Task<ErrorOr<ViewProjectResponse>> View(EncryptedInt projectId);
    Task<ErrorOr<OperationResponse<EncryptedInt>>> Delete(EncryptedInt projectId);
}
