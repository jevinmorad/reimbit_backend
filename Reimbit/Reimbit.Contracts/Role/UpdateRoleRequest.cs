using System.Text.Json.Serialization;

namespace Reimbit.Contracts.Role;

public class UpdateRoleRequest
{
    public int RoleID { get; set; }
    public required string RoleName { get; set; }
    public string? RoleShortName { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public List<int>? PermissionValues { get; set; }

    [JsonIgnore]
    public int ModifiedByUserID { get; set; }
    [JsonIgnore]
    public DateTime Modified { get; set; }
}