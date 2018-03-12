using Abp.AspNetCore.Mvc.Results.Wrapping;
using Abp.Dependency;
using Abp.Threading;
using AbpCompanyName.AbpProjectName.Web.Mvc.RenderViewToString;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AbpCompanyName.AbpProjectName.Web.Mvc.Results.Wrapping
{
    public class MyViewActionResultWrapper : IAbpActionResultWrapper, ITransientDependency
    {
        private readonly IServiceProvider _serviceProvider;

        public MyViewActionResultWrapper(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void Wrap(ResultExecutingContext actionResult)
        {
            if (!(actionResult.Result is ViewResult))
            {
                throw new ArgumentException($"{nameof(actionResult)} should be ViewResult!");
            }

            var renderer = _serviceProvider.GetRequiredService<MyRazorViewToStringRenderer>();
            var s = AsyncHelper.RunSync(() => renderer.RenderViewToStringAsync(actionResult));
            var wrapper = new AbpJsonActionResultWrapper();

            actionResult.Result = new JsonResult(s);
            wrapper.Wrap(actionResult);
        }
    }
}
