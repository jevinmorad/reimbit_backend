namespace Reimbit.Domain.Models;

public partial class SysAuditLog
{
    public long AuditLogId { get; set; }

    public int? OrganizationId { get; set; }

    public int? UserId { get; set; }

    public string EntityType { get; set; } = null!;

    public int? EntityId { get; set; }

    public string Action { get; set; } = null!;

    public string? OldValue { get; set; }

    public string? NewValue { get; set; }

    public string? Ipaddress { get; set; }

    public string? UserAgent { get; set; }

    public DateTime CreatedAt { get; set; }
}
