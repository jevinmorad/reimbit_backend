using AegisInt.Core;
using System.Text.Json.Serialization;

namespace Reimbit.Contracts.Finance;

public sealed class ProcessPayoutRequest
{
    public required EncryptedInt ReportId { get; set; }
    public required decimal PaidAmount { get; set; }
    public required DateTime PaidOn { get; set; }
    public required string ReferenceNo { get; set; }

    [JsonIgnore]
    public int OrganizationId { get; set; }

    [JsonIgnore]
    public int ProcessedByUserId { get; set; }
}