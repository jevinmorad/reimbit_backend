using Azure.Core;
using ErrorOr;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Reimbit.Application.Jwt;
using Reimbit.Contracts.Security.Account;
using Reimbit.Domain.Interfaces;
using Reimbit.Domain.Models;
using Reimbit.Domain.Repositories;

namespace Reimbit.Infrastructure.Repositories;

public class AccountRepository(
    IApplicationDbContext context,
    IPasswordHasher<SecUser> passwordHasher,
    IJwtTokenService jwtTokenService
) : IAccountRepository
{
    public async Task<ErrorOr<LoginResponse<LoginInfo>>> Login(LoginRequest request)
    {
        var user = await context.SecUsers
            .Include(u => u.SecUserAuth)
            .FirstOrDefaultAsync(u => u.Email == request.Email);

        if (user == null || user.SecUserAuth == null)
        {
            return Error.NotFound(description: "Invalid credentials");
        }

        var isPassCorrect = passwordHasher.VerifyHashedPassword(
            user,
            user.SecUserAuth.PasswordHash,
            request.Password);

        if (isPassCorrect == PasswordVerificationResult.Failed)
        {
            return Error.Unauthorized(description: "Invalid credentials");
        }

        var userRole = await context.SecUserRoles
            .FirstOrDefaultAsync(ur => ur.UserId == user.UserId);

        var loginInfo = new LoginInfo
        {
            UserId = user.UserId,
            OrganizationId = user.SecUserAuth.OrganizationId,
            Email = user.Email,
            RoleId = userRole?.UserRoleId
        };

        var accessToken = jwtTokenService.GenerateAccessToken(loginInfo);
        var refreshToken = jwtTokenService.GenerateRefreshToken();

        user.SecUserAuth.RefreshToken = refreshToken;
        user.SecUserAuth.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
        user.SecUserAuth.LastLogin = DateTime.UtcNow;

        var logAuth = new LogSecUserAuth
        {
            Iud = "U",
            IudbyUserId = user.UserId,
            IuddateTime = DateTime.UtcNow,
            UserId = user.SecUserAuth.UserId,
            OrganizationId = user.SecUserAuth.OrganizationId,
            PasswordHash = user.SecUserAuth.PasswordHash,
            RefreshToken = user.SecUserAuth.RefreshToken,
            RefreshTokenExpiry = user.SecUserAuth.RefreshTokenExpiry,
            LastLogin = user.SecUserAuth.LastLogin
        };

        context.LogSecUserAuths.Add(logAuth);

        await context.SaveChangesAsync(default);

        return new LoginResponse<LoginInfo>
        {
            AccessToken = accessToken,
            User = loginInfo
        };
    }

    public async Task<ErrorOr<LoginResponse<LoginInfo>>> Register(RegisterRequest request)
    {
        var dbContext = (DbContext)context;

        await using var tx = await dbContext.Database.BeginTransactionAsync();

        var existingUser = await context.SecUsers
            .FirstOrDefaultAsync(u => u.Email == request.Email);

        if (existingUser != null)
        {
            return Error.Conflict(description: "User with this email already exists.");
        }

        var existingOrg = await context.OrgOrganizations
            .FirstOrDefaultAsync(o => o.OrganizationName == request.OrganizationName);

        if (existingOrg != null)
        {
            return Error.Conflict(description: "Organization with this name already exists.");
        }

        var user = new SecUser
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            UserName = request.Email,
            MobileNo = request.MobileNo,
            UserProfileImageUrl = request.UserProfileImageUrl,
            IsActive = true,
            CreatedByUserId = null,
            ModifiedByUserId = null,
            Created = DateTime.UtcNow,
            Modified = DateTime.UtcNow
        };

        await context.SecUsers.AddAsync(user);
        await context.SaveChangesAsync(default);

        user.CreatedByUserId = user.UserId;
        user.ModifiedByUserId = user.UserId;

        var org = new OrgOrganization
        {
            OrganizationName = request.OrganizationName,
            CreatedByUserId = user.UserId,
            ModifiedByUserId = user.UserId,
            Created = DateTime.UtcNow,
            Modified = DateTime.UtcNow
        };

        await context.OrgOrganizations.AddAsync(org);
        await context.SaveChangesAsync(default);

        var auth = new SecUserAuth
        {
            UserId = user.UserId,
            OrganizationId = org.OrganizationId,
            PasswordHash = passwordHasher.HashPassword(user, request.Password),
            RefreshToken = null,
            RefreshTokenExpiry = DateTime.UtcNow.AddDays(7),
            LastLogin = DateTime.UtcNow
        };

        await context.SecUserAuths.AddAsync(auth);

        var logUser = new LogSecUser
        {
            Iud = "I",
            IuddateTime = DateTime.UtcNow,
            IudbyUserId = user.UserId,
            UserId = user.UserId,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            UserName = user.UserName,
            MobileNo = user.MobileNo,
            UserProfileImageUrl = user.UserProfileImageUrl,
            IsActive = user.IsActive,
            CreatedByUserId = user.CreatedByUserId,
            ModifiedByUserId = user.ModifiedByUserId,
            Created = user.Created,
            Modified = user.Modified
        };

        context.LogSecUsers.Add(logUser);

        var logOrg = new LogOrgOrganization
        {
            Iud = "I",
            IuddateTime = DateTime.UtcNow,
            IudbyUserId = user.UserId,
            OrganizationId = org.OrganizationId,
            OrganizationName = org.OrganizationName,
            Created = org.Created,
            Modified = org.Modified
        };

        context.LogOrgOrganizations.Add(logOrg);

        await context.SaveChangesAsync(default);

        var loginInfo = new LoginInfo
        {
            UserId = user.UserId,
            OrganizationId = org.OrganizationId,
            Email = user.Email,
            RoleId = null
        };

        var accessToken = jwtTokenService.GenerateAccessToken(loginInfo);
        var refreshToken = jwtTokenService.GenerateRefreshToken();

        auth.RefreshToken = refreshToken;
        auth.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
        auth.LastLogin = DateTime.UtcNow;

        var logAuth = new LogSecUserAuth
        {
            Iud = "I",
            IudbyUserId = user.UserId,
            IuddateTime = DateTime.UtcNow,
            UserId = auth.UserId,
            OrganizationId = auth.OrganizationId,
            PasswordHash = auth.PasswordHash,
            RefreshToken = auth.RefreshToken,
            RefreshTokenExpiry = auth.RefreshTokenExpiry,
            LastLogin = auth.LastLogin
        };

        context.LogSecUserAuths.Add(logAuth);

        await context.SaveChangesAsync(default);
        await tx.CommitAsync();

        var response = new LoginResponse<LoginInfo>
        {
            AccessToken = accessToken,
            User = loginInfo
        };

        return response;
    }
}