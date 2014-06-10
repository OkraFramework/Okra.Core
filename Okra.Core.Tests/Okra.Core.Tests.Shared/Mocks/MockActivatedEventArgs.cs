using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;

namespace Okra.Tests.Mocks
{
    public class MockActivatedEventArgs : IActivatedEventArgs
    {
        // *** Properties ***

        public ActivationKind Kind
        {
            get;
            set;
        }

        public ApplicationExecutionState PreviousExecutionState
        {
            get;
            set;
        }

        public SplashScreen SplashScreen
        {
            get { throw new NotImplementedException(); }
        }
    }
}
