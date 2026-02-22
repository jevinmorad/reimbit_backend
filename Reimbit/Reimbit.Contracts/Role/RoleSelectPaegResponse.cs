using AegisInt.Core;

namespace Reimbit.Contracts.Role;

public class RoleSelectPaegResponse
{
    public required EncryptedInt RoleID { get; set; }
    public required string RoleName { get; set; }
    public required int ActiveUserCount { get; set; }
    public required int InactiveUserCount { get; set; }
    public required int TotalUserCount { get; set; }
}