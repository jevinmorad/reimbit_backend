namespace Reimbit.Contracts.Project;

public class ViewProjectResponse
{
    public required string ProjectName { get; set; }
    public required string ManagerDisplayName { get; set; }
    public required string CreatedByUserName { get; set; }
    public required bool IsActive { get; set; }
    public DateTime Created { get; set; }
    public DateTime? Modified { get; set; }
    public decimal TotalExpense { get; set; }
    public decimal AcceptedAmount { get; set; }
    public decimal RejectedAmount { get; set; }
    public decimal  PendingToViewAmount { get; set; }
    public required List<string> Employees { get; set; }
}
