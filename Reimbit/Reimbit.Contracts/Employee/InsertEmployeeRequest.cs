using AegisInt.Core;

namespace Reimbit.Contracts.Employee;

public class InsertEmployeeRequest
{
    public EncryptedInt? UserId { get; set; }
    public required int OrganizationId { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string MobileNo { get; set; }
    public required int RoleId { get; set; }
    public int CreatedByUserId { get; set; }
    public int ModifiedByUserId { get; set; }
    public DateTime Created { get; set; }
    public DateTime Modified { get; set; }
    public bool IsActive { get; set; }
}