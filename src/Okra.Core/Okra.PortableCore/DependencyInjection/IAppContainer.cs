using Okra.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okra.DependencyInjection
{
    public interface IAppContainer
    {
        IServiceProvider Services { get; }
        IStateService State { get; }

        IAppContainer CreateChildContainer();
    }
}
