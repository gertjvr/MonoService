using System;
using Microsoft.Owin.Hosting;
using Topshelf;

namespace OwinSample.Web
{
    public class WebService : ServiceControl
    {
        private IDisposable _webApp;

        public bool Start(HostControl hostControl)
        {
            try
            {
                _webApp = WebApp.Start<Startup>("http://*:5000");
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            _webApp.Dispose();

            return true;
        }
    }
}