# Abp.Web.SignalR.AspNetCore

1. Clone https://github.com/acjh/aspnetboilerplate.git
1. Switch branch to: `signalr`
1. Open `aspnetboilerplate\Abp.sln`
1. Add remote NuGet package source: https://dotnet.myget.org/F/aspnetcore-dev/api/v3/index.json
1. Build in Release mode: `Abp.Web.SignalR.AspNetCore`
1. Open `aspnetboilerplate\common.props`
1. Change `Version` to `3.2.6`
1. Open `aspnetboilerplate\nupkg\pack.ps1`
1. Remove all except `Abp` and `Abp.Web.SignalR.AspNetCore` from `$projects`
1. Run `aspnetboilerplate\nupkg\pack.ps1`

Doc: https://github.com/acjh/aspnetboilerplate/blob/signalr/doc/WebSite/SignalR-AspNetCore-Integration.md

# Module-zero-core-template

1. Clone https://github.com/acjh/module-zero-core-template.git
1. Switch branch to: `signalr-try`
1. Open `module-zero-core-template\aspnet-core\AbpCompanyName.AbpProjectName.sln`
1. Add local NuGet package source: `aspnetboilerplate\nupkg`
1. Restore NuGet packages
1. Build in Debug mode: `AbpCompanyName.AbpProjectName.Web.Mvc` (or `*.Web.Host` for Angular)
1. Run by pressing `F5` key
1. Login as any user
1. Open the Console by pressing `Ctrl`+`Shift`+`I`
1. Copy-paste and run: `abp.signalr.hubs.common.invoke("sendMessage", "Hello World!")`
