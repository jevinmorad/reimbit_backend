using AegisInt.Core;
using Common.Data.Models;
using ErrorOr;
using Reimbit.Contracts.Security.Role;

namespace Reimbit.Application.EmployeeManagement.Role;

public class RoleService : IRoleService
{
    public Task<ErrorOr<OperationResponse<EncryptedInt>>> Delete(EncryptedInt RoleID)
    {
        throw new NotImplementedException();
    }

    public Task<ErrorOr<GetResponse>> Get(EncryptedInt RoleID)
    {
        throw new NotImplementedException();
    }

    public Task<ErrorOr<OperationResponse<EncryptedInt>>> Insert(InsertRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<ErrorOr<PagedResult<ListResponse>>> List(ListRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<ErrorOr<IReadOnlyList<OptionsResponse<EncryptedInt>>>> Options()
    {
        throw new NotImplementedException();
    }

    public Task<ErrorOr<OperationResponse<EncryptedInt>>> Update(UpdateRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<ErrorOr<ViewResponse>> View(EncryptedInt RoleID)
    {
        throw new NotImplementedException();
    }
}
