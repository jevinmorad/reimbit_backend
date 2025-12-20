using System;
using System.Collections.Generic;

namespace Reimbit.Domain.Models;

public partial class LogProjProject
{
    public int LogprojectsId { get; set; }

    public string Iud { get; set; } = null!;

    public DateTime IuddateTime { get; set; }

    public int IudbyUserId { get; set; }

    public int? ProjectId { get; set; }

    public string? ProjectName { get; set; }

    public string? ProjectLogoUrl { get; set; }

    public string? ProjectDetails { get; set; }

    public string? ProjectDescription { get; set; }

    public int? OrganizationId { get; set; }

    public int? ManagerId { get; set; }

    public int? CreatedByUserId { get; set; }

    public int? ModifiedByUserId { get; set; }

    public DateTime? Created { get; set; }

    public DateTime? Modified { get; set; }

    public bool? IsActive { get; set; }
}
