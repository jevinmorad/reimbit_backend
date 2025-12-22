using AegisInt.Core;
using Common.Data.Models;
using ErrorOr;
using Microsoft.EntityFrameworkCore;
using Reimbit.Contracts.Project;
using Reimbit.Domain.Interfaces;
using Reimbit.Domain.Models;
using Reimbit.Domain.Repositories;

namespace Reimbit.Infrastructure.Repositories;

public class ProjectRepository(IApplicationDbContext context) : IProjectRepository
{
    public async Task<ErrorOr<OperationResponse<EncryptedInt>>> Delete(EncryptedInt projectId)
    {
        var response = new OperationResponse<EncryptedInt>();
        var dbContext = (DbContext)context;

        await using var tx = await dbContext.Database.BeginTransactionAsync();

        try
        {
            int id = projectId;
            var project = await context.ProjProjects.FirstOrDefaultAsync(x => x.ProjectId == id);

            if (project == null)
            {
                return Error.NotFound("Project.NotFound", "Project not found");
            }

            // Soft Delete
            project.IsActive = false;
            project.Modified = DateTime.UtcNow;
            
            var logProject = new LogProjProject
            {
                Iud = "D",
                IuddateTime = DateTime.UtcNow,
                IudbyUserId = 0,
                ProjectId = project.ProjectId,
                ProjectName = project.ProjectName,
                OrganizationId = project.OrganizationId,
                ProjectLogoUrl = project.ProjectLogoUrl,
                ProjectDetails = project.ProjectDetails,
                ProjectDescription = project.ProjectDescription,
                ManagerId = project.ManagerId,
                Created = project.Created,
                CreatedByUserId = project.CreatedByUserId,
                ModifiedByUserId = project.ModifiedByUserId,
                IsActive = false
            };

            await context.LogProjProjects.AddAsync(logProject);
            await context.SaveChangesAsync(default);

            await tx.CommitAsync();

            response.Id = project.ProjectId;

            return response;
        }
        catch (Exception ex)
        {
            await tx.RollbackAsync();
            return Error.Failure("Project.Delete.Failed", ex.Message);
        }
    }

    public async Task<ErrorOr<GetProjectResponse>> Get(EncryptedInt projectId)
    {
        int id = projectId;
        var project = await context.ProjProjects
            .Where(x => x.ProjectId == id)
            .Select(x => new GetProjectResponse
            {
                ProjectId = x.ProjectId,
                ProjectName = x.ProjectName,
                ProjectLogoUrl = x.ProjectLogoUrl,
                ProjectDetails = x.ProjectDetails,
                ProjectDescription = x.ProjectDescription,
                ManagerId = x.ManagerId,
                IsActive = x.IsActive
            })
            .FirstOrDefaultAsync();

        if (project == null)
        {
            return Error.NotFound("Project.NotFound", "Project not found");
        }

        return project;
    }

    public async Task<ErrorOr<OperationResponse<EncryptedInt>>> Insert(InsertProjectRequest request)
    {
        var response = new OperationResponse<EncryptedInt>();

        var dbContext = (DbContext)context;

        await using var tx = await dbContext.Database.BeginTransactionAsync();

        try
        {
            var project = new ProjProject
            {
                ProjectName = request.ProjectName,
                OrganizationId = request.OrganizationId,
                ProjectLogoUrl = request.ProjectLogoUrl,
                ProjectDetails = request.ProjectDetails,
                ProjectDescription = request.ProjectDescription,
                ManagerId = request.ManagerId,
                Created = request.Created,
                CreatedByUserId = request.CreatedByUserId,
                ModifiedByUserId = request.ModifiedByUserId,
                IsActive = request.IsActive
            };

            await context.ProjProjects.AddAsync(project);
            await context.SaveChangesAsync(default);

            var logProject = new LogProjProject
            {
                Iud = "I",
                IuddateTime = request.Created,
                IudbyUserId = request.CreatedByUserId,
                ProjectName = request.ProjectName,
                OrganizationId = request.OrganizationId,
                ProjectLogoUrl = request.ProjectLogoUrl,
                ProjectDetails = request.ProjectDetails,
                ProjectDescription = request.ProjectDescription,
                ManagerId = request.ManagerId,
                Created = request.Created,
                CreatedByUserId = request.CreatedByUserId,
                ModifiedByUserId = request.CreatedByUserId,
                IsActive = request.IsActive
            };

            await context.LogProjProjects.AddAsync(logProject);

            await context.SaveChangesAsync(default);

            await tx.CommitAsync();

            response.Id = project.ProjectId;

            return response;
        }
        catch (Exception ex)
        {
            await tx.RollbackAsync();
            return Error.Failure("Project.Insert.Failed", ex.Message);
        }
    }

    public async Task<ErrorOr<PagedResult<ListProjectsResponse>>> List(int organizationId)
    {
        var projects = await context.ProjProjects
            .Where(x => x.OrganizationId == organizationId)
            .Select(x => new
            {
                x.ProjectId,
                x.ProjectName,
                x.ProjectLogoUrl,
                x.ProjectDetails,
                x.IsActive
            })
            .ToListAsync();

        var data = projects.Select(x => new ListProjectsResponse
        {
            ProjectId = x.ProjectId,
            ProjectName = x.ProjectName,
            ProjectLogoUrl = x.ProjectLogoUrl,
            ProjectDetails = x.ProjectDetails,
            IsActive = x.IsActive
        }).ToList();

        //var query =
        //    from p in context.ProjProjects
        //    where p.OrganizationId == OrganizationId
        //    select new ListResponse
        //    {
        //        ProjectId = p.ProjectId,
        //        ProjectName = p.ProjectName,
        //        ProjectLogoUrl = p.ProjectLogoUrl,
        //        ProjectDetails = p.ProjectDetails,
        //        IsActive = p.IsActive
        //    };

        //var data = await query.ToListAsync();

        return new PagedResult<ListProjectsResponse>
        {
            Data = data,
            Total = data.Count
        };
    }

    public async Task<ErrorOr<OperationResponse<EncryptedInt>>> Update(UpdateProjectRequest request)
    {
        var response = new OperationResponse<EncryptedInt>();
        var dbContext = (DbContext)context;

        await using var tx = await dbContext.Database.BeginTransactionAsync();

        try
        {
            var project = await context.ProjProjects.FirstOrDefaultAsync(x => x.ProjectId == (int)request.ProjectId);

            if (project == null)
            {
                return Error.NotFound("Project.NotFound", "Project not found");
            }

            project.ProjectName = request.ProjectName;
            project.OrganizationId = request.OrganizationId;
            project.ProjectLogoUrl = request.ProjectLogoUrl;
            project.ProjectDetails = request.ProjectDetails;
            project.ProjectDescription = request.ProjectDescription;
            project.ManagerId = request.ManagerId;
            project.Modified = request.Modified;
            project.ModifiedByUserId = request.ModifiedByUserId;
            project.IsActive = request.IsActive;

            var logProject = new LogProjProject
            {
                Iud = "U",
                IuddateTime = request.Modified,
                IudbyUserId = request.ModifiedByUserId,
                ProjectId = project.ProjectId,
                ProjectName = project.ProjectName,
                OrganizationId = project.OrganizationId,
                ProjectLogoUrl = project.ProjectLogoUrl,
                ProjectDetails = project.ProjectDetails,
                ProjectDescription = project.ProjectDescription,
                ManagerId = project.ManagerId,
                Created = project.Created,
                CreatedByUserId = project.CreatedByUserId,
                ModifiedByUserId = project.ModifiedByUserId,
                IsActive = project.IsActive
            };

            await context.LogProjProjects.AddAsync(logProject);
            await context.SaveChangesAsync(default);

            await tx.CommitAsync();

            response.Id = project.ProjectId;

            return response;
        }
        catch (Exception ex)
        {
            await tx.RollbackAsync();
            return Error.Failure("Project.Update.Failed", ex.Message);
        }
    }

    public async Task<ErrorOr<ViewProjectResponse>> View(EncryptedInt projectId)
    {
        int id = projectId;
        
        var project = await context.ProjProjects
            .Include(x => x.Manager)
            .Include(x => x.CreatedByUser)
            .Include(x => x.ProjProjectMembers).ThenInclude(m => m.User)
            .Include(x => x.ExpExpenses)
            .Include(x => x.ExpReports)
            .AsSplitQuery()
            .FirstOrDefaultAsync(x => x.ProjectId == id);

        if (project == null)
        {
            return Error.NotFound("Project.NotFound", "Project not found");
        }

        var response = new ViewProjectResponse
        {
            ProjectName = project.ProjectName,
            ManagerDisplayName = $"{project.Manager.FirstName} {project.Manager.LastName}",
            CreatedByUserName = $"{project.CreatedByUser.FirstName} {project.CreatedByUser.LastName}",
            IsActive = project.IsActive,
            Created = project.Created,
            Modified = project.Modified,
            TotalExpense = project.ExpExpenses.Sum(e => e.Amount),
            AcceptedAmount = project.ExpReports.Sum(r => r.AcceptedAmount),
            RejectedAmount = project.ExpReports.Sum(r => r.RejectedAmount),
            PendingToViewAmount = project.ExpExpenses.Where(e => e.ExpenseStatus == "submitted").Sum(e => e.Amount),
            Employees = project.ProjProjectMembers.Select(m => $"{m.User.FirstName} {m.User.LastName}").ToList()
        };

        return response;
    }
}
