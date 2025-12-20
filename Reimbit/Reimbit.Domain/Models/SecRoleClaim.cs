using System;
using System.Collections.Generic;

namespace Reimbit.Domain.Models;

public partial class SecRoleClaim
{
    public int RoleClaimId { get; set; }

    public int RoleId { get; set; }

    public string ClaminType { get; set; } = null!;

    public string ClaimValue { get; set; } = null!;

    public int CreatedByUserId { get; set; }

    public int ModifiedByUserId { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public virtual SecUser CreatedByUser { get; set; } = null!;

    public virtual SecUser ModifiedByUser { get; set; } = null!;

    public virtual SecRole Role { get; set; } = null!;
}
