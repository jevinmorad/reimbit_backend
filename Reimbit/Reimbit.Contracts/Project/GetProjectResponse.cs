using AegisInt.Core;

namespace Reimbit.Contracts.Project;

public class GetProjectResponse
{
    public required EncryptedInt ProjectId { get; set; }
    public required string ProjectName { get; set; }
    public string? ProjectLogoUrl { get; set; }
    public string? ProjectDetails { get; set; }
    public string? ProjectDescription { get; set; }
    public required EncryptedInt ManagerId { get; set; }
    public bool IsActive { get; set; }
}
