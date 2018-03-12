using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Abp.AspNetCore.Mvc.Results.Wrapping;
using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.Modules;
using Abp.Reflection.Extensions;
using AbpCompanyName.AbpProjectName.Configuration;
using AbpCompanyName.AbpProjectName.Web.Mvc.Results.Wrapping;

namespace AbpCompanyName.AbpProjectName.Web.Startup
{
    [DependsOn(typeof(AbpProjectNameWebCoreModule))]
    public class AbpProjectNameWebMvcModule : AbpModule
    {
        private readonly IHostingEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public AbpProjectNameWebMvcModule(IHostingEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void PreInitialize()
        {
            Configuration.Navigation.Providers.Add<AbpProjectNameNavigationProvider>();

            Configuration.ReplaceService<IAbpActionResultWrapperFactory, MyActionResultWrapperFactory>(DependencyLifeStyle.Transient);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(AbpProjectNameWebMvcModule).GetAssembly());
        }
    }
}
