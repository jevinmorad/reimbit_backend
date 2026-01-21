using AegisInt.Core;

namespace Reimbit.Contracts.Account;

public class LoginInfo
{
    public required EncryptedInt UserId { get; set; }
    public required EncryptedInt OrganizationId { get; set; }
    public required string Email { get; set; }
    public EncryptedInt? RoleId { get; set; }
}
