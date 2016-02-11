using Okra.Activation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okra.Tests.Mocks
{
    public class MockAppActivationContext : AppActivationContext
    {
        public override IAppActivationRequest ActivationRequest
        {
            get;
        }
    }
}
