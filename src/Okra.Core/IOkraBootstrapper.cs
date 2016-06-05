using Okra.Lifetime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okra
{
    public interface IOkraBootstrapper
    {
        IServiceProvider ApplicationServices { get; }
        void Initialize();
    }
}
