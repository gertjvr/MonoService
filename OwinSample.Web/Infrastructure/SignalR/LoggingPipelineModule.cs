using Microsoft.AspNet.SignalR.Hubs;
using Serilog;

namespace OwinSample.Web.Infrastructure.SignalR
{
    public class LoggingPipelineModule : HubPipelineModule
    {
        protected override bool OnBeforeIncoming(IHubIncomingInvokerContext context)
        {
            Log.Debug("SignalR Invoking {Method} on {Hub}", context.MethodDescriptor.Name, context.MethodDescriptor.Hub.Name);
            return base.OnBeforeIncoming(context);
        }

        protected override bool OnBeforeOutgoing(IHubOutgoingInvokerContext context)
        {
            Log.Debug("SignalR Invoking {Method} on client {Hub}", context.Invocation.Method, context.Invocation.Hub);
            return base.OnBeforeOutgoing(context);
        }
    }
}