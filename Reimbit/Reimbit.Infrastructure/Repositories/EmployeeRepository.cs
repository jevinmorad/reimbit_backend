using AegisInt.Core;
using Common.Data.Models;
using ErrorOr;
using Microsoft.EntityFrameworkCore;
using Reimbit.Contracts.Employee;
using Reimbit.Domain.Interfaces;
using Reimbit.Domain.Models;
using Reimbit.Domain.Repositories;

namespace Reimbit.Infrastructure.Repositories;

public class EmployeeRepository(
    IApplicationDbContext context    
) : IEmployeeRepository
{
    public async Task<ErrorOr<OperationResponse<EncryptedInt>>> Insert(InsertEmployeeRequest request)
    {
        var dbContext = (DbContext)context;

        await using var tx = await dbContext.Database.BeginTransactionAsync();

        try
        {
            var user = new SecUser
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                MobileNo = request.MobileNo,
                UserName = request.Email,
                IsActive = request.IsActive,
                Created = request.Created,
                Modified = request.Modified,
                CreatedByUserId = request.CreatedByUserId,
                ModifiedByUserId = request.ModifiedByUserId
            };

            var us = context.SecUsers.Add(user);

            var userRole = new SecUserRole
            {
                UserRoleId = request.RoleId,
                UserId = user.UserId,
                Created = request.Created,
                Modified = request.Modified
            };

            context.SecUserRoles.Add(userRole);

            var logUser = new LogSecUser
            {
                Iud = "I",
                IuddateTime = request.Created,
                IudbyUserId = request.CreatedByUserId,
                UserId = user.UserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                UserName = user.UserName,
                MobileNo = user.MobileNo,
                UserProfileImageUrl = user.UserProfileImageUrl,
                CreatedByUserId = user.CreatedByUserId,
                ModifiedByUserId = user.ModifiedByUserId,
                Created = user.Created,
                Modified = user.Modified
            };

            context.LogSecUsers.Add(logUser);

            var logUserRole = new LogSecUserRole
            {
                Iud = "I",
                IuddateTime = request.Created,
                IudbyUserId = request.CreatedByUserId,
                UserId = user.UserId,
                UserRoleId = userRole.UserRoleId,
                Created = userRole.Created,
                Modified = userRole.Modified,
                CreatedByUserId = userRole.CreatedByUserId,
                ModifiedByUserId = userRole.ModifiedByUserId
            };

            context.LogSecUserRoles.Add(logUserRole);

            var rowsAffected = await context.SaveChangesAsync(default);
            await tx.CommitAsync();

            return new OperationResponse<EncryptedInt>()
            {
                Id = user.UserId,
                RowsAffected = rowsAffected
            };
        }
        catch (Exception ex)
        {
            await tx.RollbackAsync();

            return Error.Failure("Employee.Insert.Failed", ex.Message);
        }
    }

    public async Task<ErrorOr<PagedResult<ListEmployeeResponse>>> List(int organizationId)
    {
        var query = context.SecUsers
            .Where(u => u.SecUserAuth!.OrganizationId == organizationId)
            .Select(u => new ListEmployeeResponse
            {
                UserId = u.UserId,
                DisplayName = u.FirstName + " " + u.LastName,
                Email = u.Email,
                MobileNo = u.MobileNo ?? "",
                Role = u.SecUserRoleUsers
                    .Join(context.SecRoles,
                        ur => ur.UserRoleId,
                        r => r.RoleId,
                        (ur, r) => r.RoleName)
                    .FirstOrDefault() ?? string.Empty,
                IsActive = u.IsActive
            });

        var data = await query.ToListAsync();
        
        return new PagedResult<ListEmployeeResponse>
        {
            Total = data.Count,
            Data = data
        };
    }
}