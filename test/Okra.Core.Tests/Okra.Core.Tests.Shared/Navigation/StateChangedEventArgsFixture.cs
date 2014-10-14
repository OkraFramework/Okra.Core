using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Okra.Navigation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Okra.Tests.Navigation
{
    [TestClass]
    public class StateChangedEventArgsFixture
    {
        // *** Constructor Tests ***

        [TestMethod]
        public void Constructor_SetsStateKey()
        {
            StateChangedEventArgs eventArgs = new StateChangedEventArgs("Test Key");

            Assert.AreEqual("Test Key", eventArgs.StateKey);
        }

        [TestMethod]
        public void Constructor_ThrowsException_WhenStateKeyIsNull()
        {
            Assert.ThrowsException<ArgumentException>(() => new StateChangedEventArgs(null));
        }
    }
}
