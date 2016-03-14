using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okra.Services
{
    public interface ILifetimeAware
    {
        // *** Methods ***

        Task OnResuming();
        Task OnSuspending();
    }
}
