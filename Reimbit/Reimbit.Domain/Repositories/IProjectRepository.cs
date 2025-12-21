using AegisInt.Core;
using Common.Data.Models;
using ErrorOr;
using Reimbit.Contracts.Project;

namespace Reimbit.Domain.Repositories;

public interface IProjectRepository
{
    Task<ErrorOr<OperationResponse<EncryptedInt>>> Insert(InsertRequest request);
    Task<ErrorOr<PagedResult<ListResponse>>> List(int organizationId);
    Task<ErrorOr<OperationResponse<EncryptedInt>>> Update(UpdateRequest request);
    Task<ErrorOr<OperationResponse<EncryptedInt>>> Delete(EncryptedInt projectId);
    Task<ErrorOr<GetResponse>> Get(EncryptedInt projectId);
    Task<ErrorOr<ViewResponse>> View(EncryptedInt projectId);
}
