# Abp.Web.SignalR.AspNetCore

Doc: https://aspnetboilerplate.com/Pages/Documents/SignalR-AspNetCore-Integration

# Module-zero-core-template

1. Clone https://github.com/acjh/module-zero-core-template.git
1. Switch branch to: `signalr-try`
1. Open `module-zero-core-template\aspnet-core\AbpCompanyName.AbpProjectName.sln`
1. Build in Debug mode: `AbpCompanyName.AbpProjectName.Web.Mvc` (or `*.Web.Host` for Angular)
1. Login as any user
1. Open the Console by pressing `Ctrl`+`Shift`+`I`
1. Copy-paste and run: `abp.signalr.hubs.common.invoke("sendMessage", "Hello World!")`
