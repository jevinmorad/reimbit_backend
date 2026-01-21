using AegisInt.Core;
using Common.Data.Models;
using ErrorOr;
using Reimbit.Contracts.Employee;

namespace Reimbit.Application.EmployeeManagement.Employee;

public interface IEmployeeService
{
    Task<ErrorOr<OperationResponse<EncryptedInt>>> Insert(InsertEmployeeRequest request);
    Task<ErrorOr<PagedResult<ListEmployeeResponse>>> List();
    Task<ErrorOr<ViewEmployeeResponse>> View(EncryptedInt userId);
    Task<ErrorOr<OperationResponse<EncryptedInt>>> AssignEmployeesToManager(AssignEmployeesToManagerRequest request);
}
