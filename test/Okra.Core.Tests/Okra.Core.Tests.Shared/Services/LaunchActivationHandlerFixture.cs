using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Okra.Navigation;
using Okra.Services;
using Okra.Tests.Mocks;
using Windows.ApplicationModel.Activation;
using Xunit;

namespace Okra.Tests.Services
{
    public class LaunchActivationHandlerFixture
    {
        // *** Constructor Tests ***

        [Fact]
        public void Constructor_RegistersWithActivationManager()
        {
            MockActivationManager activationManager = new MockActivationManager();
            LaunchActivationHandler activationHandler = CreateLaunchActivationHandler(activationManager: activationManager);

            Assert.Contains(activationHandler, activationManager.RegisteredServices);
        }

        [Fact]
        public void Constructor_ThrowsException_IfActivationManagerIsNull()
        {
            var e = Assert.Throws<ArgumentNullException>(() => new LaunchActivationHandler(null, new MockNavigationManager()));

            Assert.Equal("Value cannot be null.\r\nParameter name: activationManager", e.Message);
            Assert.Equal("activationManager", e.ParamName);
        }

        [Fact]
        public void Constructor_ThrowsException_IfNavigationManagerIsNull()
        {
            var e = Assert.Throws<ArgumentNullException>(() => new LaunchActivationHandler(new MockActivationManager(), null));

            Assert.Equal("Value cannot be null.\r\nParameter name: navigationManager", e.Message);
            Assert.Equal("navigationManager", e.ParamName);
        }

        // *** Method Tests ***

        [Fact]
        public async Task Activate_ReturnsTrueIfActivationKindIsLaunch()
        {
            MockNavigationManager navigationManager = new MockNavigationManager() { CanRestoreNavigationStack = true };
            LaunchActivationHandler activationHandler = CreateLaunchActivationHandler(navigationManager);

            // Activate the application

            bool result = await activationHandler.Activate(new MockLaunchActivatedEventArgs() { PreviousExecutionState = ApplicationExecutionState.Terminated });

            // Check the result

            Assert.Equal(true, result);
        }

        [Fact]
        public async Task Activate_ReturnsFalseIfActivationKindIsNotLaunch()
        {
            MockNavigationManager navigationManager = new MockNavigationManager() { CanRestoreNavigationStack = true };
            LaunchActivationHandler activationHandler = CreateLaunchActivationHandler(navigationManager);

            // Activate the application

            bool result = await activationHandler.Activate(new MockActivatedEventArgs() { Kind = ActivationKind.Search });

            // Check the result

            Assert.Equal(false, result);
        }

        [Fact]
        public async Task Activate_RestoresNavigationIfPreviousExecutionTerminated()
        {
            MockNavigationManager navigationManager = new MockNavigationManager() { CanRestoreNavigationStack = true };
            LaunchActivationHandler activationHandler = CreateLaunchActivationHandler(navigationManager);

            // Activate the application

            await activationHandler.Activate(new MockLaunchActivatedEventArgs() { PreviousExecutionState = ApplicationExecutionState.Terminated });

            // Assert that the home page was navigated to

            Assert.Equal(new string[] { "[Restored Pages]" }, navigationManager.NavigatedPages.Select(t => t.Item1).ToArray());
        }

        [Fact]
        public async Task Activate_Launch_NavigatesToHomePageIfPreviousExecutionClosedByUser()
        {
            MockNavigationManager navigationManager = new MockNavigationManager() { CanRestoreNavigationStack = true };
            LaunchActivationHandler activationHandler = CreateLaunchActivationHandler(navigationManager);

            // Activate the application

            await activationHandler.Activate(new MockLaunchActivatedEventArgs() { PreviousExecutionState = ApplicationExecutionState.ClosedByUser });

            // Assert that the home page was navigated to

            Assert.Equal(new string[] { "Home" }, navigationManager.NavigatedPages.Select(t => t.Item1).ToArray());
        }

        [Fact]
        public async Task Activate_Launch_NavigatesToHomePageIfPreviousExecutionNotRunning()
        {
            MockNavigationManager navigationManager = new MockNavigationManager() { CanRestoreNavigationStack = true };
            LaunchActivationHandler activationHandler = CreateLaunchActivationHandler(navigationManager);

            // Activate the application

            await activationHandler.Activate(new MockLaunchActivatedEventArgs() { PreviousExecutionState = ApplicationExecutionState.NotRunning });

            // Assert that the home page was navigated to

            Assert.Equal(new string[] { "Home" }, navigationManager.NavigatedPages.Select(t => t.Item1).ToArray());
        }

        [Fact]
        public async Task Activate_DoesNotNavigateIfActivationKindIsNotLaunch()
        {
            MockNavigationManager navigationManager = new MockNavigationManager() { CanRestoreNavigationStack = true };
            LaunchActivationHandler activationHandler = CreateLaunchActivationHandler(navigationManager);

            // Activate the application

            await activationHandler.Activate(new MockActivatedEventArgs() { Kind = ActivationKind.Search });

            // Assert that the home page was navigated to

            Assert.Equal(new string[] { }, navigationManager.NavigatedPages.Select(t => t.Item1).ToArray());
        }

        [Fact]
        public async void Activate_ThrowsException_IfEventArgsIsNull()
        {
            LaunchActivationHandler activationHandler = CreateLaunchActivationHandler();

            var e = await Assert.ThrowsAsync<ArgumentNullException>(() => activationHandler.Activate(null));

            Assert.Equal("Value cannot be null.\r\nParameter name: activatedEventArgs", e.Message);
            Assert.Equal("activatedEventArgs", e.ParamName);
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
