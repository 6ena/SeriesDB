using SerieDB.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace SerieDB.Permissions;

public class SerieDBPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(SerieDBPermissions.GroupName);

        //Define your own permissions here. Example:
        //myGroup.AddPermission(SerieDBPermissions.MyPermission1, L("Permission:MyPermission1"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<SerieDBResource>(name);
    }
}
