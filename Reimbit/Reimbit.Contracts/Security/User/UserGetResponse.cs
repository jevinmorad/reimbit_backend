using AegisInt.Core;

namespace Reimbit.Contracts.Security.User;

public class UserGetResponse
{
    public EncryptedInt? UserID { get; set; }
    public string DisplayName { get; set; }
    public  EncryptedInt RoleID { get; set; }
    public int CreatedByUserID { get; set; }
    public int ModifiedByUserID { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public int MobileNo { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}
