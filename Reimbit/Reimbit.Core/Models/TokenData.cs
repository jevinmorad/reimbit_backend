namespace Reimbit.Core.Models;

public class TokenData
{
    public int UserId { get; set; }
    public int OrganizationId { get; set; }
    public required string Email { get; set; }
    public int RoleId { get; set; }
}
