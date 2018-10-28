using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Timing;
using Abp.Zero;
using Abp.Zero.Configuration;
using AbpCompanyName.AbpProjectName.Authorization.Roles;
using AbpCompanyName.AbpProjectName.Authorization.Users;
using AbpCompanyName.AbpProjectName.Configuration;
using AbpCompanyName.AbpProjectName.Localization;
using AbpCompanyName.AbpProjectName.MultiTenancy;
using AbpCompanyName.AbpProjectName.Timing;

using Abp.Auditing;
using Abp.Configuration.Startup;
using Abp.Dependency;

using System;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Threading.Tasks;

namespace AbpCompanyName.AbpProjectName
{
    public class AuditingStore : IAuditingStore, ITransientDependency
    {
        public Task SaveAsync(AuditInfo auditInfo)
        {
            return Task.CompletedTask;
        }
    }

    [DependsOn(typeof(AbpZeroCoreModule))]
    public class AbpProjectNameCoreModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Auditing.IsEnabledForAnonymousUsers = true;

            // Declare entity types
            Configuration.Modules.Zero().EntityTypes.Tenant = typeof(Tenant);
            Configuration.Modules.Zero().EntityTypes.Role = typeof(Role);
            Configuration.Modules.Zero().EntityTypes.User = typeof(User);

            AbpProjectNameLocalizationConfigurer.Configure(Configuration.Localization);

            // Enable this line to create a multi-tenant application.
            Configuration.MultiTenancy.IsEnabled = AbpProjectNameConsts.MultiTenancyEnabled;

            // Configure roles
            AppRoleConfig.Configure(Configuration.Modules.Zero().RoleManagement);

            Configuration.Settings.Providers.Add<AppSettingProvider>();

            Configuration.ReplaceService<IAuditSerializer, JsonNetAuditSerializer>();

            Configuration.ReplaceService<IAuditingStore, AuditingStore>();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(AbpProjectNameCoreModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            IocManager.Resolve<AppTimes>().StartupTime = Clock.Now;
        }
    }

    public class JsonNetAuditSerializer : IAuditSerializer, ITransientDependency
    {
        private readonly IAuditingConfiguration _configuration;

        public JsonNetAuditSerializer(IAuditingConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Serialize(object obj)
        {
            var options = new JsonSerializerSettings
            {
                ContractResolver = new AuditingContractResolver(_configuration.IgnoredTypes)
            };

            return JsonConvert.SerializeObject(obj, options);
        }
    }

    public class MaskableAuditingContractResolver : AuditingContractResolver
    {
        public MaskableAuditingContractResolver(List<Type> ignoredTypes)
            : base(ignoredTypes)
        {
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);

            if (member.IsDefined(typeof(MaskedAuditedAttribute)))
            {
                property.ValueProvider = new MaskedValueProvider();
            }

            return property;
        }
    }

    public class MaskedValueProvider : IValueProvider
    {
        public object GetValue(object target)
        {
            return "***";
        }

        public void SetValue(object target, object value)
        {
            throw new NotImplementedException();
        }
    }

    public class MaskedAuditedAttribute : AuditedAttribute
    {
    }
}
