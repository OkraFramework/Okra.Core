using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Okra.Navigation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

#if WINDOWS_APP
using Windows.UI.ApplicationSettings;
#endif

namespace Okra.Tests.Navigation
{
    [TestClass]
    public class NavigationCommandsFixture
    {
        // *** Method Tests ***

        [TestMethod]
        public void GetGoBackCommand_ReturnsNewICommand()
        {
            MockNavigationManager navigationManager = new MockNavigationManager();

            ICommand command = navigationManager.GetGoBackCommand();

            Assert.IsNotNull(command);
        }

        [TestMethod]
        public void GetGoBackCommand_CanExecute_IsFalseIfCannotGoBack()
        {
            MockNavigationManager navigationManager = new MockNavigationManager()
                    {
                        CanGoBack = false
                    };

            ICommand command = navigationManager.GetGoBackCommand();

            Assert.AreEqual(false, command.CanExecute(null));
        }

        [TestMethod]
        public void GetGoBackCommand_CanExecute_IsTrueIfCanGoBack()
        {
            MockNavigationManager navigationManager = new MockNavigationManager()
            {
                CanGoBack = true
            };

            ICommand command = navigationManager.GetGoBackCommand();

            Assert.AreEqual(true, command.CanExecute(null));
        }

        [TestMethod]
        public void GoBackCommand_Execute_CallsGoBackIfCanGoBack()
        {
            MockNavigationManager navigationManager = new MockNavigationManager()
            {
                CanGoBack = true
            };

            ICommand command = navigationManager.GetGoBackCommand();
            command.Execute(null);

            CollectionAssert.AreEqual(new string[] { "GoBack()" }, (ICollection)navigationManager.MethodCallLog);
        }

        [TestMethod]
        public void GoBackCommand_Execute_DoesNothingIfCannotGoBack()
        {
            MockNavigationManager navigationManager = new MockNavigationManager()
            {
                CanGoBack = false
            };

            ICommand command = navigationManager.GetGoBackCommand();
            command.Execute(null);

            CollectionAssert.AreEqual(new string[] { }, (ICollection)navigationManager.MethodCallLog);
        }

        [TestMethod]
        public void GetGoBackCommand_CanExecuteChanged_IsCalledWhenCanGoBackChanged()
        {
            MockNavigationManager navigationManager = new MockNavigationManager();

            ICommand command = navigationManager.GetGoBackCommand();

            int canExecuteChangedCount = 0;
            command.CanExecuteChanged += delegate(object sender, EventArgs e) { canExecuteChangedCount++; };

            navigationManager.RaiseCanGoBackChanged();

            Assert.AreEqual(1, canExecuteChangedCount);
        }

        [TestMethod]
        public void NavigateToCommand_ReturnsNewICommand()
        {
            MockNavigationManager navigationManager = new MockNavigationManager();

            ICommand command = navigationManager.GetNavigateToCommand("Page Name", "Arguments");

            Assert.IsNotNull(command);
        }

        [TestMethod]
        public void NavigateToCommand_CanExecute_IsTrue()
        {
            MockNavigationManager navigationManager = new MockNavigationManager();

            ICommand command = navigationManager.GetNavigateToCommand("Page Name", "Arguments");

            Assert.AreEqual(true, command.CanExecute(null));
        }

        [TestMethod]
        public void NavigateToCommand_Execute_CallsNavigateToWithSpecifiedArguments()
        {
            MockNavigationManager navigationManager = new MockNavigationManager()
            {
                CanGoBack = true
            };

            ICommand command = navigationManager.GetNavigateToCommand("PageName", "Arguments");
            command.Execute(null);

            CollectionAssert.AreEqual(new string[] { "NavigateTo(PageName, Arguments)" }, (ICollection)navigationManager.MethodCallLog);
        }

#if WINDOWS_APP
        [TestMethod]
        public void NavigateToSettingsCommand_ReturnsNewSettingsCommand()
        {
            MockNavigationManager navigationManager = new MockNavigationManager();

            SettingsCommand command = navigationManager.GetNavigateToSettingsCommand("MyLabel", "Page Name", "Arguments");

            Assert.IsNotNull(command);
        }

        [TestMethod]
        public void NavigateToSettingsCommand_Label_IsAsSpecified()
        {
            MockNavigationManager navigationManager = new MockNavigationManager();

            SettingsCommand command = navigationManager.GetNavigateToSettingsCommand("MyLabel", "Page Name", "Arguments");

            Assert.AreEqual("MyLabel", command.Label);
        }

        [TestMethod]
        public void NavigateToSettingsCommand_Invoked_CallsNavigateToWithSpecifiedArguments()
        {
            MockNavigationManager navigationManager = new MockNavigationManager()
            {
                CanGoBack = true
            };

            SettingsCommand command = navigationManager.GetNavigateToSettingsCommand("MyLabel", "PageName", "Arguments");
            command.Invoked(command);

            CollectionAssert.AreEqual(new string[] { "NavigateTo(PageName, Arguments)" }, (ICollection)navigationManager.MethodCallLog);
        }
#endif

        // *** Private sub-classes ***

        private class MockNavigationManager : INavigationBase
        {
            // *** Fields ***

            public IList<string> MethodCallLog = new List<string>();

            // *** Events ***

            public event EventHandler CanGoBackChanged;
            public event EventHandler<PageNavigationEventArgs> NavigatingFrom;
            public event EventHandler<PageNavigationEventArgs> NavigatedTo;

            // *** Constructors ***

            public MockNavigationManager()
            {
                this.NavigationStack = new MockNavigationStack(this);
            }

            // *** Properties ***

            public bool CanGoBack
            {
                get;
                set;
            }

            public INavigationStack NavigationStack
            {
                get;
                private set;
            }

            // *** Methods ***

            public bool CanNavigateTo(string pageName)
            {
                return true;
            }

            public IEnumerable<object> GetPageElements(PageInfo page)
            {
                throw new NotImplementedException();
            }

            public void RaiseCanGoBackChanged()
            {
                if (CanGoBackChanged != null)
                    CanGoBackChanged(this, EventArgs.Empty);
            }
        }

        private class MockNavigationStack : List<PageInfo>, INavigationStack
        {
            // *** Fields ***

            private MockNavigationManager navigationManager;

            // *** Events ***

            public event EventHandler<PageNavigationEventArgs> NavigatingFrom;
            public event EventHandler<PageNavigationEventArgs> NavigatedTo;
            public event EventHandler<PageNavigationEventArgs> PageDisposed;
            public event PropertyChangedEventHandler PropertyChanged;

            // *** Constructors ***

            public MockNavigationStack(MockNavigationManager navigationManager)
            {
                this.navigationManager = navigationManager;
            }

            // *** Properties ***

            public bool CanGoBack
            {
                get { return true; }
            }

            public PageInfo CurrentPage
            {
                get { throw new NotImplementedException(); }
            }

            // *** Methods ***

            public void GoBack()
            {
                navigationManager.MethodCallLog.Add("GoBack()");
            }

            public void NavigateTo(PageInfo page)
            {
                navigationManager.MethodCallLog.Add(string.Format("NavigateTo({0}, {1})", page.PageName, page.GetArguments<string>()));
            }

            public void Push(IEnumerable<PageInfo> pages)
            {
                throw new NotImplementedException();
            }
        }
    }
}
