using Common.PermissionModule;

namespace Reimbit.Application.Common.Module;

public class ModulePermissionMetaData : ModulePermissionMetaData<SubModules>
{
    public ModulePermissionMetaData(SubModules module)
    {
        this.module = module;
    }
}
