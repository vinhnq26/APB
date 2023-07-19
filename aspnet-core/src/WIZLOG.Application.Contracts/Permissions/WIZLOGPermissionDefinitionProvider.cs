using WIZLOG.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace WIZLOG.Permissions;

public class WIZLOGPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(WIZLOGPermissions.GroupName);
        //Define your own permissions here. Example:
        //myGroup.AddPermission(WIZLOGPermissions.MyPermission1, L("Permission:MyPermission1"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<WIZLOGResource>(name);
    }
}
