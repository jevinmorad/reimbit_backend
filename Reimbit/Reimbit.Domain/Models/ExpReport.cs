using System;
using System.Collections.Generic;

namespace Reimbit.Domain.Models;

public partial class ExpReport
{
    public int ReportId { get; set; }

    public int OrganizationId { get; set; }

    public int ManagerId { get; set; }

    public int ProjectId { get; set; }

    public string Title { get; set; } = null!;

    public string ReportStatus { get; set; } = null!;

    public decimal AcceptedAmount { get; set; }

    public decimal RejectedAmount { get; set; }

    public DateTime? ViewedAt { get; set; }

    public int CreatedByUserId { get; set; }

    public int ModifiedByUserId { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public virtual SecUser CreatedByUser { get; set; } = null!;

    public virtual SecUser Manager { get; set; } = null!;

    public virtual SecUser ModifiedByUser { get; set; } = null!;

    public virtual OrgOrganization Organization { get; set; } = null!;

    public virtual ProjProject Project { get; set; } = null!;
}
