using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Abp;
using Abp.Extensions;
using Abp.Notifications;
using Abp.Timing;
using AbpCompanyName.AbpProjectName.Controllers;
using AbpCompanyName.AbpProjectName.Configuration;
using System.Linq;
using Abp.Configuration;
using System.Collections.Generic;

namespace AbpCompanyName.AbpProjectName.Web.Host.Controllers
{
    public class HomeController : AbpProjectNameControllerBase
    {
        private readonly INotificationPublisher _notificationPublisher;
        private readonly ISettingDefinitionManager _settingDefinitionManager;

        public HomeController(
            INotificationPublisher notificationPublisher,
            ISettingDefinitionManager settingDefinitionManager)
        {
            _notificationPublisher = notificationPublisher;
            _settingDefinitionManager = settingDefinitionManager;
        }

        public async Task<IActionResult> Index()
        {
            var rootSettingDefinitionGroupDto = await GetRootSettingDefinitionGroupDto();

            return Redirect("/swagger");
        }

        public async Task<SettingDefinitionGroupDto> GetRootSettingDefinitionGroupDto()
        {
            var groups = AppSettingProvider.Groups;
            var rootGroups = groups.Where(g => g.Parent == null).ToList();
            var settingDefinitions = _settingDefinitionManager.GetAllSettingDefinitions();
            var groupedSettingDefinitions = settingDefinitions.Where(sd => sd.Group != null).ToList();
            var settings = await SettingManager.GetAllSettingValuesAsync();

            return new SettingDefinitionGroupDto
            {
                Children = GetChildrenGroupDtos(rootGroups, groupedSettingDefinitions, settings),
                GroupName = null,
                Settings = settingDefinitions
                    .Where(sd => sd.Group == null)
                    .Select(sd => settings.Where(s => s.Name == sd.Name).FirstOrDefault())
                    .Where(s => s != null)
                    .ToList()
            };
        }

        public class SettingDefinitionGroupDto
        {
            public List<SettingDefinitionGroupDto> Children { get; set; }
            
            public string GroupName { get; set; }

            public List<ISettingValue> Settings { get; set; }
        }

        private List<SettingDefinitionGroupDto> GetChildrenGroupDtos(IReadOnlyList<SettingDefinitionGroup> groups, IReadOnlyList<SettingDefinition> settingDefinitions, IReadOnlyList<ISettingValue> settings)
        {
            return groups
                .Select(group => new SettingDefinitionGroupDto
                {
                    Children = GetChildrenGroupDtos(group.Children, settingDefinitions, settings),
                    GroupName = group.Name,
                    Settings = GetGroupSettings(group, settingDefinitions, settings)
                })
                .ToList();
        }

        private List<ISettingValue> GetGroupSettings(SettingDefinitionGroup group, IReadOnlyList<SettingDefinition> settingDefinitions, IReadOnlyList<ISettingValue> settings)
        {
            return settingDefinitions
                .Where(sd => sd.Group.Name == group.Name)
                .Select(sd => settings.Where(s => s.Name == sd.Name).FirstOrDefault())
                .Where(s => s != null)
                .ToList();
        }

        /// <summary>
        /// This is a demo code to demonstrate sending notification to default tenant admin and host admin uers.
        /// Don't use this code in production !!!
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task<ActionResult> TestNotification(string message = "")
        {
            if (message.IsNullOrEmpty())
            {
                message = "This is a test notification, created at " + Clock.Now;
            }

            var defaultTenantAdmin = new UserIdentifier(1, 2);
            var hostAdmin = new UserIdentifier(null, 1);

            await _notificationPublisher.PublishAsync(
                "App.SimpleMessage",
                new MessageNotificationData(message),
                severity: NotificationSeverity.Info,
                userIds: new[] { defaultTenantAdmin, hostAdmin }
            );

            return Content("Sent notification: " + message);
        }
    }
}
