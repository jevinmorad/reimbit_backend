namespace Reimbit.Domain.Models;

public partial class SecUserManager
{
    public int UserManagerId { get; set; }

    public int UserId { get; set; }

    public int ManagerId { get; set; }

    public byte ManagerType { get; set; }

    public bool IsPrimary { get; set; }

    public DateTime ValidFrom { get; set; }

    public DateTime? ValidTo { get; set; }

    public virtual SecUser Manager { get; set; } = null!;

    public virtual SecUser User { get; set; } = null!;
}
