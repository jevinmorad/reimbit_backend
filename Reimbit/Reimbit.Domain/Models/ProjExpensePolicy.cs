using System;
using System.Collections.Generic;

namespace Reimbit.Domain.Models;

public partial class ProjExpensePolicy
{
    public int PolicyId { get; set; }

    public int ProjectId { get; set; }

    public int? CategoryId { get; set; }

    public decimal? MaxAmount { get; set; }

    public bool IsReceiptMandatory { get; set; }

    public string? Description { get; set; }

    public DateTime? Created { get; set; }

    public virtual ExpCategory? Category { get; set; }

    public virtual ProjProject Project { get; set; } = null!;
}
