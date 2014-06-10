using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okra.Navigation
{
    public interface IViewLifetimeContext : IDisposable
    {
        // *** Properties ***

        object View { get; }
        object ViewModel { get; }
    }
}
