using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Okra.Navigation;
using Okra.Services;
using Okra.Tests.Mocks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Windows.ApplicationModel.Activation;

namespace Okra.Tests.Services
{
    [TestClass]
    public class LaunchActivationHandlerFixture
    {
        // *** Constructor Tests ***

        [TestMethod]
        public void Constructor_RegistersWithActivationManager()
        {
            MockActivationManager activationManager = new MockActivationManager();
            LaunchActivationHandler activationHandler = CreateLaunchActivationHandler(activationManager: activationManager);

            CollectionAssert.Contains(activationManager.RegisteredServices, activationHandler);
        }

        // *** Method Tests ***

        [TestMethod]
        public async Task Activate_ReturnsTrueIfActivationKindIsLaunch()
        {
            MockNavigationManager navigationManager = new MockNavigationManager() { CanRestoreNavigationStack = true };
            LaunchActivationHandler activationHandler = CreateLaunchActivationHandler(navigationManager);

            // Activate the application

            bool result = await activationHandler.Activate(new MockLaunchActivatedEventArgs() { PreviousExecutionState = ApplicationExecutionState.Terminated });

            // Check the result

            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public async Task Activate_ReturnsFalseIfActivationKindIsNotLaunch()
        {
            MockNavigationManager navigationManager = new MockNavigationManager() { CanRestoreNavigationStack = true };
            LaunchActivationHandler activationHandler = CreateLaunchActivationHandler(navigationManager);

            // Activate the application

            bool result = await activationHandler.Activate(new MockActivatedEventArgs() { Kind = ActivationKind.Search });

            // Check the result

            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public async Task Activate_RestoresNavigationIfPreviousExecutionTerminated()
        {
            MockNavigationManager navigationManager = new MockNavigationManager() { CanRestoreNavigationStack = true };
            LaunchActivationHandler activationHandler = CreateLaunchActivationHandler(navigationManager);

            // Activate the application

            await activationHandler.Activate(new MockLaunchActivatedEventArgs() { PreviousExecutionState = ApplicationExecutionState.Terminated });

            // Assert that the home page was navigated to

            CollectionAssert.AreEqual(new string[] { "[Restored Pages]" }, navigationManager.NavigatedPages.Select(t =>t.Item1).ToArray());
        }

        [TestMethod]
        public async Task Activate_Launch_NavigatesToHomePageIfPreviousExecutionClosedByUser()
        {
            MockNavigationManager navigationManager = new MockNavigationManager() { CanRestoreNavigationStack = true };
            LaunchActivationHandler activationHandler = CreateLaunchActivationHandler(navigationManager);

            // Activate the application

            await activationHandler.Activate(new MockLaunchActivatedEventArgs() { PreviousExecutionState = ApplicationExecutionState.ClosedByUser });

            // Assert that the home page was navigated to

            CollectionAssert.AreEqual(new string[] { "Home" }, navigationManager.NavigatedPages.Select(t => t.Item1).ToArray());
        }

        [TestMethod]
        public async Task Activate_Launch_NavigatesToHomePageIfPreviousExecutionNotRunning()
        {
            MockNavigationManager navigationManager = new MockNavigationManager() { CanRestoreNavigationStack = true };
            LaunchActivationHandler activationHandler = CreateLaunchActivationHandler(navigationManager);

            // Activate the application

            await activationHandler.Activate(new MockLaunchActivatedEventArgs() { PreviousExecutionState = ApplicationExecutionState.NotRunning });

            // Assert that the home page was navigated to

            CollectionAssert.AreEqual(new string[] { "Home" }, navigationManager.NavigatedPages.Select(t => t.Item1).ToArray());
        }

        [TestMethod]
        public async Task Activate_DoesNotNavigateIfActivationKindIsNotLaunch()
        {
            MockNavigationManager navigationManager = new MockNavigationManager() { CanRestoreNavigationStack = true };
            LaunchActivationHandler activationHandler = CreateLaunchActivationHandler(navigationManager);

            // Activate the application

            await activationHandler.Activate(new MockActivatedEventArgs() { Kind = ActivationKind.Search });

            // Assert that the home page was navigated to

            CollectionAssert.AreEqual(new string[] { }, navigationManager.NavigatedPages.Select(t => t.Item1).ToArray());
        }

        // *** Private Methods ***

        private LaunchActivationHandler CreateLaunchActivationHandler(INavigationManager navigationManager = null, IActivationManager activationManager = null)
        {
            if (navigationManager == null)
                navigationManager = new MockNavigationManager();

            if (activationManager == null)
                activationManager = new MockActivationManager();

            return new LaunchActivationHandler(activationManager, navigationManager);
        }

        // *** Private sub-classes ***

        private class MockLaunchActivatedEventArgs : MockActivatedEventArgs, ILaunchActivatedEventArgs
        {
            // *** Constructors ***

            public MockLaunchActivatedEventArgs()
            {
                base.Kind = ActivationKind.Launch;
                base.PreviousExecutionState = ApplicationExecutionState.Terminated;
            }

            // *** Propertes ***

            public string Arguments
            {
                get { throw new NotImplementedException(); }
            }

            public string TileId
            {
                get { throw new NotImplementedException(); }
            }
        }
    }
}
