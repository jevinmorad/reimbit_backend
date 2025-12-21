namespace Common.PermissionModule;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = false)]
public class ModuleMetaData : Attribute
{
    public required string Name { get; set; }

    public string? Description { get; set; }

    public string? GroupName { get; set; }

    public required int Order { get; set; }

    public string? Icon { get; set; }

    public bool IsMenu { get; set; }

    public bool IsGroup { get; set; }
}
