using AegisInt.Core;
using Common.Data.Models;
using ErrorOr;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Reimbit.Application.Audit;
using Reimbit.Application.Jwt;
using Reimbit.Contracts.Account;
using Reimbit.Core.Common.Permissions;
using Reimbit.Domain.Interfaces;
using Reimbit.Domain.Models;
using Reimbit.Domain.Repositories;

namespace Reimbit.Infrastructure.Repositories;

public class AccountRepository(
    IApplicationDbContext context,
    IPasswordHasher<SecUser> passwordHasher,
    IJwtTokenService jwtTokenService,
    IAuditLogger auditLogger
) : IAccountRepository
{
    public async Task<ErrorOr<LoginResponse<LoginInfo>>> Login(LoginRequest request)
    {
        var user = await context.SecUsers
            .Include(u => u.SecUserAuth)
            .FirstOrDefaultAsync(u => u.Email == request.Email);

        if (user == null || user.SecUserAuth == null)
        {
            await auditLogger.WriteAsync(
                entityType: "SEC_UserAuth",
                entityId: null,
                action: "LOGIN_FAILED",
                organizationId: null,
                userId: null,
                oldValue: null,
                newValue: new { request.Email, Reason = "USER_NOT_FOUND" },
                ipAddress: null,
                userAgent: null);

            return Error.NotFound(description: "Invalid credentials");
        }

        var isPassCorrect = passwordHasher.VerifyHashedPassword(user, user.SecUserAuth.PasswordHash, request.Password);

        if (isPassCorrect == PasswordVerificationResult.Failed)
        {
            await auditLogger.WriteAsync(
                entityType: "SEC_UserAuth",
                entityId: user.UserId,
                action: "LOGIN_FAILED",
                organizationId: user.OrganizationId,
                userId: user.UserId,
                oldValue: null,
                newValue: new { user.UserId, user.Email, Reason = "INVALID_PASSWORD" },
                ipAddress: null,
                userAgent: null);

            return Error.Unauthorized(description: "Invalid credentials");
        }

        var userRole = await context.SecUserRoles
            .FirstOrDefaultAsync(ur => ur.UserId == user.UserId);

        var loginInfo = new LoginInfo
        {
            UserId = user.UserId,
            OrganizationId = user.OrganizationId,
            Email = user.Email,
            RoleId = userRole?.UserRoleId
        };

        var accessToken = jwtTokenService.GenerateAccessToken(loginInfo);
        var refreshToken = jwtTokenService.GenerateRefreshToken();

        user.SecUserAuth.RefreshToken = refreshToken;
        user.SecUserAuth.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
        user.SecUserAuth.IsRevoked = false;
        user.SecUserAuth.LastLogin = DateTime.UtcNow;

        await context.SaveChangesAsync();

        await auditLogger.WriteAsync(
            entityType: "SEC_UserAuth",
            entityId: user.UserId,
            action: "LOGIN",
            organizationId: user.OrganizationId,
            userId: user.UserId,
            oldValue: null,
            newValue: new
            {
                user.UserId,
                user.Email,
                user.OrganizationId,
                user.SecUserAuth.LastLogin,
                user.SecUserAuth.RefreshTokenExpiry
            },
            ipAddress: null,
            userAgent: null);

        await auditLogger.WriteAsync(
            entityType: "SEC_UserAuth",
            entityId: user.UserId,
            action: "REFRESH_TOKEN_ISSUED",
            organizationId: user.OrganizationId,
            userId: user.UserId,
            oldValue: null,
            newValue: new { user.UserId, user.SecUserAuth.RefreshTokenExpiry },
            ipAddress: null,
            userAgent: null);

        return new LoginResponse<LoginInfo>
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            User = loginInfo
        };
    }

    public async Task<ErrorOr<LoginResponse<LoginInfo>>> Refresh(RefreshTokenRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.RefreshToken))
        {
            return Error.Unauthorized(description: "Refresh token is missing.");
        }

        var auth = await context.SecUserAuths
            .Include(a => a.User)
            .FirstOrDefaultAsync(a => a.RefreshToken == request.RefreshToken);

        if (auth == null)
        {
            await auditLogger.WriteAsync(
                entityType: "SEC_UserAuth",
                entityId: null,
                action: "REFRESH_TOKEN_REJECTED",
                organizationId: null,
                userId: null,
                oldValue: null,
                newValue: new { Reason = "TOKEN_NOT_FOUND" },
                ipAddress: null,
                userAgent: null);

            return Error.Unauthorized(description: "Invalid refresh token.");
        }

        if (auth.IsRevoked || auth.RefreshTokenExpiry < request.CurrentDate)
        {
            await auditLogger.WriteAsync(
                entityType: "SEC_UserAuth",
                entityId: auth.UserId,
                action: "REFRESH_TOKEN_REJECTED",
                organizationId: auth.User.OrganizationId,
                userId: auth.UserId,
                oldValue: null,
                newValue: new { Reason = auth.IsRevoked ? "REVOKED" : "EXPIRED" },
                ipAddress: null,
                userAgent: null);

            return Error.Unauthorized(description: "Refresh token expired/revoked.");
        }

        var userRole = await context.SecUserRoles
            .AsNoTracking()
            .FirstOrDefaultAsync(ur => ur.UserId == auth.UserId);

        var loginInfo = new LoginInfo
        {
            UserId = auth.UserId,
            OrganizationId = auth.User.OrganizationId,
            Email = auth.User.Email,
            RoleId = userRole?.UserRoleId
        };

        var accessToken = jwtTokenService.GenerateAccessToken(loginInfo);

        var oldTokenSnapshot = new { auth.RefreshTokenExpiry };

        var newRefreshToken = jwtTokenService.GenerateRefreshToken();
        auth.RefreshToken = newRefreshToken;
        auth.RefreshTokenExpiry = request.CurrentDate.AddDays(7);
        auth.IsRevoked = false;

        await context.SaveChangesAsync();

        await auditLogger.WriteAsync(
            entityType: "SEC_UserAuth",
            entityId: auth.UserId,
            action: "REFRESH_TOKEN_REVOKED",
            organizationId: auth.User.OrganizationId,
            userId: auth.UserId,
            oldValue: oldTokenSnapshot,
            newValue: new { Reason = "ROTATION" },
            ipAddress: null,
            userAgent: null);

        await auditLogger.WriteAsync(
            entityType: "SEC_UserAuth",
            entityId: auth.UserId,
            action: "REFRESH_TOKEN_ISSUED",
            organizationId: auth.User.OrganizationId,
            userId: auth.UserId,
            oldValue: null,
            newValue: new { auth.RefreshTokenExpiry },
            ipAddress: null,
            userAgent: null);

        return new LoginResponse<LoginInfo>
        {
            AccessToken = accessToken,
            RefreshToken = newRefreshToken,
            User = loginInfo
        };
    }

    public async Task<ErrorOr<OperationResponse<EncryptedInt>>> Logout(LogoutRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.RefreshToken))
        {
            return Error.Unauthorized(description: "Refresh token is missing.");
        }

        var auth = await context.SecUserAuths
            .Include(a => a.User)
            .FirstOrDefaultAsync(a => a.RefreshToken == request.RefreshToken);

        if (auth == null)
        {
            return Error.NotFound(description: "Refresh token not found.");
        }

        auth.IsRevoked = true;
        auth.RefreshTokenExpiry = request.CurrentDate;

        var rows = await context.SaveChangesAsync();

        await auditLogger.WriteAsync(
            entityType: "SEC_UserAuth",
            entityId: auth.UserId,
            action: "LOGOUT",
            organizationId: auth.User.OrganizationId,
            userId: auth.UserId,
            oldValue: null,
            newValue: new { auth.UserId, Reason = "USER_LOGOUT" },
            ipAddress: null,
            userAgent: null);

        await auditLogger.WriteAsync(
            entityType: "SEC_UserAuth",
            entityId: auth.UserId,
            action: "REFRESH_TOKEN_REVOKED",
            organizationId: auth.User.OrganizationId,
            userId: auth.UserId,
            oldValue: null,
            newValue: new { Reason = "LOGOUT" },
            ipAddress: null,
            userAgent: null);

        return new OperationResponse<EncryptedInt> { Id = auth.UserId, RowsAffected = rows };
    }

    public async Task<ErrorOr<LoginResponse<LoginInfo>>> Register(RegisterRequest request)
    {
        var dbContext = (DbContext)context;
        await using var tx = await dbContext.Database.BeginTransactionAsync();

        try
        {
            var existingUser = await context.SecUsers
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == request.Email);

            if (existingUser != null)
            {
                return Error.Conflict(description: "User with this email already exists.");
            }

            var existingOrg = await context.OrgOrganizations
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.OrganizationName == request.OrganizationName);

            if (existingOrg != null)
            {
                return Error.Conflict(description: "Organization with this name already exists.");
            }

            var org = new OrgOrganization
            {
                OrganizationName = request.OrganizationName.Trim(),
                Created = request.Created
            };

            await context.OrgOrganizations.AddAsync(org);
            await context.SaveChangesAsync();

            var user = new SecUser
            {
                FirstName = request.FirstName.Trim(),
                LastName = request.LastName.Trim(),
                Email = request.Email.Trim(),
                DisplayName = request.DisplayName.Trim(),
                MobileNo = request.MobileNo.Trim(),
                ProfileImageUrl = request.ProfileImageUrl,
                OrganizationId = org.OrganizationId,
                IsActive = true,
                Created = request.Created,
            };

            await context.SecUsers.AddAsync(user);
            await context.SaveChangesAsync();

            var adminRole = new SecRole
            {
                RoleName = "Admin",
                Description = "Organization Admin Role with all permissions",
                OrganizationId = org.OrganizationId,
                CreatedByUserId = user.UserId,
                Created = request.Created,
                IsActive = true,
                ValidFrom = request.Created,
            };

            await context.SecRoles.AddAsync(adminRole);
            await context.SaveChangesAsync();

            var allPermissions = Enum.GetValues(typeof(Permission)).Cast<int>();
            foreach (var permValue in allPermissions)
            {
                var roleClaim = new SecRoleClaim
                {
                    RoleId = adminRole.RoleId,
                    ClaimValue = permValue,
                    CreatedByUserId = user.UserId,
                    Created = request.Created
                };
                await context.SecRoleClaims.AddAsync(roleClaim);
            }
            await context.SaveChangesAsync();

            var userRole = new SecUserRole
            {
                UserId = user.UserId,
                RoleId = adminRole.RoleId,
                CreatedByUserId = user.UserId,
                Created = request.Created,
            };

            await context.SecUserRoles.AddAsync(userRole);

            var refreshToken = jwtTokenService.GenerateRefreshToken();

            var auth = new SecUserAuth
            {
                UserId = user.UserId,
                PasswordHash = passwordHasher.HashPassword(user, request.Password),
                RefreshToken = refreshToken,
                RefreshTokenExpiry = request.Created.AddDays(7),
                IsRevoked = false,
                LastLogin = request.Created
            };

            await context.SecUserAuths.AddAsync(auth);

            await context.SaveChangesAsync();
            await tx.CommitAsync();

            await auditLogger.WriteAsync(
                entityType: "SEC_User",
                entityId: user.UserId,
                action: "REGISTER",
                organizationId: user.OrganizationId,
                userId: user.UserId,
                oldValue: null,
                newValue: new
                {
                    user.UserId,
                    user.Email,
                    user.OrganizationId,
                    adminRole.RoleId,
                    adminRole.RoleName
                },
                ipAddress: null,
                userAgent: null);

            var loginInfo = new LoginInfo
            {
                UserId = user.UserId,
                OrganizationId = org.OrganizationId,
                Email = user.Email,
                RoleId = adminRole.RoleId
            };

            var accessToken = jwtTokenService.GenerateAccessToken(loginInfo);

            return new LoginResponse<LoginInfo>
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                User = loginInfo
            };
        }
        catch (Exception ex)
        {
            await tx.RollbackAsync();

            await auditLogger.WriteAsync(
                entityType: "SEC_User",
                entityId: null,
                action: "REGISTER_FAILED",
                organizationId: null,
                userId: null,
                oldValue: null,
                newValue: new { request.Email, request.OrganizationName, ex.Message },
                ipAddress: null,
                userAgent: null);

            return Error.Failure("Account.Register.Failed", ex.Message);
        }
    }
}