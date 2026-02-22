using AegisInt.Core;
using Common.Data.Models;
using ErrorOr;
using Reimbit.Contracts.ExpenseReports;

namespace Reimbit.Application.ExpenseReports;

public interface IExpenseReportService
{
    Task<ErrorOr<OperationResponse<EncryptedInt>>> Create(ExpenseReportInsertRequest request);
    Task<ErrorOr<OperationResponse<EncryptedInt>>> AddExpense(AddExpenseToReportRequest request);
    Task<ErrorOr<OperationResponse<EncryptedInt>>> RemoveExpense(RemoveExpenseFromReportRequest request);
    Task<ErrorOr<OperationResponse<EncryptedInt>>> Submit(SubmitExpenseReportRequest request);
    Task<ErrorOr<ExpenseReportSelectPkResponse>> Get(EncryptedInt reportId);
}