using System.Text.Json;
using System.Text.Json.Serialization;

namespace Reimbit.Application.Audit;

public interface IAuditLogger
{
    Task WriteAsync(
        string entityType,
        int? entityId,
        string action,
        int? organizationId,
        int? userId,
        object? oldValue,
        object? newValue,
        string? ipAddress,
        string? userAgent);
}

public static class AuditJson
{
    public static readonly JsonSerializerOptions Options = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        WriteIndented = false
    };
}