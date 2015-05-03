using System;

namespace OwinSample.Worker.Infrastructure.Clock
{
    public class SystemClock : IClock
    {
        public DateTimeOffset UtcNow
        {
            get { return DateTimeOffset.UtcNow; }
        }
    }
}