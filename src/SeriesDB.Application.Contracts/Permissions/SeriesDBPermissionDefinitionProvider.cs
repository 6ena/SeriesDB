using SeriesDB.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace SeriesDB.Permissions;

public class SeriesDBPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(SeriesDBPermissions.GroupName);

        //Define your own permissions here. Example:
        //myGroup.AddPermission(SeriesDBPermissions.MyPermission1, L("Permission:MyPermission1"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<SeriesDBResource>(name);
    }
}
