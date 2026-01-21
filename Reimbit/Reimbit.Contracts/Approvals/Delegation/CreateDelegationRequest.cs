using AegisInt.Core;
using System.Text.Json.Serialization;

namespace Reimbit.Contracts.Approvals.Delegation;

public sealed class CreateDelegationRequest
{
    public required EncryptedInt DelegateUserId { get; set; }
    public required DateTime ValidFrom { get; set; }
    public required DateTime ValidTo { get; set; }

    [JsonIgnore]
    public int OrganizationId { get; set; }
    [JsonIgnore]
    public int UserId { get; set; }
}