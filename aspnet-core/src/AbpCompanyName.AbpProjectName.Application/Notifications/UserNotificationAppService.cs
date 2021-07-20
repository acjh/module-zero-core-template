using Abp;
using Abp.Application.Services;
using Abp.Notifications;
using AbpCompanyName.AbpProjectName.Notifications.Dto;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AbpCompanyName.AbpProjectName.Notifications
{
    public class UserNotificationAppService : ApplicationService
    {
        private IUserNotificationManager _userNotificationManager;
        private readonly INotificationSubscriptionManager _notificationSubscriptionManager;
        private readonly INotificationPublisher _notificationPublisher;

        public UserNotificationAppService(
            IUserNotificationManager userNotificationManager,
            INotificationSubscriptionManager notificationSubscriptionManager,
            INotificationPublisher notificationPublisher)
        {
            _userNotificationManager = userNotificationManager;
            _notificationSubscriptionManager = notificationSubscriptionManager;
            _notificationPublisher = notificationPublisher;
        }

        public async Task SubscribeAdminNotification(long userId)
        {
            await _notificationSubscriptionManager.SubscribeAsync(new UserIdentifier(AbpSession.TenantId, userId), "AdminNotification");
        }

        public async Task PublishAdminNotification(PublishAdminNotificationDto input)
        {
            if (!AbpSession.TenantId.HasValue)
                await _notificationPublisher.PublishAsync(notificationName: "AdminNotification", data: new
                        AdminNotificationData(input.Title, input.Message, input.SenderUserName, input.FileUrl));

            else if (AbpSession.TenantId.HasValue && !input.TargetUserIds.Any())
                await _notificationPublisher.PublishAsync(notificationName: "AdminNotification", data: new
                         AdminNotificationData(input.Title, input.Message, input.SenderUserName, input.FileUrl), tenantIds: new[] { AbpSession.TenantId });

            else if (AbpSession.TenantId.HasValue && input.TargetUserIds.Any())
            {
                var targetUsers = input.TargetUserIds.Select(u => new UserIdentifier(AbpSession?.TenantId, u)).ToArray();
                await _notificationPublisher.PublishAsync(notificationName: "AdminNotification", data: new AdminNotificationData(input.Title, input.Message, input.SenderUserName, input.FileUrl), userIds: targetUsers);
            }
        }

        public async Task<List<AdminNotificationData>> GetUserNotifications(long userId)
        {
            var userIdentifier = new UserIdentifier(AbpSession.TenantId, userId);
            var userNotifications = await _userNotificationManager.GetUserNotificationsAsync(userIdentifier);
            var notifications = userNotifications
                .Where(n => n.Notification.NotificationName == "AdminNotification")
                .Select(n => (AdminNotificationData)n.Notification.Data)
                .ToList();

            return notifications;
        }
    }
}
