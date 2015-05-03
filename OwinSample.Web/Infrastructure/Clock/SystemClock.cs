using System;

namespace OwinSample.Web.Infrastructure.Clock
{
    public class SystemClock : IClock
    {
        public DateTimeOffset UtcNow
        {
            get { return DateTimeOffset.UtcNow; }
        }
    }
}