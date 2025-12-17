using AegisInt.Core;

namespace Reimbit.Contracts.Security.User;

public class LoginInfo
{
    public string? DisplayName { get; set; }
    public EncryptedInt? UserID { get; set; }
    public string? Email { get; set; }
    public EncryptedInt? RoleID { get; set; }
    public string? Role { get; set; }
}
