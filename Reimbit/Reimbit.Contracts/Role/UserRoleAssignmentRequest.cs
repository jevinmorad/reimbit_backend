using AegisInt.Core;
using System.Text.Json.Serialization;

namespace Reimbit.Contracts.Role;

public class UserRoleAssignmentRequest
{
    public required EncryptedInt UserId { get; set; }
    public required EncryptedInt RoleId { get; set; }
    public required DateTime ValidFrom { get; set; }
    public DateTime? ValidTo { get; set; }

    [JsonIgnore]
    public int CreatedByUserId { get; set; }
    [JsonIgnore]
    public DateTime Created { get; set; }
}
