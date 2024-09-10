using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using Microsoft.AspNetCore.Identity;

namespace BoilerplateProject.Controllers
{
    public abstract class BoilerplateProjectControllerBase: AbpController
    {
        protected BoilerplateProjectControllerBase()
        {
            LocalizationSourceName = BoilerplateProjectConsts.LocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
