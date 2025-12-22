using AegisInt.Core;

namespace Reimbit.Contracts.Employee;

public class ListEmployeeResponse
{
    public required EncryptedInt UserId { get; set; }
    public required string DisplayName { get; set; }
    public required string Email { get; set; }
    public required string MobileNo { get; set; }
    public required string Role { get; set; }
    public bool IsActive { get; set; }
}
