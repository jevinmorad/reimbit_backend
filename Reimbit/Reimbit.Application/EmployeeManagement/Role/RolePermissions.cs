using Common.PermissionModule;
using Reimbit.Application.Common.Module;

namespace Reimbit.Application.EmployeeManagement.Role;

[ModulePermissionMetaData(SubModules.RoleManagement)]
public class RolePermissions
{
    [PermissionMetadata(Name = "Create", Description = "Allows to create new role")]
    public const string Insert = "10301";

    [PermissionMetadata(Name = "List", Description = "Allows to list all role")]
    public const string List = "10302";

    [PermissionMetadata(Name = "Update", Description = "Allows to update role")]
    public const string Update = "10303";

    [PermissionMetadata(Name = "Delete", Description = "Allows to delete role")]
    public const string Delete = "10304";

    [PermissionMetadata(Name = "Options", Description = "Allows to fetch options of role")]
    public const string Options = "10305";

    [PermissionMetadata(Name = "Get", Description = "Allows to get role")]
    public const string Get = "10306";

    [PermissionMetadata(Name = "View", Description = "Allows to view role")]
    public const string View = "10307";

}
