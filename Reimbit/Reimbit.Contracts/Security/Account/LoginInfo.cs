using AegisInt.Core;

namespace Reimbit.Contracts.Security.Account;

public class LoginInfo
{
    public required int UserId { get; set; }
    public required int OrganizationId { get; set; }
    public required string Email { get; set; }
    public int? RoleId { get; set; }
}
