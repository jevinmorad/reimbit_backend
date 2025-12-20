namespace Reimbit.Contracts.Security.Account;

public class RegisterRequest
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string MobileNo { get; set; }
    public string? UserProfileImageUrl { get; set; }
    public required string OrganizationName { get; set; }
    public required string Password { get; set; }
}
