using System;
using System.Collections.Generic;

namespace Reimbit.Domain.Models;

public partial class LogExpExpense
{
    public int LogexpenseId { get; set; }

    public string Iud { get; set; } = null!;

    public DateTime IuddateTime { get; set; }

    public int IudbyUserId { get; set; }

    public int? ExpenseId { get; set; }

    public int? OrganizationId { get; set; }

    public int? UserId { get; set; }

    public int? CategoryId { get; set; }

    public string? Title { get; set; }

    public decimal? Amount { get; set; }

    public string? Currency { get; set; }

    public string? AttachmentUrl { get; set; }

    public int? ProjectId { get; set; }

    public string? Description { get; set; }

    public string? RejectionReason { get; set; }

    public string? ExpenseStatus { get; set; }

    public int? CreatedByUserId { get; set; }

    public int? ModifiedByUserId { get; set; }

    public DateTime? Created { get; set; }

    public DateTime? Modified { get; set; }
}
