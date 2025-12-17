using AegisInt.Core;

namespace Reimbit.Contracts.Security.User;

public class UserInsertRequest
{
    public EncryptedInt? UserID { get; set; }
    public EncryptedInt? RoleID { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string UserName { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public string? MobileNo { get; set; }
    public int CreatedByUserID { get; set; }
}
