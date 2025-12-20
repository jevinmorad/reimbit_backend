using ErrorOr;
using Reimbit.Contracts.Security.Account;

namespace Reimbit.Application.Security.Account;

public interface IAccountService
{
    Task<ErrorOr<LoginResponse<LoginInfo>>> Login(LoginRequest request);
    Task<ErrorOr<LoginResponse<LoginInfo>>> Register(RegisterRequest request);

}
