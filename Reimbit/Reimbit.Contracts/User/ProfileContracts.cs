using AegisInt.Core;

namespace Reimbit.Contracts.User;

public class UserResponse
{
    public EncryptedInt UserId { get; set; }
    public required string DisplayName { get; set; }
    public required string Email { get; set; }
    public string? MobileNo { get; set; }
    public string? ProfileImageUrl { get; set; }
    public bool IsActive { get; set; }
    public DateTime Created { get; set; }
    public DateTime? Modified { get; set; }
}

public class UpdateProfileRequest
{
    public int UserId { get; set; }
    public string? DisplayName { get; set; }
    public string? MobileNo { get; set; }
    public string? ProfileImageUrl { get; set; }

    public int? ModifiedByUserId { get; set; }
    public DateTime? Modified { get; set; }
}

public class PermissionsResponse
{
    public required Dictionary<string, bool> Permissions { get; set; }
}

public class InfoResponse
{
    public required UserResponse User { get; set; }
    public required Dictionary<string, bool> Permissions { get; set; }
}