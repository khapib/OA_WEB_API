using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using System.Web.Http.ExceptionHandling;

using OA_WEB_API.Core.Filters.ActionFilters;
using OA_WEB_API.Core.Filters.ExceptionFilters;
using OA_WEB_API.Core.Handlers;
using OA_WEB_API.Core.Selectors;

namespace OA_WEB_API
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.EnableCors(new EnableCorsAttribute("*", "*", "*"));

            // Web API 設定和服務

            // Web API 路由
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/octet-stream"));

            // 預設傳回 JSON 格式
            var appXmlType = config.Formatters.XmlFormatter.SupportedMediaTypes.FirstOrDefault(t => t.MediaType == "application/xml");
            config.Formatters.XmlFormatter.SupportedMediaTypes.Remove(appXmlType);

            config.Services.Replace(typeof(IExceptionHandler), new GlobalApiExceptionHandler());
            config.Services.Replace(typeof(IHttpControllerSelector), new HttpNotFoundAwareDefaultHttpControllerSelector(config));
            config.Services.Replace(typeof(IHttpActionSelector), new HttpNotFoundAwareControllerActionSelector());

            // 統一傳回 JSON 格式
            config.Filters.Add(new ApiResponseAttribute());
            config.Filters.Add(new ApiExceptionResponseAttribute());
        }
    }
}
