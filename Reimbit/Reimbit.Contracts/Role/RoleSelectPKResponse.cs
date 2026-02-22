using AegisInt.Core;

namespace Reimbit.Contracts.Role;

public class RoleSelectPKResponse
{
    public EncryptedInt RoleID { get; set; }
    public required string RoleName { get; set; }
    public string? Description { get; set; }
}
