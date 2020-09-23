using ApiPractice.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace ApiPractice
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            //加入Filter
            RegisterWebApiFilters(GlobalConfiguration.Configuration.Filters);

        }
        public static void RegisterWebApiFilters(System.Web.Http.Filters.HttpFilterCollection filters)
        {
            //加進Attribute
            filters.Add(new JwtAuthAttribute());
        }
    }
}
