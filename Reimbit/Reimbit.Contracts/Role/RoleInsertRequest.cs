using System.Text.Json.Serialization;

namespace Reimbit.Contracts.Role;

public class RoleInsertRequest
{
    public required string RoleName { get; set; }
    public string? Description { get; set; }
    public List<int>? PermissionValues { get; set; } = null;
    public List<UserRoleAssignmentRequest>? Assignments { get; set; } = null;
    public required DateTime ValidFrom { get; set; } = DateTime.UtcNow;
    public DateTime? ValidTo { get; set; } = null;
    public required bool IsActive { get; set; }
    [JsonIgnore]
    public DateTime Created { get; set; }
    [JsonIgnore]
    public int OrganizationID { get; set; }
    [JsonIgnore]
    public int CreatedByUserID { get; set; }
}