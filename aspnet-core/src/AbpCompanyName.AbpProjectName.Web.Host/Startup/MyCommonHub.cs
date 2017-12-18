using Abp.Auditing;
using Abp.RealTime;
using Abp.Web.SignalR.Hubs;
using System.Threading.Tasks;

#if FEATURE_SIGNALR_ASPNETCORE
using Microsoft.AspNetCore.SignalR;

namespace AbpCompanyName.AbpProjectName.Web.Host.Startup
{
    public class MyCommonHub : AbpCommonHub
    {
        public MyCommonHub(
            IOnlineClientManager onlineClientManager,
            IClientInfoProvider clientInfoProvider
            ) : base(onlineClientManager, clientInfoProvider)
        {
        }

        public async Task SendMessage(string message)
        {
            await Clients.All.InvokeAsync("getMessage", string.Format("User {0}: {1}", AbpSession.UserId, message));
        }
    }
}
#endif
