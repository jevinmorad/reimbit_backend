namespace Reimbit.Domain.Models;

public partial class SecRole
{
    public int RoleId { get; set; }

    public string RoleName { get; set; } = null!;

    public int? OrganizationId { get; set; }

    public string? Description { get; set; }

    public int CreatedByUserId { get; set; }

    public int ModifiedByUserId { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public bool? IsActive { get; set; }

    public virtual SecUser CreatedByUser { get; set; } = null!;

    public virtual SecUser ModifiedByUser { get; set; } = null!;

    public virtual OrgOrganization? Organization { get; set; }

    public virtual ICollection<SecRoleClaim> SecRoleClaims { get; set; } = new List<SecRoleClaim>();
}
