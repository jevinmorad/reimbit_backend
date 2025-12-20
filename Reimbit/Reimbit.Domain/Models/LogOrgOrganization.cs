using System;
using System.Collections.Generic;

namespace Reimbit.Domain.Models;

public partial class LogOrgOrganization
{
    public int LogorganizationId { get; set; }

    public string Iud { get; set; } = null!;

    public DateTime IuddateTime { get; set; }

    public int IudbyUserId { get; set; }

    public int? OrganizationId { get; set; }

    public string? OrganizationName { get; set; }

    public DateTime? Created { get; set; }

    public DateTime? Modified { get; set; }
}
