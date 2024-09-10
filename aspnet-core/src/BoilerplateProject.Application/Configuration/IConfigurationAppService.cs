using System.Threading.Tasks;
using BoilerplateProject.Configuration.Dto;

namespace BoilerplateProject.Configuration
{
    public interface IConfigurationAppService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}
