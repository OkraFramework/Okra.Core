using System;

namespace Okra.Builder
{
    public interface IOkraAppBuilder
    {
        IServiceProvider ApplicationServices { get; }

        AppLaunchDelegate Build();
        IOkraAppBuilder Use(Func<AppLaunchDelegate, AppLaunchDelegate> middleware);
    }
}
