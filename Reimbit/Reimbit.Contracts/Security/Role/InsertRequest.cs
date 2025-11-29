namespace Reimbit.Contracts.Security.Role;

public class InsertRequest
{
    public int RoleID { get; set; }
    public string RoleName { get; set; }
    public string? Description { get; set; }
    public int OrganizationID { get; set; }
    public int CreatedByUserID { get; set; }
}
