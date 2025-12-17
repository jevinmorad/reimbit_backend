namespace Reimbit.Contracts.Security.User;

public class LoginResponse<TData>
{
    public string? AccessToken { get; set; }
    public TData? User { get; set; }
}
