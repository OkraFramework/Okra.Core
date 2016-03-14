using System;
using System.Collections.Generic;
using System.Linq;

namespace Okra.Services
{
    public class LifetimeManager : LifetimeManagerBase, ILifetimeAware
    {
        public System.Threading.Tasks.Task OnResuming()
        {
            return ResumeServicesAsync();
        }

        public System.Threading.Tasks.Task OnSuspending()
        {
            return SuspendServicesAsync();
        }
    }
}
