using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okra.Builder
{
    public interface IOkraAppBuilder
    {
        IServiceProvider ApplicationServices { get; }

        AppLaunchDelegate Build();
        IOkraAppBuilder Use(Func<AppLaunchDelegate, AppLaunchDelegate> middleware);
    }
}
