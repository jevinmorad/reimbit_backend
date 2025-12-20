using System;
using System.Collections.Generic;

namespace Reimbit.Domain.Models;

public partial class LogExpCategory
{
    public int LogcategoryId { get; set; }

    public string Iud { get; set; } = null!;

    public DateTime IuddateTime { get; set; }

    public int IudbyUserId { get; set; }

    public int? CategoryId { get; set; }

    public int? OrganizationId { get; set; }

    public string? CategoryName { get; set; }

    public int? ProjectId { get; set; }

    public string? Description { get; set; }

    public int? CreatedByUserId { get; set; }

    public int? ModifiedByUserId { get; set; }

    public DateTime? Created { get; set; }

    public DateTime? Modified { get; set; }
}
