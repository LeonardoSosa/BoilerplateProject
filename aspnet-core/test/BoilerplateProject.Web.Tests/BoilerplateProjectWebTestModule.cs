using Abp.AspNetCore;
using Abp.AspNetCore.TestBase;
using Abp.Modules;
using Abp.Reflection.Extensions;
using BoilerplateProject.EntityFrameworkCore;
using BoilerplateProject.Web.Startup;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace BoilerplateProject.Web.Tests
{
    [DependsOn(
        typeof(BoilerplateProjectWebMvcModule),
        typeof(AbpAspNetCoreTestBaseModule)
    )]
    public class BoilerplateProjectWebTestModule : AbpModule
    {
        public BoilerplateProjectWebTestModule(BoilerplateProjectEntityFrameworkModule abpProjectNameEntityFrameworkModule)
        {
            abpProjectNameEntityFrameworkModule.SkipDbContextRegistration = true;
        } 
        
        public override void PreInitialize()
        {
            Configuration.UnitOfWork.IsTransactional = false; //EF Core InMemory DB does not support transactions.
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(BoilerplateProjectWebTestModule).GetAssembly());
        }
        
        public override void PostInitialize()
        {
            IocManager.Resolve<ApplicationPartManager>()
                .AddApplicationPartsIfNotAddedBefore(typeof(BoilerplateProjectWebMvcModule).Assembly);
        }
    }
}