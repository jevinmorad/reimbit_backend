using AegisInt.Core;
using System.Text.Json.Serialization;

namespace Reimbit.Contracts.Project;

public class UpdateProjectRequest
{
    public EncryptedInt ProjectId { get; set; }
    public required string ProjectName { get; set; }
    public string? ProjectLogoUrl { get; set; }
    public string? ProjectDetails { get; set; }
    public string? ProjectDescription { get; set; }
    public required EncryptedInt ManagerId { get; set; }
    public required bool IsActive { get; set; }
    [JsonIgnore]
    public int OrganizationId { get; set; }
    [JsonIgnore]
    public int ModifiedByUserId { get; set; }
    [JsonIgnore]
    public DateTime Created { get; set; }
    [JsonIgnore]
    public DateTime Modified { get; set; }
}
