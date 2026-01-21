namespace Reimbit.Contracts.Account;

public class LoginResponse<TData>
{
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
    public TData? User { get; set; }
}
