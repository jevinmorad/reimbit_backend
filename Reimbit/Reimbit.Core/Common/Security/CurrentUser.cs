namespace Common.Security;

public class CurrentUser<TokenData>
{
    private int _userId;
    private int _organizationId;

    private TokenData _userData;

    public int UserId => _userId;
    public int OrganizationId => _organizationId;

    public TokenData UserData => _userData;

    public CurrentUser(int userId, int organizationId, TokenData userData)
    {
        _userId = userId;
        _organizationId = organizationId;
        _userData = userData;
    }
}
