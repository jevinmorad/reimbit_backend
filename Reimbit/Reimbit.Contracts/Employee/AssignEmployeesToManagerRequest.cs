using AegisInt.Core;
using System.Text.Json.Serialization;

namespace Reimbit.Contracts.Employee;

public sealed class AssignEmployeesToManagerRequest
{
    public required EncryptedInt ManagerId { get; set; }
    public required List<EncryptedInt> EmployeeIds { get; set; }
    public byte ManagerType { get; set; } = 1;
    public bool IsPrimary { get; set; } = true;
    public DateTime? ValidFrom { get; set; }
    public DateTime? ValidTo { get; set; }

    [JsonIgnore]
    public int OrganizationId { get; set; }
    [JsonIgnore]
    public int ModifiedByUserId { get; set; }
}