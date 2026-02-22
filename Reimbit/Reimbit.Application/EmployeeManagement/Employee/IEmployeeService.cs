using AegisInt.Core;
using Common.Data.Models;
using ErrorOr;
using Reimbit.Contracts.Employee;

namespace Reimbit.Application.EmployeeManagement.Employee;

public interface IEmployeeService
{
    Task<ErrorOr<OperationResponse<EncryptedInt>>> Insert(EmployeeInsertRequest request);
    Task<ErrorOr<PagedResult<EmployeeSelectPageResponse>>> List();
    Task<ErrorOr<EmployeeSelecttViewResponse>> View(EncryptedInt userId);
    Task<ErrorOr<OperationResponse<EncryptedInt>>> AssignEmployeesToManager(AssignEmployeesToManagerRequest request);
}
