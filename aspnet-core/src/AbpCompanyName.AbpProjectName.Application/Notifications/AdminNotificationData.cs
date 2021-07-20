using Abp.Notifications;

namespace AbpCompanyName.AbpProjectName.Notifications
{
    public class AdminNotificationData : NotificationData
    {
        public string Title { get; set; }

        public string Message { get; set; }

        public string SenderUserName { get; set; }

        public string FileUrl { get; set; }

        public int[] TargetUserIds { get; set; }

        public AdminNotificationData(string title, string message, string senderUserName, string fileUrl)
        {
            Title = title;
            Message = message;
            SenderUserName = senderUserName;
            FileUrl = fileUrl;
        }
    }
}
