namespace Reimbit.Contracts.Security.Account;

public class LoginResponse<TData>
{
    public string? AccessToken { get; set; }
    public TData? User { get; set; }
}
