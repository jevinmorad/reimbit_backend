using AegisInt.Core;
using Common.Data.Models;
using ErrorOr;
using Reimbit.Contracts.Employee;

namespace Reimbit.Domain.Repositories;

public interface IEmployeeRepository
{
    Task<ErrorOr<OperationResponse<EncryptedInt>>> Insert(InsertEmployeeRequest request);
    Task<ErrorOr<PagedResult<ListEmployeeResponse>>> List(int organizationId);
}