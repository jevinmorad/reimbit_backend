namespace Reimbit.Domain.Models;

public partial class SecUserAuth
{
    public int UserId { get; set; }

    public string PasswordHash { get; set; } = null!;

    public string? RefreshToken { get; set; }

    public DateTime RefreshTokenExpiry { get; set; }

    public bool IsRevoked { get; set; }

    public DateTime? LastLogin { get; set; }

    public virtual SecUser User { get; set; } = null!;
}
