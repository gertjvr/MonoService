using System;
using System.Diagnostics;
using Microsoft.AspNet.SignalR.Hubs;
using Serilog;

namespace OwinSample.Web.Infrastructure.SignalR
{
    public class ErrorHandlingPipelineModule : HubPipelineModule
    {
        protected override void OnIncomingError(ExceptionContext exceptionContext, IHubIncomingInvokerContext invokerContext)
        {
            try
            {
                Log.Error(exceptionContext.Error, "Exception invoking {Method} on {Hub} with {Args}", invokerContext.MethodDescriptor.Name, invokerContext.Hub.GetType().Name, invokerContext.Args);
            }
            // ReSharper disable once UnusedVariable
            catch (Exception e)
            {
                if (Debugger.IsAttached) Debugger.Break();
            }
        }
    }
}