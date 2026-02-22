namespace Reimbit.Contracts.Employee;

public class EmployeeSelecttViewResponse
{
    public required string Name { get; set; }
    public string? Role { get; set; }
    public required decimal TotalExpense { get; set; }
    public required  DateTime Created { get; set; }
    public string? CreatedByUserName { get; set; }
    public DateTime? Modified { get; set; }
    public string? ModifiededByUserName { get; set; }
}
