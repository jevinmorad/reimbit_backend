using AegisInt.Core;

namespace Reimbit.Contracts.Role;

public class GetRoleResponse
{
    public EncryptedInt RoleID { get; set; }
    public required string RoleName { get; set; }
    public string? Description { get; set; }
}
