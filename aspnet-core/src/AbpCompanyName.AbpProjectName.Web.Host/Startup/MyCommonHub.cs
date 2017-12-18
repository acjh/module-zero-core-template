using Abp.Auditing;
using Abp.RealTime;
using Abp.Web.SignalR.Hubs;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace AbpCompanyName.AbpProjectName.Web.Startup
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
