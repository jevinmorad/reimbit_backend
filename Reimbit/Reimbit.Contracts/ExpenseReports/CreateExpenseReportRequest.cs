using AegisInt.Core;
using System.Text.Json.Serialization;

namespace Reimbit.Contracts.ExpenseReports;

public class CreateExpenseReportRequest
{
    public required DateOnly PeriodStart { get; set; }
    public required DateOnly PeriodEnd { get; set; }
    public required string Title { get; set; }
    public EncryptedInt? ProjectId { get; set; }

    [JsonIgnore]
    public int OrganizationId { get; set; }
    [JsonIgnore]
    public int CreatedByUserId { get; set; }
    [JsonIgnore]
    public int ModifiedByUserId { get; set; }
    [JsonIgnore]
    public DateTime Created { get; set; }
    [JsonIgnore]
    public DateTime Modified { get; set; }
}