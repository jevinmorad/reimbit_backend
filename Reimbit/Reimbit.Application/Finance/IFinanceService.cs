using AegisInt.Core;
using Common.Data.Models;
using ErrorOr;
using Reimbit.Contracts.Finance;

namespace Reimbit.Application.Finance;

public interface IFinanceService
{
    Task<ErrorOr<PagedResult<PayableReportResponse>>> PayableReports();
    Task<ErrorOr<OperationResponse<EncryptedInt>>> ProcessPayout(ProcessPayoutRequest request);
    Task<ErrorOr<PagedResult<PayoutHistoryResponse>>> PayoutHistory(EncryptedInt reportId);
    Task<ErrorOr<OperationResponse<EncryptedInt>>> ReversePayout(EncryptedInt reportId, string reason);
}