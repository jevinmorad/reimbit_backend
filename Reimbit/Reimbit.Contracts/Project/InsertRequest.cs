using AegisInt.Core;

namespace Reimbit.Contracts.Project;

public class InsertRequest
{
    public EncryptedInt? ProjectId { get; set; }
    public required string ProjectName { get; set; }
    public string? ProjectLogoUrl { get; set; }
    public string? ProjectDetails { get; set; }
    public string? ProjectDescription { get; set; }
    public required int OrganizationId { get; set; }
    public required EncryptedInt ManagerId { get; set; }
    public int CreatedByUserId { get; set; }
    public int ModifiedByUserId { get; set; }
    public DateTime Created { get; set; }
    public DateTime Modified { get; set; }
    public bool IsActive { get; set; }
}
