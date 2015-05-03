using System;
using Nancy;

namespace OwinSample.Web.NancyModules
{
    public class HomeModule : NancyModule
    {
        public HomeModule()
        {
            Get["/"] = x => Response.AsText(string.Format(
                "I'm running on {0} \r\nFrom assembly {1}",
                Environment.OSVersion,
                typeof(Program).Assembly.FullName
                ));
        }
    }
}