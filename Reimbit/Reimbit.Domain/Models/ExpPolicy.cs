namespace Reimbit.Domain.Models;

public partial class ExpPolicy
{
    public int PolicyId { get; set; }

    public int? CategoryId { get; set; }

    public decimal? MaxAmount { get; set; }

    public bool IsReceiptMandatory { get; set; }

    public string? Description { get; set; }

    public int CreatedByUserId { get; set; }

    public int? ModifiedByUserId { get; set; }

    public DateTime Created { get; set; }

    public DateTime? Modified { get; set; }

    public virtual ExpCategory? Category { get; set; }

    public virtual SecUser CreatedByUser { get; set; } = null!;

    public virtual SecUser ModifiedByUser { get; set; } = null!;
}
