using AegisInt.Core;
using System.Text.Json.Serialization;

namespace Reimbit.Contracts.ExpenseReports;

public class SubmitExpenseReportRequest
{
    public required EncryptedInt ReportId { get; set; }

    [JsonIgnore]
    public int OrganizationId { get; set; }
    [JsonIgnore]
    public int UserId { get; set; }
}