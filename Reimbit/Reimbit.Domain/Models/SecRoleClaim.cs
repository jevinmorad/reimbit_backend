namespace Reimbit.Domain.Models;

public partial class SecRoleClaim
{
    public int RoleClaimId { get; set; }

    public int RoleId { get; set; }

    public int ClaimValue { get; set; }

    public int CreatedByUserId { get; set; }

    public DateTime Created { get; set; }

    public virtual SecUser CreatedByUser { get; set; } = null!;

    public virtual SecRole Role { get; set; } = null!;
}
