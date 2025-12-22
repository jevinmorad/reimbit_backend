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
        var response = new OperationResponse<EncryptedInt>();
        var dbContext = (DbContext)context;

        try
        {
            var policy = new ProjExpensePolicy
            {
                ProjectId = (int)request.ProjectId!,
                CategoryId = request.CategoryId,
                MaxAmount = request.MaxAmount,
                IsReceiptMandatory = request.IsReceiptMandatory,
                Description = request.Description,
                Created = DateTime.UtcNow
            };

            await context.ProjExpensePolicies.AddAsync(policy);
            await context.SaveChangesAsync(default);

            response.Id = policy.PolicyId;
            return response;
        }
        catch (Exception ex)
        {
            return Error.Failure("Policy.Insert.Failed", ex.Message);
        }
    }

    public async Task<ErrorOr<PagedResult<ListPoliciesResponse>>> List(EncryptedInt projectId)
    {
        int id = projectId;
        var query = context.ProjExpensePolicies
            .Include(x => x.Project)
            .Include(x => x.Category)
            .Where(x => x.ProjectId == id)
            .Select(x => new ListPoliciesResponse
            {
                PolicyId = x.PolicyId,
                ProjectId = x.ProjectId,
                ProjectName = x.Project.ProjectName,
                CategoryId = x.CategoryId,
                CategoryName = x.Category != null ? x.Category.CategoryName : null,
                MaxAmount = x.MaxAmount,
                IsReceiptMandatory = x.IsReceiptMandatory,
                Description = x.Description,
                Created = x.Created
            });

        var data = await query.ToListAsync();

        return new PagedResult<ListPoliciesResponse>
        {
            Data = data,
            Total = data.Count
        };
    }

    public async Task<ErrorOr<OperationResponse<EncryptedInt>>> Update(UpdatePolicyRequest request)
    {
        var response = new OperationResponse<EncryptedInt>();
        
        try
        {
            int id = request.PolicyId;
            var policy = await context.ProjExpensePolicies.FirstOrDefaultAsync(x => x.PolicyId == id);

            if (policy == null)
            {
                return Error.NotFound("Policy.NotFound", "Policy not found");
            }

            policy.ProjectId = request.ProjectId;
            policy.CategoryId = request.CategoryId;
            policy.MaxAmount = request.MaxAmount;
            policy.IsReceiptMandatory = request.IsReceiptMandatory;
            policy.Description = request.Description;

            await context.SaveChangesAsync(default);

            response.Id = policy.PolicyId;
            return response;
        }
        catch (Exception ex)
        {
            return Error.Failure("Policy.Update.Failed", ex.Message);
        }
    }

    public async Task<ErrorOr<OperationResponse<EncryptedInt>>> Delete(EncryptedInt policyId)
    {
        var response = new OperationResponse<EncryptedInt>();
        
        try
        {
            int id = policyId;
            var policy = await context.ProjExpensePolicies.FirstOrDefaultAsync(x => x.PolicyId == id);

            if (policy == null)
            {
                return Error.NotFound("Policy.NotFound", "Policy not found");
            }

            context.ProjExpensePolicies.Remove(policy);
            await context.SaveChangesAsync(default);

            response.Id = policy.PolicyId;
            return response;
        }
        catch (Exception ex)
        {
            return Error.Failure("Policy.Delete.Failed", ex.Message);
        }
    }

    public async Task<ErrorOr<GetPolicyResponse>> Get(EncryptedInt policyId)
    {
        int id = policyId;
        var policy = await context.ProjExpensePolicies
            .Where(x => x.PolicyId == id)
            .Select(x => new GetPolicyResponse
            {
                PolicyId = x.PolicyId,
                ProjectId = x.ProjectId,
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