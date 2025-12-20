using System;
using System.Collections.Generic;

namespace Reimbit.Domain.Models;

public partial class LogSecRoleClaim
{
    public int LogorganizationId { get; set; }

    public string Iud { get; set; } = null!;

    public DateTime IuddateTime { get; set; }

    public int IudbyUserId { get; set; }

    public int? RoleClaimId { get; set; }

    public int? RoleId { get; set; }

    public string? Description { get; set; }

    public int? CreatedByUserId { get; set; }

    public int? ModifiedByUserId { get; set; }

    public DateTime? Created { get; set; }

    public DateTime? Modified { get; set; }
}
