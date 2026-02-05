using AegisInt.Core;
using Common.Data.Models;
using Common.Security;
using ErrorOr;
using Reimbit.Contracts.User;
using Reimbit.Core.Models;
using Reimbit.Domain.Repositories;

namespace Reimbit.Application.UserManagement;

public class UserService(
    ICurrentUserProvider currentUserProvider,
    IUserRepository repository
) : IUserService
{
    private readonly CurrentUser<TokenData> currentUser = currentUserProvider.GetCurrentUser<TokenData>();

    public async Task<ErrorOr<PermissionsResponse>> GetPermissions()
    {
        var userId = currentUser.UserId;
        return await repository.GetPermissions(userId);
    }

    public async Task<ErrorOr<InfoResponse>> GetInfo()
    {
        var userId = currentUser.UserId;
        return await repository.GetInfo(userId);
    }

    public async Task<ErrorOr<UserResponse>> GetProfile()
    {
        return await repository.GetProfile(currentUser.UserId);
    }

    public async Task<ErrorOr<OperationResponse<EncryptedInt>>> UpdateProfile(UpdateProfileRequest request)
    {
        request.UserId = currentUser.UserId;
        request.ModifiedByUserId = currentUser.UserId;
        request.Modified = DateTime.UtcNow;

        return await repository.UpdateProfile(request);
    }
}