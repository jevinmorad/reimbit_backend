using ErrorOr;
using Reimbit.Contracts.User;
using AegisInt.Core;
using Common.Data.Models;

namespace Reimbit.Application.UserManagement;

public interface IUserService
{
    Task<ErrorOr<UserResponse>> GetProfile();
    Task<ErrorOr<OperationResponse<EncryptedInt>>> UpdateProfile(UpdateProfileRequest request);
    Task<ErrorOr<PermissionsResponse>> GetPermissions();
    Task<ErrorOr<InfoResponse>> GetInfo();
}