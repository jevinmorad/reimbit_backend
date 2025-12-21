using AegisInt.Core;
using Common.Data.Models;
using ErrorOr;
using Reimbit.Contracts.Security.Role;

namespace Reimbit.Application.EmployeeManagement.Role;

public interface IRoleService
{
    Task<ErrorOr<OperationResponse<EncryptedInt>>> Delete(EncryptedInt RoleID);
    Task<ErrorOr<OperationResponse<EncryptedInt>>> Insert(InsertRequest request);
    Task<ErrorOr<IReadOnlyList<OptionsResponse<EncryptedInt>>>> Options();
    Task<ErrorOr<PagedResult<ListResponse>>> List(ListRequest request);
    Task<ErrorOr<GetResponse>> Get(EncryptedInt RoleID);
    Task<ErrorOr<ViewResponse>> View(EncryptedInt RoleID);
    Task<ErrorOr<OperationResponse<EncryptedInt>>> Update(UpdateRequest request);
    //Task<ErrorOr<IReadOnlyList<OptionsResponse<EncryptedInt>>>> SelectAutoComplete(RoleAutocompleteRequest request);
}
