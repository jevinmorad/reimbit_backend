using AegisInt.Core;
using Common.Data.Models;
using Common.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Reimbit.Application.Projects.Project;
using Reimbit.Contracts.Project;

namespace Reimbit.Web.Controllers;

[ApiController]
[Authorize]
[Route("/api/[controller]")]
public class ProjectController(
    IProjectService projectService,
    ICurrentUserProvider currentUserProvider
) : ApiController(currentUserProvider)
{
    [HttpPost]
    [Produces<OperationResponse<EncryptedInt>>]
    [EndpointSummary("Create project")]
    public async Task<IActionResult> Insert([FromBody] InsertProjectRequest request)
    {
        var result = await projectService.Insert(request);

        return result.Match(_ => Ok(result.Value), Problem);
    }

    [HttpGet]
    [Produces<PagedResult<ListProjectsResponse>>]
    [EndpointSummary("Project list")]
    public async Task<IActionResult> List()
    {
        var result = await projectService.List();

        return result.Match(_ => Ok(result.Value), Problem);
    }

    [HttpPut]
    [Produces<OperationResponse<EncryptedInt>>]
    [EndpointSummary("Update project")]
    public async Task<IActionResult> Update([FromBody] UpdateProjectRequest request)
    {
        var result = await projectService.Update(request);

        return result.Match(_ => Ok(result.Value), Problem);
    }

    [HttpGet("{id}")]
    [Produces<GetProjectResponse>]
    [EndpointSummary("Get project")]
    public async Task<IActionResult> Get(EncryptedInt id)
    {
        var result = await projectService.Get(id);

        return result.Match(_ => Ok(result.Value), Problem);
    }

    [HttpDelete("{id}")]
    [Produces<OperationResponse<EncryptedInt>>]
    [EndpointSummary("Delete project")]
    public async Task<IActionResult> Delete(EncryptedInt id)
    {
        var result = await projectService.Delete(id);

        return result.Match(_ => Ok(result.Value), Problem);
    }

    [HttpGet("view/{id}")]
    [Produces<ViewProjectResponse>]
    [EndpointSummary("View project details")]
    public async Task<IActionResult> View(EncryptedInt id)
    {
        var result = await projectService.View(id);

        return result.Match(_ => Ok(result.Value), Problem);
    }
}