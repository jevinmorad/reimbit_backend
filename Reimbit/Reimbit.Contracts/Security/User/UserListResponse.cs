using AegisInt.Core;

namespace Reimbit.Contracts.Security.User;

public class UserListResponse
{
    public EncryptedInt? UserID { get; set; }
    public string? DisplayName { get; set; }
    public string? Email { get; set; }
    public string? MobileNo { get; set; }
    public bool IsActive { get; set; }
}
