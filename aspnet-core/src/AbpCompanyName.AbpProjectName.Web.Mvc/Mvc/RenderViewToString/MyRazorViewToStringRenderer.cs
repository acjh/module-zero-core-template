using Abp.Dependency;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Mvc.RenderViewToString;
using System;
using System.Threading.Tasks;

namespace AbpCompanyName.AbpProjectName.Web.Mvc.RenderViewToString
{
    public class MyRazorViewToStringRenderer : RazorViewToStringRenderer, ITransientDependency
    {
        private ActionContext _actionContext;

        public MyRazorViewToStringRenderer(
            IRazorViewEngine viewEngine,
            ITempDataProvider tempDataProvider,
            IServiceProvider serviceProvider)
            : base(viewEngine, tempDataProvider, serviceProvider)
        {
        }

        public Task<string> RenderViewToStringAsync(ResultExecutingContext context)
        {
            var result = context.Result as ViewResult;
            if (result.ViewName == null)
            {
                result.ViewName = (context.ActionDescriptor as ControllerActionDescriptor).ActionName;
            }

            _actionContext = context;

            return RenderViewToStringAsync(result.ViewName, result.Model);
        }

        protected override ActionContext GetActionContext()
        {
            return _actionContext;
        }
    }
}
