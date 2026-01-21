using AegisInt.Core;

namespace Reimbit.Contracts.Approvals;

public sealed class ApprovalInboxItemResponse
{
    public EncryptedInt ApprovalInstanceId { get; set; }
    public EncryptedInt ReportId { get; set; }
    public string ReportTitle { get; set; }
    public decimal TotalAmount { get; set; }
    public byte ReportStatus { get; set; }
    public int LevelOrder { get; set; }
    public DateTime? ActionAt { get; set; }
}