using System;

namespace Okra.Lifetime
{
    public abstract class AppLaunchContext
    {
        public abstract IAppLaunchRequest LaunchRequest { get; }
        public abstract IServiceProvider Services { get; set; }
    }
}
