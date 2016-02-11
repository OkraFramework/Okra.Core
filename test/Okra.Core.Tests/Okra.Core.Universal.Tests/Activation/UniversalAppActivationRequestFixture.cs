using Okra.Activation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Xunit;

namespace Okra.Core.Universal.Tests.Activation
{
    public class UniversalAppActivationRequestFixture
    {
        [Fact]
        public void Constructor_SetsEventArgsProperty()
        {
            IActivatedEventArgs args = new MockActivationEventArgs();
            var request = new UniversalAppActivationRequest(args);

            Assert.Equal(args, request.EventArgs);
        }

        // *** Helper Classes ***

        private class MockActivationEventArgs : IActivatedEventArgs
        {
            public ActivationKind Kind
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public ApplicationExecutionState PreviousExecutionState
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public SplashScreen SplashScreen
            {
                get
                {
                    throw new NotImplementedException();
                }
            }
        }
    }
}
