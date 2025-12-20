using System;
using System.Collections.Generic;

namespace Reimbit.Domain.Models;

public partial class ProjProjectMember
{
    public int ProjectMemberId { get; set; }

    public int ProjectId { get; set; }

    public int OrganizationId { get; set; }

    public int UserId { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public virtual OrgOrganization Organization { get; set; } = null!;

    public virtual ProjProject Project { get; set; } = null!;

    public virtual SecUser User { get; set; } = null!;
}
