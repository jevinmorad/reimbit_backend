namespace Reimbit.Domain.Models;

public partial class SecUserRole
{
    public int UserRoleId { get; set; }

    public int? UserId { get; set; }

    public int CreatedByUserId { get; set; }

    public int ModifiedByUserId { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public virtual SecUser CreatedByUser { get; set; } = null!;

    public virtual SecUser ModifiedByUser { get; set; } = null!;

    public virtual SecUser? User { get; set; }
}
