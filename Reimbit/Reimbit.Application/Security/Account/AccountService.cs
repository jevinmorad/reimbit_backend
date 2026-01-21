using AegisInt.Core;
using Common.Data.Models;
using ErrorOr;
using Reimbit.Contracts.Account;
using Reimbit.Domain.Repositories;

namespace Reimbit.Application.Security.Account;

public class AccountService(
    IAccountRepository repository
) : IAccountService
{
    public Task<ErrorOr<LoginResponse<LoginInfo>>> Login(LoginRequest request) => repository.Login(request);
    public Task<ErrorOr<LoginResponse<LoginInfo>>> Register(RegisterRequest request) => repository.Register(request);
    public Task<ErrorOr<LoginResponse<LoginInfo>>> Refresh(RefreshTokenRequest request)
    {
        request.CurrentDate = DateTime.UtcNow;
        return repository.Refresh(request);
    }
    public Task<ErrorOr<OperationResponse<EncryptedInt>>> Logout(LogoutRequest request)
    {
        request.CurrentDate = DateTime.UtcNow;
        return repository.Logout(request);
    }
}
