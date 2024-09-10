using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using BoilerplateProject.Configuration;

namespace BoilerplateProject.Web.Host.Startup
{
    [DependsOn(
       typeof(BoilerplateProjectWebCoreModule))]
    public class BoilerplateProjectWebHostModule: AbpModule
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public BoilerplateProjectWebHostModule(IWebHostEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(BoilerplateProjectWebHostModule).GetAssembly());
        }
    }
}
