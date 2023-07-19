using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace WIZLOG;

[Dependency(ReplaceServices = true)]
public class WIZLOGBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "WIZLOG";
}
