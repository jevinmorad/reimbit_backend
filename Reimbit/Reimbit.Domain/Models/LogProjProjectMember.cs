using System;
using System.Collections.Generic;

namespace Reimbit.Domain.Models;

public partial class LogProjProjectMember
{
    public int LogprojectMemberId { get; set; }

    public string Iud { get; set; } = null!;

    public DateTime IuddateTime { get; set; }

    public int IudbyUserId { get; set; }

    public int? OrganizationId { get; set; }

    public int? UserId { get; set; }

    public DateTime? Created { get; set; }

    public DateTime? Modified { get; set; }
}
