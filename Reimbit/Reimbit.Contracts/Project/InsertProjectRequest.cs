using AegisInt.Core;
using System.Text.Json.Serialization;

namespace Reimbit.Contracts.Project;

public class InsertProjectRequest
{
    public EncryptedInt? ProjectId { get; set; }
    public required string ProjectName { get; set; }
    public string? ProjectLogoUrl { get; set; }
    public string? ProjectDetails { get; set; }
    public string? ProjectDescription { get; set; }
    public required EncryptedInt ManagerId { get; set; }
    [JsonIgnore]
    public int OrganizationId { get; set; }
    [JsonIgnore]
    public int CreatedByUserId { get; set; }
    [JsonIgnore]
    public DateTime Created { get; set; }
    [JsonIgnore]
    public bool IsActive { get; set; }
}
