using Newtonsoft.Json;

namespace Reimbit.Contracts.Account;

public class RegisterRequest
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string DisplayName { get; set; }
    public required string Email { get; set; }
    public required string MobileNo { get; set; }
    public string? ProfileImageUrl { get; set; }
    public required string OrganizationName { get; set; }
    public required string Password { get; set; }
    [JsonIgnore]
    public DateTime Created { get; set; }
}