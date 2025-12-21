namespace Common.PermissionModule;

[AttributeUsage(AttributeTargets.Field, Inherited = true)]
public class PermissionMetadata : Attribute
{
    public required string Name;
    public required string Description;
}
