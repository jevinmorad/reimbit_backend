using AegisInt.Core;
using System.Text.Json.Serialization;

namespace Reimbit.Contracts.Employee;

public class EmployeeInsertRequest
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string DisplayName { get; set; }
    public required string Email { get; set; }
    public required string MobileNo { get; set; }
    public string? ProfileImageUrl { get; set; }
    public required EncryptedInt RoleId { get; set; }
    public EncryptedInt? ManagerId { get; set; }
    public byte? ManagerType { get; set; } = 1;
    public bool IsPrimaryManager { get; set; } = true;
    public DateTime? ManagerValidFrom { get; set; } = DateTime.UtcNow;
    public DateTime? ManagerValidTo { get; set; } = null;
    public required bool IsActive { get; set; }

    [JsonIgnore]
    public EncryptedInt? UserId { get; set; }
    [JsonIgnore]
    public int OrganizationId { get; set; }
    [JsonIgnore]
    public int CreatedByUserId { get; set; }
    [JsonIgnore]
    public DateTime Created { get; set; }
}