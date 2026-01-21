using AegisInt.Core;
using Common.Data.Models;
using ErrorOr;
using Microsoft.EntityFrameworkCore;
using Reimbit.Contracts.Policies;
using Reimbit.Domain.Interfaces;
using Reimbit.Domain.Models;
using Reimbit.Domain.Repositories;

namespace Reimbit.Infrastructure.Repositories;

public class PoliciesRepository(IApplicationDbContext context) : IPoliciesRepository
{
    public async Task<ErrorOr<OperationResponse<EncryptedInt>>> Insert(InsertPolicyRequest request)
    {
        try
        {
            var policy = new ExpPolicy
            {
                CategoryId = request.CategoryId,
                MaxAmount = request.MaxAmount,
                IsReceiptMandatory = request.IsReceiptMandatory,
                Description = request.Description,
                CreatedByUserId = request.CreatedByUserId,
                ModifiedByUserId = request.CreatedByUserId,
                Created = request.Created,
                Modified = request.Created
            };

            await context.ExpPolicies.AddAsync(policy);

            var rowsAffected = await context.SaveChangesAsync(default);

            return new OperationResponse<EncryptedInt>
            {
                Id = policy.PolicyId,
                RowsAffected = rowsAffected
            };
        }
        catch (Exception ex)
        {
            return Error.Failure("Policy.Insert.Failed", ex.Message);
        }
    }

    public async Task<ErrorOr<PagedResult<ListPoliciesResponse>>> List(EncryptedInt categoryId)
    {
        var data = await context.ExpPolicies
            .AsNoTracking()
            .Where(x => x.CategoryId == categoryId)
            .Include(x => x.Category)
            .Select(x => new ListPoliciesResponse
            {
                PolicyId = x.PolicyId,
                CategoryId = x.CategoryId,
                CategoryName = x.Category != null ? x.Category.CategoryName : null,
                MaxAmount = x.MaxAmount,
                IsReceiptMandatory = x.IsReceiptMandatory,
                Description = x.Description,
                Created = x.Created
            })
            .ToListAsync();

        return new PagedResult<ListPoliciesResponse>
        {
            Data = data,
            Total = data.Count
        };
    }

    public async Task<ErrorOr<OperationResponse<EncryptedInt>>> Update(UpdatePolicyRequest request)
    {
        try
        {
            var policy = await context.ExpPolicies.FirstOrDefaultAsync(x => x.PolicyId == request.CategoryId);

            if (policy == null)
            {
                return Error.NotFound("Policy.NotFound", "Policy not found");
            }

            policy.CategoryId = request.CategoryId;
            policy.MaxAmount = request.MaxAmount;
            policy.IsReceiptMandatory = request.IsReceiptMandatory;
            policy.Description = request.Description;
            policy.ModifiedByUserId = request.ModifiedByUserId;
            policy.Modified = request.Modified;

            var rowsAffected = await context.SaveChangesAsync(default);

            return new OperationResponse<EncryptedInt>
            {
                Id = policy.PolicyId,
                RowsAffected = rowsAffected
            };
        }
        catch (Exception ex)
        {
            return Error.Failure("Policy.Update.Failed", ex.Message);
        }
    }

    public async Task<ErrorOr<OperationResponse<EncryptedInt>>> Delete(EncryptedInt policyId)
    {
        try
        {
            var policy = await context.ExpPolicies.FirstOrDefaultAsync(x => x.PolicyId == policyId);

            if (policy == null)
            {
                return Error.NotFound("Policy.NotFound", "Policy not found");
            }

            context.ExpPolicies.Remove(policy);

            var rowsAffected = await context.SaveChangesAsync(default);

            await context.SaveChangesAsync(default);

            return new OperationResponse<EncryptedInt>
            {
                Id = policy.PolicyId,
                RowsAffected = rowsAffected
            };
        }
        catch (Exception ex)
        {
            return Error.Failure("Policy.Delete.Failed", ex.Message);
        }
    }

    public async Task<ErrorOr<GetPolicyResponse>> Get(EncryptedInt policyId)
    {
        var id = (int)policyId;

        var policy = await context.ExpPolicies
            .AsNoTracking()
            .Where(x => x.PolicyId == id)
            .Select(x => new GetPolicyResponse
            {
                PolicyId = x.PolicyId,
                ProjectId = 0,
                CategoryId = x.CategoryId,
                MaxAmount = x.MaxAmount,
                IsReceiptMandatory = x.IsReceiptMandatory,
                Description = x.Description
            })
            .FirstOrDefaultAsync();

        if (policy == null)
        {
            return Error.NotFound("Policy.NotFound", "Policy not found");
        }

        return policy;
    }
}