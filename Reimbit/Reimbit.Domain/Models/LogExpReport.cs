using System;
using System.Collections.Generic;

namespace Reimbit.Domain.Models;

public partial class LogExpReport
{
    public int LogorganizationId { get; set; }

    public string Iud { get; set; } = null!;

    public DateTime IuddateTime { get; set; }

    public int IudbyUserId { get; set; }

    public int? ReportId { get; set; }

    public int? OrganizationId { get; set; }

    public int? ManagerId { get; set; }

    public int? ProjectId { get; set; }

    public string? Title { get; set; }

    public string? ReportStatus { get; set; }

    public decimal? AcceptedAmount { get; set; }

    public decimal? RejectedAmount { get; set; }

    public DateTime? ViewedAt { get; set; }

    public int? CreatedByUserId { get; set; }

    public int? ModifiedByUserId { get; set; }

    public DateTime? Created { get; set; }

    public DateTime? Modified { get; set; }
}
