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

        ActivationDelegate Build();
        IOkraAppBuilder Use(Func<ActivationDelegate, ActivationDelegate> middleware);
    }
}
