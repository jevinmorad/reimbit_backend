namespace Common.PermissionModule;

public class ModulePermissionMetaData<TModule> : Attribute where TModule : Enum
{
    protected TModule module;
    public TModule Module => module;
}
