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
        var response = new OperationResponse<EncryptedInt>();

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
                IsActive = true,
                Created = request.Created,
                CreatedByUserId = request.CreatedByUserId,
                ModifiedByUserId = request.ModifiedByUserId
            };

            await context.SecUsers.AddAsync(user);
            //await context.SaveChangesAsync(default);

            var userRole = new SecUserRole
            {
                UserRoleId = request.RoleId,
                UserId = user.UserId,
                Created = request.Created,
                Modified = request.Modified
            };

            await context.SecUserRoles.AddAsync(userRole);
            //await context.SaveChangesAsync(default);

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

            await context.LogSecUsers.AddAsync(logUser);

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

            await context.LogSecUserRoles.AddAsync(logUserRole);

            await context.SaveChangesAsync(default);
            await tx.CommitAsync();

            response.Id = user.UserId;

            return response;
        }
        catch (Exception ex)
        {
            await tx.RollbackAsync();

            return Error.Failure("Employee.Insert.Failed", ex.Message);
        }
    }

    public async Task<ErrorOr<PagedResult<ListEmployeeResponse>>> List(int organizationId)
    {
        //var query =
        //    from u in context.SecUsers
        //    join ua in context.SecUserAuths on u.UserId equals ua.UserId
        //    join ur in context.SecUserRoles on u.UserId equals ur.UserId
        //    join r in context.SecRoles on ur.UserRoleId equals r.RoleId
        //    where ua.OrganizationId == OrganizationId
        //    select new ListResponse
        //    {
        //        UserId = u.UserId,
        //        DisplayName = u.FirstName + " " + u.LastName,
        //        Email = u.Email,
        //        MobileNo = u.MobileNo ?? "",
        //        Role = r.RoleName,
        //        IsActive = u.IsActive
        //    };

        var query =
            from u in context.SecUsers
            join ua in context.SecUserAuths on u.UserId equals ua.UserId
            join ur in context.SecUserRoles on u.UserId equals ur.UserId into urGroup
            from ur in urGroup.DefaultIfEmpty()
            join r in context.SecRoles on ur.UserRoleId equals r.RoleId into rGroup
            from r in rGroup.DefaultIfEmpty()
            where ua.OrganizationId == organizationId
            select new ListEmployeeResponse
            {
                UserId = u.UserId,
                DisplayName = u.FirstName + " " + u.LastName,
                Email = u.Email,
                MobileNo = u.MobileNo ?? "",
                Role = r != null ? r.RoleName : string.Empty,
                IsActive = u.IsActive
            };

        var data = await query.ToListAsync();
        
        return new PagedResult<ListEmployeeResponse>
        {
            Total = data.Count,
            Data = data
        };
    }
}
