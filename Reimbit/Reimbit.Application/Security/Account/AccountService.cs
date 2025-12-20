using ErrorOr;
using Reimbit.Contracts.Security.Account;
using Reimbit.Domain.Repositories;

namespace Reimbit.Application.Security.Account;

public class AccountService(
    IAccountRepository repository
) : IAccountService
{

    public async Task<ErrorOr<LoginResponse<LoginInfo>>> Login(LoginRequest request)
    {
        var result = await repository.Login(request);
        return result;
    }

    public async Task<ErrorOr<LoginResponse<LoginInfo>>> Register(RegisterRequest request)
    {
        var result = await repository.Register(request);
        return result;
    }
}
