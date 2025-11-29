namespace Reimbit.Contracts.Security.Role;

public class ViewResponse
{
    public int RoleID { get; set; }
    public string RoleName { get; set; }
    public string Description { get; set; }
    public string CreatedByUserName { get; set; }
    public string ModifiedByUserName { get; set; }
    public DateTime? Created { get; set; }
    public DateTime? Modified { get; set; }
}