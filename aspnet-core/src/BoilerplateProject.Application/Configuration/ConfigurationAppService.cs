using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Runtime.Session;
using BoilerplateProject.Configuration.Dto;

namespace BoilerplateProject.Configuration
{
    [AbpAuthorize]
    public class ConfigurationAppService : BoilerplateProjectAppServiceBase, IConfigurationAppService
    {
        public async Task ChangeUiTheme(ChangeUiThemeInput input)
        {
            await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.UiTheme, input.Theme);
        }
    }
}
