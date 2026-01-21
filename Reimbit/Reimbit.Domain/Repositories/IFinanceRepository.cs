using AegisInt.Core;
using Common.Data.Models;
using ErrorOr;
using Reimbit.Contracts.Finance;

namespace Reimbit.Domain.Repositories;

public interface IFinanceRepository
{
    Task<ErrorOr<PagedResult<PayableReportResponse>>> ListPayableReports(int organizationId);
    Task<ErrorOr<OperationResponse<EncryptedInt>>> ProcessPayout(ProcessPayoutRequest request);
    Task<ErrorOr<PagedResult<PayoutHistoryResponse>>> PayoutHistory(int organizationId, EncryptedInt reportId);
    Task<ErrorOr<OperationResponse<EncryptedInt>>> ReversePayout(EncryptedInt reportId, int organizationId, int reversedByUserId, string reason);
}