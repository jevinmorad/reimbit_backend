using AegisInt.Core;
using Common.Data.Models;
using ErrorOr;
using Reimbit.Contracts.Project;

namespace Reimbit.Application.Projects.Project;

public interface IProjectService
{
    Task<ErrorOr<OperationResponse<EncryptedInt>>> Insert(InsertRequest request);
    Task<ErrorOr<PagedResult<ListResponse>>> List();
    Task<ErrorOr<OperationResponse<EncryptedInt>>> Update(UpdateRequest request);
    Task<ErrorOr<GetResponse>> Get(EncryptedInt projectId);
    Task<ErrorOr<ViewResponse>> View(EncryptedInt projectId);
    Task<ErrorOr<OperationResponse<EncryptedInt>>> Delete(EncryptedInt projectId);
}
