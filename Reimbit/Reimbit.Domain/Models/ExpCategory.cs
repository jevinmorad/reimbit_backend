using System;
using System.Collections.Generic;

namespace Reimbit.Domain.Models;

public partial class ExpCategory
{
    public int CategoryId { get; set; }

    public int OrganizationId { get; set; }

    public string CategoryName { get; set; } = null!;

    public int ProjectId { get; set; }

    public string? Description { get; set; }

    public int CreatedByUserId { get; set; }

    public int? ModifiedByUserId { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public virtual SecUser CreatedByUser { get; set; } = null!;

    public virtual ICollection<ExpExpense> ExpExpenses { get; set; } = new List<ExpExpense>();

    public virtual SecUser? ModifiedByUser { get; set; }

    public virtual OrgOrganization Organization { get; set; } = null!;

    public virtual ICollection<ProjExpensePolicy> ProjExpensePolicies { get; set; } = new List<ProjExpensePolicy>();

    public virtual ProjProject Project { get; set; } = null!;
}
