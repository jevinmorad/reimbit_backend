using AegisInt.Core;

namespace Reimbit.Contracts.Role;

public class ViewRoleResponse
{
    public required string RoleName { get; set; }
    public string? Description { get; set; }
    public required int TotalUserCount { get; set; }
    public required int ActiveUserCount { get; set; }
    public required int InactiveUserCount { get; set; }
    public required string CreatedByUserName { get; set; }
    public string? ModifiedByUserName { get; set; }
    public DateTime Created { get; set; }
    public DateTime? Modified { get; set; }
}