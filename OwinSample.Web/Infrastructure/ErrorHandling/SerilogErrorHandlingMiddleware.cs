using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using Serilog;

namespace OwinSample.Web.Infrastructure.ErrorHandling
{
    public class SerilogErrorHandlingMiddleware : OwinMiddleware
    {
        public SerilogErrorHandlingMiddleware(OwinMiddleware next, IAppBuilder app)
            : base(next)
        {
        }

        public override Task Invoke(IOwinContext context)
        {
            return Next.Invoke(context).ContinueWith(appTask =>
            {
                if (appTask.IsFaulted)
                {
                    Log.Error(appTask.Exception.GetBaseException(), "Owin pipeline faulted {Message}", appTask.Exception.GetBaseException().Message);
                }
                if (appTask.IsCanceled)
                {
                    Log.Warning("Owin pipeline was cancelled.");
                }
                return Task.Yield();
            });
        }
    }
}