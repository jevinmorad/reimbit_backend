using AegisInt.Core;

namespace Reimbit.Contracts.Finance;

public sealed class PayoutHistoryResponse
{
    public EncryptedInt PayoutId { get; set; }
    public EncryptedInt ReportId { get; set; }
    public decimal? PaidAmount { get; set; }
    public DateTime? PaidOn { get; set; }
    public string? ReferenceNo { get; set; }
}