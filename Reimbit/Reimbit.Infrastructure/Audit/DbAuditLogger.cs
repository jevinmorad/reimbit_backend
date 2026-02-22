using System.Text.Json;
using Reimbit.Application.Audit;
using Reimbit.Domain.Interfaces;
using Reimbit.Domain.Models;

namespace Reimbit.Infrastructure.Audit;

public sealed class DbAuditLogger(IApplicationDbContext context) : IAuditLogger
{
    public async Task WriteAsync(
        string entityType,
        int? entityId,
        string action,
        int? organizationId,
        int? userId,
        object? oldValue,
        object? newValue,
        string? ipAddress,
        string? userAgent)
    {
        var row = new SysAuditLog
        {
            OrganizationId = organizationId,
            UserId = userId,
            EntityType = entityType,
            EntityId = entityId,
            Action = action,
            OldValue = oldValue == null ? null : JsonSerializer.Serialize(oldValue, AuditJson.Options),
            NewValue = newValue == null ? null : JsonSerializer.Serialize(newValue, AuditJson.Options),
            Ipaddress = ipAddress,
            UserAgent = userAgent,
            CreatedAt = DateTime.UtcNow
        };

        context.SysAuditLogs.Add(row);
        await context.SaveChangesAsync();
    }
}