using ErrorOr;
using Reimbit.Contracts.Security.Account;

namespace Reimbit.Domain.Repositories;

public interface IAccountRepository
{
    Task<ErrorOr<LoginResponse<LoginInfo>>> Login(LoginRequest request);
    Task<ErrorOr<LoginResponse<LoginInfo>>> Register(RegisterRequest request);

}