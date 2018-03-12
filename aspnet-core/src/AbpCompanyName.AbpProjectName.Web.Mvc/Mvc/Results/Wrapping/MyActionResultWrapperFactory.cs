using Abp.AspNetCore.Mvc.Results.Wrapping;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AbpCompanyName.AbpProjectName.Web.Mvc.Results.Wrapping
{
    public class MyActionResultWrapperFactory : IAbpActionResultWrapperFactory
    {
        public IAbpActionResultWrapper CreateFor(ResultExecutingContext actionResult)
        {
            var wrapper = new AbpActionResultWrapperFactory().CreateFor(actionResult);
            if (wrapper is NullAbpActionResultWrapper && actionResult.Result is ViewResult)
            {
                wrapper = new MyViewActionResultWrapper(actionResult.HttpContext.RequestServices);
            }

            return wrapper;
        }
    }
}
