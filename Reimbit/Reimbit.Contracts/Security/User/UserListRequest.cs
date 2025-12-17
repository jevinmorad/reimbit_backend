using Common.Web.Models;

namespace Reimbit.Contracts.Security.User;

public class UserListRequest : PagedRequest
{
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public bool? IsActive { get; set; }
}