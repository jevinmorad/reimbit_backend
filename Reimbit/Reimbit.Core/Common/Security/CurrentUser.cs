namespace Common.Security;

public class CurrentUser<TokenData>
{
    private readonly TokenData _userData;

    public TokenData UserData => _userData;
    public CurrentUser(TokenData userData) => _userData = userData;
}
