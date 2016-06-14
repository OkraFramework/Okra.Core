using Okra.Lifetime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okra.Tests.Mocks
{
    public class MockAppLaunchContext : AppLaunchContext
    {
        public override IAppLaunchRequest LaunchRequest
        {
            get;
        }

        public override IServiceProvider Services
        {
            get;
            set;
        }
    }
}
