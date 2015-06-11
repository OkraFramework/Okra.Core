using Okra.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Okra.Tests.Navigation
{
    public class StateChangedEventArgsFixture
    {
        // *** Constructor Tests ***

        [Fact]
        public void Constructor_SetsStateKey()
        {
            StateChangedEventArgs eventArgs = new StateChangedEventArgs("Test Key");

            Assert.Equal("Test Key", eventArgs.StateKey);
        }

        [Fact]
        public void Constructor_ThrowsException_WhenStateKeyIsNull()
        {
            Assert.Throws<ArgumentException>(() => new StateChangedEventArgs(null));
        }
    }
}
