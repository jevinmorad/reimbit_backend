using AegisInt.Core;

namespace Reimbit.Contracts.Approvals.Delegation;

public sealed class DelegationResponse
{
    public EncryptedInt DelegateId { get; set; }
    public EncryptedInt UserId { get; set; }
    public EncryptedInt DelegateUserId { get; set; }
    public string DelegateDisplayName { get; set; } = string.Empty;
    public DateTime ValidFrom { get; set; }
    public DateTime ValidTo { get; set; }
    public bool IsActive { get; set; }
}