using AegisInt.Core;
using Common.Data.Models;
using ErrorOr;
using Microsoft.EntityFrameworkCore;
using Reimbit.Application.Audit;
using Reimbit.Contracts.Approvals.Delegation;
using Reimbit.Domain.Interfaces;
using Reimbit.Domain.Models;
using Reimbit.Domain.Repositories;

namespace Reimbit.Infrastructure.Repositories;

public sealed class DelegationRepository(
    IApplicationDbContext context,
    IAuditLogger auditLogger
) : IDelegationRepository
{
    public async Task<int?> ResolveDelegateApproverUserId(int organizationId, int originalApproverUserId, DateTime atUtc)
    {
        // Rule #7: delegation cannot be re-delegated => if approver is acting as delegate for someone, do NOT allow them to delegate further.
        // This method resolves ONLY a direct delegation for the original approver.
        var now = atUtc;

        var active = await context.SecDelegateApprovers
            .AsNoTracking()
            .Where(d =>
                d.UserId == originalApproverUserId &&
                d.ValidFrom <= now &&
                d.ValidTo >= now &&
                d.User.OrganizationId == organizationId &&
                d.DelegateUser.OrganizationId == organizationId)
            .OrderByDescending(d => d.ValidFrom)
            .FirstOrDefaultAsync();

        return active?.DelegateUserId;
    }

    public async Task<ErrorOr<PagedResult<DelegationResponse>>> MyDelegations(int organizationId, int userId)
    {
        var now = DateTime.UtcNow;

        var data = await context.SecDelegateApprovers
            .AsNoTracking()
            .Include(x => x.DelegateUser)
            .Where(x => x.UserId == userId && x.User.OrganizationId == organizationId)
            .OrderByDescending(x => x.ValidFrom)
            .Select(x => new DelegationResponse
            {
                DelegateId = x.DelegateId,
                UserId = x.UserId,
                DelegateUserId = x.DelegateUserId,
                DelegateDisplayName = x.DelegateUser.DisplayName,
                ValidFrom = x.ValidFrom,
                ValidTo = x.ValidTo,
                IsActive = x.ValidFrom <= now && x.ValidTo >= now
            })
            .ToListAsync();

        return new PagedResult<DelegationResponse> { Total = data.Count, Data = data };
    }

    public async Task<ErrorOr<OperationResponse<EncryptedInt>>> Create(CreateDelegationRequest request)
    {
        var dbContext = (DbContext)context;
        await using var tx = await dbContext.Database.BeginTransactionAsync();

        try
        {
            var now = DateTime.UtcNow;
            var delegateUserId = (int)request.DelegateUserId;

            // Rule #1: cannot delegate to self
            if (delegateUserId == request.UserId)
            {
                return Error.Validation("Delegation.Self", "User cannot delegate to self.");
            }

            // Rule #5: must have end date
            if (request.ValidTo == default)
            {
                return Error.Validation("Delegation.ValidTo.Required", "Delegation must have an end date.");
            }

            // User must exist and be active, same org
            var user = await context.SecUsers.AsNoTracking()
                .FirstOrDefaultAsync(u => u.UserId == request.UserId && u.OrganizationId == request.OrganizationId);

            if (user == null)
            {
                return Error.NotFound("User.NotFound", "User not found.");
            }

            var delegateUser = await context.SecUsers.AsNoTracking()
                .FirstOrDefaultAsync(u => u.UserId == delegateUserId && u.OrganizationId == request.OrganizationId);

            // Rule #2 + #3
            if (delegateUser == null)
            {
                return Error.Validation("Delegation.Delegate.NotInOrg", "Delegate must belong to same organization.");
            }

            if (!delegateUser.IsActive)
            {
                return Error.Validation("Delegation.Delegate.Inactive", "Delegate must be active.");
            }

            // Rule #7: cannot be re-delegated
            var isUserCurrentlyADelegate = await context.SecDelegateApprovers
                .AsNoTracking()
                .AnyAsync(d => d.DelegateUserId == request.UserId && d.ValidFrom <= now && d.ValidTo >= now);

            if (isUserCurrentlyADelegate)
            {
                return Error.Validation("Delegation.ReDelegation.NotAllowed", "Delegation cannot be re-delegated.");
            }

            // Rule #4: delegate must have approval permission
            // Implemented as: delegate must be an "approver-eligible" user in current org (manager any approver role used in rules).
            var delegateIsApproverEligible = await IsApproverEligibleAsync(request.OrganizationId, delegateUserId);
            if (!delegateIsApproverEligible)
            {
                return Error.Validation("Delegation.Delegate.NotApprover", "Delegate must have approval permission.");
            }

            // Rule #1 also implies: delegator must be an actual approver (Who CAN delegate)
            var delegatorIsApproverEligible = await IsApproverEligibleAsync(request.OrganizationId, request.UserId);
            if (!delegatorIsApproverEligible)
            {
                return Error.Validation("Delegation.User.NotApprover", "Only approvers can delegate.");
            }

            // Rule #6: periods cannot overlap for same delegator
            var overlaps = await context.SecDelegateApprovers
                .AsNoTracking()
                .AnyAsync(d =>
                    d.UserId == request.UserId &&
                    d.ValidFrom <= request.ValidTo &&
                    d.ValidTo >= request.ValidFrom);

            if (overlaps)
            {
                return Error.Validation("Delegation.Overlap", "Delegation periods cannot overlap.");
            }

            // Rule #3 & #8: no admin-driven creation here, enforced by service/controller using current user.
            var entity = new SecDelegateApprover
            {
                UserId = request.UserId,
                DelegateUserId = delegateUserId,
                ValidFrom = request.ValidFrom,
                ValidTo = request.ValidTo
            };

            await context.SecDelegateApprovers.AddAsync(entity);
            var rows = await context.SaveChangesAsync();

            await auditLogger.WriteAsync(
                entityType: "SEC_DelegateApprover",
                entityId: entity.DelegateId,
                action: "DELEGATE_APPROVER_ADDED",
                organizationId: request.OrganizationId,
                userId: request.UserId,
                oldValue: null,
                newValue: new
                {
                    entity.DelegateId,
                    entity.UserId,
                    entity.DelegateUserId,
                    entity.ValidFrom,
                    entity.ValidTo
                },
                ipAddress: null,
                userAgent: null);

            await tx.CommitAsync();

            return new OperationResponse<EncryptedInt> { Id = entity.DelegateId, RowsAffected = rows };
        }
        catch (Exception ex)
        {
            await tx.RollbackAsync();
            return Error.Failure("Delegation.Create.Failed", ex.Message);
        }
    }

    public async Task<ErrorOr<OperationResponse<EncryptedInt>>> Revoke(RevokeDelegationRequest request, string reason)
    {
        var dbContext = (DbContext)context;
        await using var tx = await dbContext.Database.BeginTransactionAsync();

        try
        {
            if (string.IsNullOrWhiteSpace(reason))
            {
                return Error.Validation("Delegation.Revoke.ReasonRequired", "Revoke reason is required.");
            }

            var id = (int)request.DelegateId;

            var entity = await context.SecDelegateApprovers
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.DelegateId == id);

            if (entity == null)
            {
                return Error.NotFound("Delegation.NotFound", "Delegation not found.");
            }

            // Normal flow: user can revoke only their own delegation.
            // Emergency flow: service can set IsForce = true (admin endpoint), still audited.
            if (!request.IsForce && entity.UserId != request.UserId)
            {
                return Error.Unauthorized("Delegation.AccessDenied", "Cannot revoke another user's delegation.");
            }

            if (entity.User.OrganizationId != request.OrganizationId)
            {
                return Error.Unauthorized("Delegation.InvalidOrg", "Invalid organization scope.");
            }

            var old = new { entity.DelegateId, entity.UserId, entity.DelegateUserId, entity.ValidFrom, entity.ValidTo };

            // Auto-expire style: set ValidTo to now
            entity.ValidTo = DateTime.UtcNow;

            var rows = await context.SaveChangesAsync();

            await auditLogger.WriteAsync(
                entityType: "SEC_DelegateApprover",
                entityId: entity.DelegateId,
                action: request.IsForce ? "DELEGATE_APPROVER_FORCE_REVOKED" : "DELEGATE_APPROVER_REMOVED",
                organizationId: request.OrganizationId,
                userId: request.UserId,
                oldValue: old,
                newValue: new { entity.DelegateId, entity.ValidTo, Reason = reason },
                ipAddress: null,
                userAgent: null);

            await tx.CommitAsync();

            return new OperationResponse<EncryptedInt> { Id = entity.DelegateId, RowsAffected = rows };
        }
        catch (Exception ex)
        {
            await tx.RollbackAsync();
            return Error.Failure("Delegation.Revoke.Failed", ex.Message);
        }
    }

    private async Task<bool> IsApproverEligibleAsync(int organizationId, int userId)
    {
        // Approver eligibility based on existing system primitives:
        // 1) Is a manager for someone (Reporting Manager)
        var now = DateTime.UtcNow;

        var isReportingManager = await context.SecUserManagers
            .AsNoTracking()
            .AnyAsync(m =>
                m.ManagerId == userId &&
                m.ManagerType == 1 &&
                (m.ValidTo == null || m.ValidTo >= now) &&
                m.User.OrganizationId == organizationId);

        if (isReportingManager)
        {
            return true;
        }


        // 3) Is in any role referenced by approval levels (role-based / finance roles)
        var approverRoleIds = await context.AprApprovalLevels
            .AsNoTracking()
            .Where(l =>
                l.ApproverType == 3 || l.ApproverType == 5 || l.ApproverType == 6)
            .Select(l => l.ApproverRoleId)
            .Where(x => x != null)
            .Select(x => x!.Value)
            .Distinct()
            .ToListAsync();

        if (approverRoleIds.Count > 0)
        {
            var isInApproverRole = await context.SecUserRoles
                .AsNoTracking()
                .AnyAsync(ur =>
                    ur.UserId == userId &&
                    approverRoleIds.Contains(ur.RoleId) &&
                    ur.Role.OrganizationId == organizationId &&
                    ur.Role.ValidFrom <= now &&
                    (ur.Role.ValidTo == null || ur.Role.ValidTo >= now));

            if (isInApproverRole)
            {
                return true;
            }
        }

        // 4) Is explicitly configured as specific user approver in any level
        var isSpecificApprover = await context.AprApprovalLevels
            .AsNoTracking()
            .AnyAsync(l => l.ApproverType == 4 && l.SpecificUserId == userId);

        return isSpecificApprover;
    }
}