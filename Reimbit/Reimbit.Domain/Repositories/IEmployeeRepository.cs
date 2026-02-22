using AegisInt.Core;
using Common.Data.Models;
using ErrorOr;
using Reimbit.Contracts.Employee;

namespace Reimbit.Domain.Repositories;

public interface IEmployeeRepository
{
    Task<ErrorOr<OperationResponse<EncryptedInt>>> Insert(EmployeeInsertRequest request);
    Task<ErrorOr<PagedResult<EmployeeSelectPageResponse>>> List(int organizationId);
    Task<ErrorOr<EmployeeSelecttViewResponse>> View(EncryptedInt userId);
    Task<ErrorOr<OperationResponse<EncryptedInt>>> AssignEmployeesToManager(AssignEmployeesToManagerRequest request);
}