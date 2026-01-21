namespace Reimbit.Domain.Models;

public partial class SecDelegateApprover
{
    public int DelegateId { get; set; }

    public int UserId { get; set; }

    public int DelegateUserId { get; set; }

    public DateTime ValidFrom { get; set; }

    public DateTime ValidTo { get; set; }

    public virtual SecUser DelegateUser { get; set; } = null!;

    public virtual SecUser User { get; set; } = null!;
}
