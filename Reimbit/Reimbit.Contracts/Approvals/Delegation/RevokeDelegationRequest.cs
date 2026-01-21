using AegisInt.Core;
using System.Text.Json.Serialization;

namespace Reimbit.Contracts.Approvals.Delegation;

public sealed class RevokeDelegationRequest
{
    public required EncryptedInt DelegateId { get; set; }

    [JsonIgnore]
    public int OrganizationId { get; set; }
    [JsonIgnore]
    public int UserId { get; set; }
    [JsonIgnore]
    public bool IsForce { get; set; }
}