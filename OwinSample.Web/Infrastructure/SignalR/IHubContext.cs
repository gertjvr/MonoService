using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace OwinSample.Web.Infrastructure.SignalR
{
    public interface IHubContext<T> : IHubContext
        where T : IHub
    {
    }
}