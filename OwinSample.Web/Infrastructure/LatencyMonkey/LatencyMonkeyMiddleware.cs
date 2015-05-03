using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OwinSample.Web.Infrastructure.LatencyMonkey
{
    public class LatencyMonkeyMiddleware
    {
        readonly Func<IDictionary<string, object>, Task> next;
        readonly LatencyMonkeyOptions options;

        public LatencyMonkeyMiddleware(Func<IDictionary<string, object>, Task> next, LatencyMonkeyOptions options)
        {
            this.next = next;
            this.options = options;
        }

        public async Task<Task> Invoke(IDictionary<string, object> environment)
        {
            await Task.Delay(options.HowLatentIsThisMonkey());
            return next(environment);
        }
    }
}