namespace AbpCompanyName.AbpProjectName.Notifications.Dto
{
    public class PublishAdminNotificationDto
    {
        public string Title { get; set; }

        public string Message { get; set; }

        public string SenderUserName { get; set; }

        public string FileUrl { get; set; }

        public int[] TargetUserIds { get; set; }
    }
}
