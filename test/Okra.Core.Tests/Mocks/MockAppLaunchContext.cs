using Okra.Lifetime;
using System;

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
