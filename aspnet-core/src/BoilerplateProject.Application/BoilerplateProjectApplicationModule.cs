using Abp.AutoMapper;
using Abp.BlobStoring;
using Abp.BlobStoring.FileSystem;
using Abp.Modules;
using Abp.Reflection.Extensions;
using BoilerplateProject.Authorization;

namespace BoilerplateProject
{
    [DependsOn(
        typeof(BoilerplateProjectCoreModule), 
        typeof(AbpAutoMapperModule), 
        typeof(AbpBlobStoringModule), 
        typeof(AbpBlobStoringFileSystemModule))]
    public class BoilerplateProjectApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Authorization.Providers.Add<BoilerplateProjectAuthorizationProvider>();
        }

        public override void Initialize()
        {
            var thisAssembly = typeof(BoilerplateProjectApplicationModule).GetAssembly();

            IocManager.RegisterAssemblyByConvention(thisAssembly);

            Configuration.Modules.AbpAutoMapper().Configurators.Add(
                // Scan the assembly for classes which inherit from AutoMapper.Profile
                cfg => cfg.AddMaps(thisAssembly)
            );

            Configuration.Modules.AbpBlobStoring().Containers.Configure<AbpBlobStoringOptions>(options =>
            {
                options.UseFileSystem(fileSystem =>
                {
                    fileSystem.BasePath = "C:\\BlobStorage";
                });
            });
        }
    }
}
