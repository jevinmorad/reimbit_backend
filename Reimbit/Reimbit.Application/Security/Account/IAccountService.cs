using AegisInt.Core;
using Common.Data.Models;
using ErrorOr;
using Reimbit.Contracts.Account;

namespace Reimbit.Application.Security.Account;

public interface IAccountService
{
    Task<ErrorOr<LoginResponse<LoginInfo>>> Login(LoginRequest request);
    Task<ErrorOr<LoginResponse<LoginInfo>>> Register(RegisterRequest request);

    Task<ErrorOr<LoginResponse<LoginInfo>>> Refresh(RefreshTokenRequest request);
    Task<ErrorOr<OperationResponse<EncryptedInt>>> Logout(LogoutRequest request);
}
