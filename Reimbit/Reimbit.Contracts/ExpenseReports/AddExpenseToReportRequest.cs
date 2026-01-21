using AegisInt.Core;
using System.Text.Json.Serialization;

namespace Reimbit.Contracts.ExpenseReports;

public class AddExpenseToReportRequest
{
    public required EncryptedInt ReportId { get; set; }
    public required EncryptedInt ExpenseId { get; set; }

    [JsonIgnore]
    public int OrganizationId { get; set; }
    [JsonIgnore]
    public int UserId { get; set; }
}