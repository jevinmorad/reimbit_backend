using AegisInt.Core;

namespace Reimbit.Contracts.Project;

public class ListResponse
{
    public EncryptedInt ProjectId { get; set; }
    public required string ProjectName { get; set; }
    public string? ProjectLogoUrl { get; set; }
    public string? ProjectDetails { get; set; }
    public bool IsActive { get; set; }
}
