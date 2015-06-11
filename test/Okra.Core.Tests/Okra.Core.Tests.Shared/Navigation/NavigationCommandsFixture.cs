using Okra.Navigation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xunit;
using Okra.Tests.Helpers;

#if WINDOWS_APP || WINDOWS_UAP
using Windows.UI.ApplicationSettings;
#endif

namespace Okra.Tests.Navigation
{
    public class NavigationCommandsFixture
    {
        // *** Method Tests ***

        [Fact]
        public void GetGoBackCommand_ReturnsNewICommand()
        {
            MockNavigationManager navigationManager = new MockNavigationManager();

            ICommand command = navigationManager.GetGoBackCommand();

            Assert.NotNull(command);
        }

        [Fact]
        public void GetGoBackCommand_CanExecute_IsFalseIfCannotGoBack()
        {
            MockNavigationManager navigationManager = new MockNavigationManager()
            {
                CanGoBack = false
            };

            ICommand command = navigationManager.GetGoBackCommand();

            Assert.Equal(false, command.CanExecute(null));
        }

        [Fact]
        public void GetGoBackCommand_CanExecute_IsTrueIfCanGoBack()
        {
            MockNavigationManager navigationManager = new MockNavigationManager()
            {
                CanGoBack = true
            };

            ICommand command = navigationManager.GetGoBackCommand();

            Assert.Equal(true, command.CanExecute(null));
        }

        [Fact]
        public void GetGoBackCommand_Execute_CallsGoBackIfCanGoBack()
        {
            MockNavigationManager navigationManager = new MockNavigationManager()
            {
                CanGoBack = true
            };

            ICommand command = navigationManager.GetGoBackCommand();
            command.Execute(null);

            Assert.Equal<string>(new string[] { "GoBack()" }, navigationManager.MethodCallLog);
        }

        [Fact]
        public void GetGoBackCommand_Execute_DoesNothingIfCannotGoBack()
        {
            MockNavigationManager navigationManager = new MockNavigationManager()
            {
                CanGoBack = false
            };

            ICommand command = navigationManager.GetGoBackCommand();
            command.Execute(null);

            Assert.Equal<string>(new string[] { }, navigationManager.MethodCallLog);
        }

        [Fact]
        public void GetGoBackCommand_CanExecuteChanged_IsCalledWhenCanGoBackChanged()
        {
            MockNavigationManager navigationManager = new MockNavigationManager();

            ICommand command = navigationManager.GetGoBackCommand();

            int canExecuteChangedCount = 0;
            command.CanExecuteChanged += delegate (object sender, EventArgs e) { canExecuteChangedCount++; };

            navigationManager.RaiseCanGoBackChanged();

            Assert.Equal(1, canExecuteChangedCount);
        }

        [Fact]
        public void GetGoBackCommand_Exception_NavigationBaseIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => NavigationCommands.GetGoBackCommand(null));
        }

        [Fact]
        public void GetNavigateToCommand_ReturnsNewICommand()
        {
            MockNavigationManager navigationManager = new MockNavigationManager();

            ICommand command = navigationManager.GetNavigateToCommand("Page Name", "Arguments");

            Assert.NotNull(command);
        }

        [Fact]
        public void GetNavigateToCommand_CanExecute_IsTrue()
        {
            MockNavigationManager navigationManager = new MockNavigationManager();

            ICommand command = navigationManager.GetNavigateToCommand("Page Name", "Arguments");

            Assert.Equal(true, command.CanExecute(null));
        }

        [Fact]
        public void GetNavigateToCommand_Execute_CallsNavigateToWithSpecifiedArguments()
        {
            MockNavigationManager navigationManager = new MockNavigationManager()
            {
                CanGoBack = true
            };

            ICommand command = navigationManager.GetNavigateToCommand("PageName", "Arguments");
            command.Execute(null);

            Assert.Equal<string>(new string[] { "NavigateTo(PageName, Arguments)" }, navigationManager.MethodCallLog);
        }

        [Fact]
        public void GetNavigateToCommand_Exception_NavigationBaseIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => NavigationCommands.GetNavigateToCommand(null, "Page Name", new object()));
        }

        [Fact]
        public void GetNavigateToCommand_Exception_PageNameIsNull()
        {
            MockNavigationManager navigationManager = new MockNavigationManager();

            Assert.Throws<ArgumentException>(() => NavigationCommands.GetNavigateToCommand(navigationManager, null, new object()));
        }

        [Fact]
        public void GetNavigateToCommand_Exception_PageNameIsEmpty()
        {
            MockNavigationManager navigationManager = new MockNavigationManager();

            Assert.Throws<ArgumentException>(() => NavigationCommands.GetNavigateToCommand(navigationManager, "", new object()));
        }

#if WINDOWS_APP || WINDOWS_UAP
        [UITestMethod]
        public void GetNavigateToSettingsCommand_ReturnsNewSettingsCommand()
        {
            MockNavigationManager navigationManager = new MockNavigationManager();

            SettingsCommand command = navigationManager.GetNavigateToSettingsCommand("MyLabel", "Page Name", "Arguments");

            Assert.NotNull(command);
        }

        [UITestMethod]
        public void GetNavigateToSettingsCommand_Label_IsAsSpecified()
        {
            MockNavigationManager navigationManager = new MockNavigationManager();

            SettingsCommand command = navigationManager.GetNavigateToSettingsCommand("MyLabel", "Page Name", "Arguments");

            Assert.Equal("MyLabel", command.Label);
        }

        [UITestMethod]
        public void GetNavigateToSettingsCommand_Invoked_CallsNavigateToWithSpecifiedArguments()
        {
            MockNavigationManager navigationManager = new MockNavigationManager()
            {
                CanGoBack = true
            };

            SettingsCommand command = navigationManager.GetNavigateToSettingsCommand("MyLabel", "PageName", "Arguments");
            command.Invoked(command);

            Assert.Equal<string>(new string[] { "NavigateTo(PageName, Arguments)" }, navigationManager.MethodCallLog);
        }

        [Fact]
        public void GetNavigateToSettingsCommand_Exception_NavigationBaseIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => NavigationCommands.GetNavigateToSettingsCommand(null, "Label", "Page Name", new object()));
        }

        [Fact]
        public void GetNavigateToSettingsCommand_Exception_PageNameIsNull()
        {
            MockNavigationManager navigationManager = new MockNavigationManager();

            Assert.Throws<ArgumentException>(() => NavigationCommands.GetNavigateToSettingsCommand(navigationManager, "Label", null, new object()));
        }

        [Fact]
        public void GetNavigateToSettingsCommand_Exception_PageNameIsEmpty()
        {
            MockNavigationManager navigationManager = new MockNavigationManager();

            Assert.Throws<ArgumentException>(() => NavigationCommands.GetNavigateToSettingsCommand(navigationManager, "Label", "", new object()));
        }

        [Fact]
        public void GetNavigateToSettingsCommand_Exception_LabelIsNull()
        {
            MockNavigationManager navigationManager = new MockNavigationManager();

            Assert.Throws<ArgumentException>(() => NavigationCommands.GetNavigateToSettingsCommand(navigationManager, null, "Page Name", new object()));
        }

        [Fact]
        public void GetNavigateToSettingsCommand_Exception_LabelIsEmpty()
        {
            MockNavigationManager navigationManager = new MockNavigationManager();

            Assert.Throws<ArgumentException>(() => NavigationCommands.GetNavigateToSettingsCommand(navigationManager, "", "Page Name", new object()));
        }
#endif

        // *** Private sub-classes ***

        private class MockNavigationManager : INavigationBase
        {
            // *** Fields ***

            private MockNavigationStack _navigationStack;

            public IList<string> MethodCallLog = new List<string>();

            // *** Constructors ***

            public MockNavigationManager(MockNavigationStack navigationStack = null)
            {
                if (navigationStack == null)
                    navigationStack = new MockNavigationStack(this);

                _navigationStack = navigationStack;
            }

            // *** Properties ***

            public bool CanGoBack
            {
                get
                {
                    return _navigationStack.CanGoBack;
                }
                set
                {
                    _navigationStack.CanGoBack = value;
                }
            }

            public INavigationStack NavigationStack
            {
                get
                {
                    return _navigationStack;
                }
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

            // *** Mock Methods ***

            public void RaiseCanGoBackChanged()
            {
                MockNavigationStack navigationStack = (MockNavigationStack)this.NavigationStack;
                navigationStack.RaisePropertyChanged(new PropertyChangedEventArgs("CanGoBack"));
            }
        }

        private class MockNavigationStack : List<PageInfo>, INavigationStack
        {
            // *** Fields ***

            private MockNavigationManager _navigationManager;

            // *** Events ***

            public event EventHandler<PageNavigationEventArgs> NavigatingFrom;
            public event EventHandler<PageNavigationEventArgs> NavigatedTo;
            public event EventHandler<PageNavigationEventArgs> PageDisposed;
            public event PropertyChangedEventHandler PropertyChanged;

            // *** Constructors ***

            public MockNavigationStack(MockNavigationManager navigationManager)
            {
                _navigationManager = navigationManager;
            }

            // *** Properties ***

            public bool CanGoBack
            {
                get;
                set;
            }

            public PageInfo CurrentPage
            {
                get { throw new NotImplementedException(); }
            }

            // *** Methods ***

            public void GoBack()
            {
                _navigationManager.MethodCallLog.Add("GoBack()");
            }

            public void GoBackTo(PageInfo page)
            {
                throw new NotImplementedException();
            }

            public void NavigateTo(PageInfo page)
            {
                _navigationManager.MethodCallLog.Add(string.Format("NavigateTo({0}, {1})", page.PageName, page.GetArguments<string>()));
            }

            public void Push(IEnumerable<PageInfo> pages)
            {
                throw new NotImplementedException();
            }

            // *** Mock Methods ***

            public void RaiseNavigatedTo(PageNavigationEventArgs eventArgs)
            {
                if (NavigatedTo != null)
                    NavigatedTo(this, eventArgs);
            }

            public void RaiseNavigatingFrom(PageNavigationEventArgs eventArgs)
            {
                if (NavigatingFrom != null)
                    NavigatingFrom(this, eventArgs);
            }

            public void RaisePageDisposed(PageNavigationEventArgs eventArgs)
            {
                if (PageDisposed != null)
                    PageDisposed(this, eventArgs);
            }

            public void RaisePropertyChanged(PropertyChangedEventArgs eventArgs)
            {
                if (PropertyChanged != null)
                    PropertyChanged(this, eventArgs);
            }
        }
    }
}
