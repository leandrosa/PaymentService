using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Payments.Presentation.RouteConfigs;

namespace Payments.Presentation.Extensions
{
    public static class MvcOptionsExtensions
    {
        public static void UseCentralRoutePrefix(this MvcOptions opts, IRouteTemplateProvider routeAttribute)
            => opts.Conventions.Insert(0, new RouteConvention(routeAttribute));
    }
}
