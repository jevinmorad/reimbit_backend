namespace Reimbit.Contracts.Security.Role;

public class UpdateRequest
{
    public int RoleID { get; set; }
    public string RoleName { get; set; }
    public string RoleShortName { get; set; }
    public string Description { get; set; }
    public int? ModifiedByUserID { get; set; }
}