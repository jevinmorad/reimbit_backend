using ErrorOr;
using Reimbit.Contracts.User;
using AegisInt.Core;
using Common.Data.Models;

namespace Reimbit.Domain.Repositories;

public interface IUserRepository
{
    Task<ErrorOr<UserResponse>> GetProfile(int userId);
    Task<ErrorOr<OperationResponse<EncryptedInt>>> UpdateProfile(UpdateProfileRequest request);
    Task<ErrorOr<PermissionsResponse>> GetPermissions(int userId);
    Task<ErrorOr<InfoResponse>> GetInfo(int userId);
}