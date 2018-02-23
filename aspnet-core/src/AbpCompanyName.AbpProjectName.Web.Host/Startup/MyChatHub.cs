#if FEATURE_SIGNALR_ASPNETCORE
using Abp.AspNetCore.SignalR.Hubs;
using Abp.Dependency;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace AbpCompanyName.AbpProjectName.Web.Host.Startup
{
    public class MyChatHub : AbpHubBase, ITransientDependency
    {
        public async Task SendMessage(string message)
        {
            await Clients.All.InvokeAsync("getMessage", string.Format("User {0}: {1}", AbpSession.UserId, message));
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }
    }
}
#endif
