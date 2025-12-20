using System;
using System.Collections.Generic;

namespace Reimbit.Domain.Models;

public partial class LogComExpenseQuery
{
    public int LogexpenseId { get; set; }

    public string Iud { get; set; } = null!;

    public DateTime IuddateTime { get; set; }

    public int IudbyUserId { get; set; }

    public int? QueryId { get; set; }

    public int? ExpenseId { get; set; }

    public int? SenderUserId { get; set; }

    public string? Message { get; set; }

    public DateTime? SentAt { get; set; }

    public bool? IsRead { get; set; }
}
