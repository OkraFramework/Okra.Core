using Okra.Navigation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Linq;
using System.Collections;
using System.Collections.Specialized;
using Windows.UI.Xaml.Navigation;
using Okra.Tests.Mocks;
using Xunit;

namespace Okra.Tests.Navigation
{
    public class NavigationStackFixture
    {
        // *** Property Tests ***

        [Fact]
        public void CanGoBack_IsFalseIfNoPagesNavigated()
        {
            NavigationStack navigationStack = new NavigationStack();

            Assert.Equal(false, navigationStack.CanGoBack);
        }

        [Fact]
        public void CanGoBack_IsTrueIfOnePageNavigated()
        {
            NavigationStack navigationStack = new NavigationStack();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));

            Assert.Equal(true, navigationStack.CanGoBack);
        }

        [Fact]
        public void CanGoBack_IsFalseIfOnePageNavigatedThenBack()
        {
            NavigationStack navigationStack = new NavigationStack();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));
            navigationStack.GoBack();

            Assert.Equal(false, navigationStack.CanGoBack);
        }

        [Fact]
        public void Count_IsInitiallyZero()
        {
            NavigationStack navigationStack = new NavigationStack();

            Assert.Equal(0, navigationStack.Count);
        }

        [Fact]
        public void CurrentPage_IsInitiallyNull()
        {
            NavigationStack navigationStack = new NavigationStack();

            Assert.Equal(null, navigationStack.CurrentPage);
        }

        // *** Method Tests ***

        [Fact]
        public void Clear_TwoPagesNavigatedThenClear_CountIsZero()
        {
            NavigationStack navigationStack = new NavigationStack();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));
            navigationStack.NavigateTo(new PageInfo("Page 2", null));
            navigationStack.Clear();

            Assert.Equal(0, navigationStack.Count);
        }

        [Fact]
        public void Clear_TwoPagesNavigatedThenClear_CurrentPageIsNull()
        {
            NavigationStack navigationStack = new NavigationStack();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));
            navigationStack.NavigateTo(new PageInfo("Page 2", null));
            navigationStack.Clear();

            Assert.Equal(null, navigationStack.CurrentPage);
        }

        [Fact]
        public void GetEnumerator_Generic_EnumeratesPagesInStack()
        {
            NavigationStack navigationStack = new NavigationStack();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));
            navigationStack.NavigateTo(new PageInfo("Page 2", null));

            IEnumerable<PageInfo> pages = (IEnumerable<PageInfo>)navigationStack;
            string[] pageNames = pages.Select(entry => entry.PageName).ToArray();

            Assert.Equal(new string[] { "Page 1", "Page 2" }, pageNames);
        }

        [Fact]
        public void GetEnumerator_NonGeneric_EnumeratesPagesInStack()
        {
            NavigationStack navigationStack = new NavigationStack();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));
            navigationStack.NavigateTo(new PageInfo("Page 2", null));

            List<string> pageNames = new List<string>();

            foreach (PageInfo pageInfo in (IEnumerable)navigationStack)
                pageNames.Add(pageInfo.PageName);

            Assert.Equal(new string[] { "Page 1", "Page 2" }, pageNames);
        }

        [Fact]
        public void GoBack_TwoPagesNavigatedThenBack_CountIsOne()
        {
            NavigationStack navigationStack = new NavigationStack();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));
            navigationStack.NavigateTo(new PageInfo("Page 2", null));
            navigationStack.GoBack();

            Assert.Equal(1, navigationStack.Count);
        }

        [Fact]
        public void GoBack_TwoPagesNavigatedThenBack_CorrectPageRemains()
        {
            NavigationStack navigationStack = new NavigationStack();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));
            navigationStack.NavigateTo(new PageInfo("Page 2", null));
            navigationStack.GoBack();

            Assert.Equal("Page 1", navigationStack[0].PageName);
        }

        [Fact]
        public void GoBack_TwoPagesNavigatedThenBack_CurrentPageIsLastPage()
        {
            NavigationStack navigationStack = new NavigationStack();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));
            navigationStack.NavigateTo(new PageInfo("Page 2", null));
            navigationStack.GoBack();

            Assert.Equal("Page 1", navigationStack.CurrentPage.PageName);
        }

        [Fact]
        public void GoBack_ThrowsException_NoPageInBackStack()
        {
            NavigationStack navigationStack = new NavigationStack();

            var e = Assert.Throws<InvalidOperationException>(() => navigationStack.GoBack());

            Assert.Equal("You cannot navigate backwards as the back stack is empty.", e.Message);
        }

        [Fact]
        public void GoBackTo_NavigatesBackToSpecifiedPage()
        {
            NavigationStack navigationStack = new NavigationStack();
            PageInfo page2 = new PageInfo("Page 2", null);

            navigationStack.NavigateTo(new PageInfo("Page 1", null));
            navigationStack.NavigateTo(page2);
            navigationStack.NavigateTo(new PageInfo("Page 3", null));
            navigationStack.NavigateTo(new PageInfo("Page 4", null));

            navigationStack.GoBackTo(page2);

            Assert.Equal(2, navigationStack.Count);
            Assert.Equal("Page 1", navigationStack[0].PageName);
            Assert.Equal("Page 2", navigationStack[1].PageName);
        }

        [Fact]
        public void GoBackTo_ThrowsException_SpecifiedPageNotInBackStack()
        {
            NavigationStack navigationStack = new NavigationStack();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));
            navigationStack.NavigateTo(new PageInfo("Page 2", null));

            PageInfo unknownPage = new PageInfo("Page 3", null);

            var e = Assert.Throws<InvalidOperationException>(() => navigationStack.GoBackTo(unknownPage));

            Assert.Equal("The specified page 'Page 3' does not exist in the navigation stack.", e.Message);
        }

        [Fact]
        public void GoBackTo_ThrowsException_NullPage()
        {
            NavigationStack navigationStack = new NavigationStack();

            var e = Assert.Throws<ArgumentNullException>(() => navigationStack.GoBackTo(null));

            Assert.Equal("Value cannot be null.\r\nParameter name: page", e.Message);
            Assert.Equal("page", e.ParamName);
        }

        [Fact]
        public void NavigateTo_OnePageNavigated_CountIsOne()
        {
            NavigationStack navigationStack = new NavigationStack();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));

            Assert.Equal(1, navigationStack.Count);
        }

        [Fact]
        public void NavigateTo_TwoPagesNavigated_CountIsTwo()
        {
            NavigationStack navigationStack = new NavigationStack();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));
            navigationStack.NavigateTo(new PageInfo("Page 2", null));

            Assert.Equal(2, navigationStack.Count);
        }

        [Fact]
        public void NavigateTo_OnePageNavigated_AddsSpecifiedPage()
        {
            NavigationStack navigationStack = new NavigationStack();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));

            Assert.Equal("Page 1", navigationStack[0].PageName);
        }

        [Fact]
        public void NavigateTo_TwoPagesNavigated_AddsSpecifiedPages()
        {
            NavigationStack navigationStack = new NavigationStack();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));
            navigationStack.NavigateTo(new PageInfo("Page 2", null));

            Assert.Equal("Page 1", navigationStack[0].PageName);
            Assert.Equal("Page 2", navigationStack[1].PageName);
        }

        [Fact]
        public void NavigateTo_OnePageNavigated_CurrentPageIsLastPage()
        {
            NavigationStack navigationStack = new NavigationStack();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));

            Assert.Equal("Page 1", navigationStack.CurrentPage.PageName);
        }

        [Fact]
        public void NavigateTo_TwoPagesNavigated_CurrentPageIsLastPage()
        {
            NavigationStack navigationStack = new NavigationStack();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));
            navigationStack.NavigateTo(new PageInfo("Page 2", null));

            Assert.Equal("Page 2", navigationStack.CurrentPage.PageName);
        }

        [Fact]
        public void NavigateTo_ThrowsException_NullPage()
        {
            NavigationStack navigationStack = new NavigationStack();

            var e = Assert.Throws<ArgumentNullException>(() => navigationStack.NavigateTo(null));

            Assert.Equal("Value cannot be null.\r\nParameter name: page", e.Message);
            Assert.Equal("page", e.ParamName);
        }

        [Fact]
        public void Push_OntoEmptyStack_ReplacesStack()
        {
            NavigationStack navigationStack = new NavigationStack();

            navigationStack.Push(new[]
                    {
                        new PageInfo("Page 1", null),
                        new PageInfo("Page 2", null)
                    });

            Assert.Equal(2, navigationStack.Count);
        }

        [Fact]
        public void Push_OntoExistingStack_AddsToEndOfStack()
        {
            NavigationStack navigationStack = new NavigationStack();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));
            navigationStack.NavigateTo(new PageInfo("Page 2", null));

            navigationStack.Push(new[]
                    {
                        new PageInfo("Page 1", null),
                        new PageInfo("Page 2", null)
                    });

            Assert.Equal(4, navigationStack.Count);
        }

        [Fact]
        public void Push_EmptyCollection_DoesNotChangeStack()
        {
            NavigationStack navigationStack = new NavigationStack();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));
            navigationStack.NavigateTo(new PageInfo("Page 2", null));

            navigationStack.Push(new PageInfo[]
                    {
                    });

            Assert.Equal(2, navigationStack.Count);
        }

        [Fact]
        public void Push_AddsSpecifiedPages()
        {
            NavigationStack navigationStack = new NavigationStack();

            navigationStack.Push(new[]
                    {
                        new PageInfo("Page 1", null),
                        new PageInfo("Page 2", null)
                    });

            Assert.Equal("Page 1", navigationStack[0].PageName);
            Assert.Equal("Page 2", navigationStack[1].PageName);
        }

        [Fact]
        public void Push_WhenPagesPushed_CurrentPageIsLastPage()
        {
            NavigationStack navigationStack = new NavigationStack();

            navigationStack.Push(new[]
                    {
                        new PageInfo("Page 1", null),
                        new PageInfo("Page 2", null)
                    });

            Assert.Equal("Page 2", navigationStack.CurrentPage.PageName);
        }

        [Fact]
        public void Push_WhenEmptyPagesPushed_CurrentPageIsLastPage()
        {
            NavigationStack navigationStack = new NavigationStack();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));
            navigationStack.NavigateTo(new PageInfo("Page 2", null));

            navigationStack.Push(new PageInfo[]
                    {
                    });

            Assert.Equal("Page 2", navigationStack.CurrentPage.PageName);
        }

        [Fact]
        public void Push_ThrowsException_NullPages()
        {
            NavigationStack navigationStack = new NavigationStack();

            var e = Assert.Throws<ArgumentNullException>(() =>
            {
                navigationStack.Push(null);
            });

            Assert.Equal("Value cannot be null.\r\nParameter name: pages", e.Message);
            Assert.Equal("pages", e.ParamName);
        }

        [Fact]
        public void Push_ThrowsException_NullPageInEnumerable()
        {
            NavigationStack navigationStack = new NavigationStack();
            PageInfo page1 = new PageInfo("Page 1", null);
            PageInfo page3 = new PageInfo("Page 1", null);

            var e = Assert.Throws<ArgumentException>(() =>
            {
                navigationStack.Push(new PageInfo[] { page1, null, page3 });
            });

            Assert.Equal("The list of pages cannot contain a 'null' page.\r\nParameter name: pages", e.Message);
            Assert.Equal("pages", e.ParamName);
        }

        // *** Behavior Tests ***

        [Fact]
        public void CollectionChanged_IsCalledWhenFirstPageNavigated()
        {
            NavigationStack navigationStack = new NavigationStack();

            List<NotifyCollectionChangedEventArgs> changeEvents = new List<NotifyCollectionChangedEventArgs>();
            navigationStack.CollectionChanged += delegate (object sender, NotifyCollectionChangedEventArgs e) { changeEvents.Add(e); };

            navigationStack.NavigateTo(new PageInfo("Page 1", null));

            Assert.Equal(1, changeEvents.Count);
            NotifyCollectionChangedEventArgs changeEvent = changeEvents[0];
            Assert.Equal(NotifyCollectionChangedAction.Add, changeEvent.Action);
            Assert.Equal(1, changeEvent.NewItems.Count);
            Assert.Equal("Page 1", ((PageInfo)changeEvent.NewItems[0]).PageName);
            Assert.Equal(0, changeEvent.NewStartingIndex);
        }

        [Fact]
        public void CollectionChanged_IsCalledWhenSecondPageNavigated()
        {
            NavigationStack navigationStack = new NavigationStack();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));

            List<NotifyCollectionChangedEventArgs> changeEvents = new List<NotifyCollectionChangedEventArgs>();
            navigationStack.CollectionChanged += delegate (object sender, NotifyCollectionChangedEventArgs e) { changeEvents.Add(e); };

            navigationStack.NavigateTo(new PageInfo("Page 2", null));

            Assert.Equal(1, changeEvents.Count);
            NotifyCollectionChangedEventArgs changeEvent = changeEvents[0];
            Assert.Equal(NotifyCollectionChangedAction.Add, changeEvent.Action);
            Assert.Equal(1, changeEvent.NewItems.Count);
            Assert.Equal("Page 2", ((PageInfo)changeEvent.NewItems[0]).PageName);
            Assert.Equal(1, changeEvent.NewStartingIndex);
        }

        [Fact]
        public void CollectionChanged_IsCalledWhenSecondPageNavigatedThenBack()
        {
            NavigationStack navigationStack = new NavigationStack();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));
            navigationStack.NavigateTo(new PageInfo("Page 2", null));

            List<NotifyCollectionChangedEventArgs> changeEvents = new List<NotifyCollectionChangedEventArgs>();
            navigationStack.CollectionChanged += delegate (object sender, NotifyCollectionChangedEventArgs e) { changeEvents.Add(e); };

            navigationStack.GoBack();

            Assert.Equal(1, changeEvents.Count);
            NotifyCollectionChangedEventArgs changeEvent = changeEvents[0];
            Assert.Equal(NotifyCollectionChangedAction.Remove, changeEvent.Action);
            Assert.Equal(1, changeEvent.OldItems.Count);
            Assert.Equal("Page 2", ((PageInfo)changeEvent.OldItems[0]).PageName);
            Assert.Equal(1, changeEvent.OldStartingIndex);
        }

        [Fact]
        public void CollectionChanged_IsCalledWhenNavigatingBackTo_MultiplePages()
        {
            NavigationStack navigationStack = new NavigationStack();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));
            navigationStack.NavigateTo(new PageInfo("Page 2", null));
            navigationStack.NavigateTo(new PageInfo("Page 3", null));

            List<NotifyCollectionChangedEventArgs> changeEvents = new List<NotifyCollectionChangedEventArgs>();
            navigationStack.CollectionChanged += delegate (object sender, NotifyCollectionChangedEventArgs e) { changeEvents.Add(e); };

            navigationStack.GoBackTo(navigationStack[0]);

            Assert.Equal(1, changeEvents.Count);
            NotifyCollectionChangedEventArgs changeEvent = changeEvents[0];
            Assert.Equal(NotifyCollectionChangedAction.Reset, changeEvent.Action);
        }

        [Fact]
        public void CollectionChanged_IsCalledWhenNavigatingBackTo_OnPage()
        {
            NavigationStack navigationStack = new NavigationStack();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));
            navigationStack.NavigateTo(new PageInfo("Page 2", null));
            navigationStack.NavigateTo(new PageInfo("Page 3", null));

            List<NotifyCollectionChangedEventArgs> changeEvents = new List<NotifyCollectionChangedEventArgs>();
            navigationStack.CollectionChanged += delegate (object sender, NotifyCollectionChangedEventArgs e) { changeEvents.Add(e); };

            navigationStack.GoBackTo(navigationStack[1]);

            Assert.Equal(1, changeEvents.Count);
            NotifyCollectionChangedEventArgs changeEvent = changeEvents[0];
            Assert.Equal(NotifyCollectionChangedAction.Remove, changeEvent.Action);
            Assert.Equal(1, changeEvent.OldItems.Count);
            Assert.Equal("Page 3", ((PageInfo)changeEvent.OldItems[0]).PageName);
            Assert.Equal(2, changeEvent.OldStartingIndex);
        }

        [Fact]
        public void CollectionChanged_IsCalledWhenNavigationStackIsCleared()
        {
            NavigationStack navigationStack = new NavigationStack();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));
            navigationStack.NavigateTo(new PageInfo("Page 2", null));

            List<NotifyCollectionChangedEventArgs> changeEvents = new List<NotifyCollectionChangedEventArgs>();
            navigationStack.CollectionChanged += delegate (object sender, NotifyCollectionChangedEventArgs e) { changeEvents.Add(e); };

            navigationStack.Clear();

            Assert.Equal(1, changeEvents.Count);
            NotifyCollectionChangedEventArgs changeEvent = changeEvents[0];
            Assert.Equal(NotifyCollectionChangedAction.Reset, changeEvent.Action);
        }

        [Fact]
        public void CollectionChanged_IsCalledWhenPagesArePushedOntoStack()
        {
            NavigationStack navigationStack = new NavigationStack();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));
            navigationStack.NavigateTo(new PageInfo("Page 2", null));

            List<NotifyCollectionChangedEventArgs> changeEvents = new List<NotifyCollectionChangedEventArgs>();
            navigationStack.CollectionChanged += delegate (object sender, NotifyCollectionChangedEventArgs e) { changeEvents.Add(e); };

            navigationStack.Push(new[]
                    {
                        new PageInfo("Page 3", null),
                        new PageInfo("Page 4", null)
                    });

            Assert.Equal(1, changeEvents.Count);
            NotifyCollectionChangedEventArgs changeEvent = changeEvents[0];
            Assert.Equal(NotifyCollectionChangedAction.Add, changeEvent.Action);
            Assert.Equal(2, changeEvent.NewItems.Count);
            Assert.Equal("Page 3", ((PageInfo)changeEvent.NewItems[0]).PageName);
            Assert.Equal("Page 4", ((PageInfo)changeEvent.NewItems[1]).PageName);
            Assert.Equal(2, changeEvent.NewStartingIndex);
        }

        [Fact]
        public void CollectionChanged_IsNotCalledWhenNoPagesArePushedOntoStack()
        {
            NavigationStack navigationStack = new NavigationStack();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));
            navigationStack.NavigateTo(new PageInfo("Page 2", null));

            List<NotifyCollectionChangedEventArgs> changeEvents = new List<NotifyCollectionChangedEventArgs>();
            navigationStack.CollectionChanged += delegate (object sender, NotifyCollectionChangedEventArgs e) { changeEvents.Add(e); };

            navigationStack.Push(new PageInfo[]
                    {
                    });

            Assert.Equal(0, changeEvents.Count);
        }

        [Fact]
        public void NavigatedTo_IsCalledWhenFirstPageNavigated()
        {
            NavigationStack navigationStack = new NavigationStack();

            List<PageNavigationEventArgs> navigationEvents = new List<PageNavigationEventArgs>();
            navigationStack.NavigatedTo += delegate (object sender, PageNavigationEventArgs e) { navigationEvents.Add(e); };

            navigationStack.NavigateTo(new PageInfo("Page 1", null));

            Assert.Equal(1, navigationEvents.Count);
            PageNavigationEventArgs navigationEvent = navigationEvents[0];
            Assert.Equal(PageNavigationMode.New, navigationEvent.NavigationMode);
            Assert.Equal("Page 1", navigationEvent.Page.PageName);
        }

        [Fact]
        public void NavigatedTo_IsCalledWhenSecondPageNavigated()
        {
            NavigationStack navigationStack = new NavigationStack();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));

            List<PageNavigationEventArgs> navigationEvents = new List<PageNavigationEventArgs>();
            navigationStack.NavigatedTo += delegate (object sender, PageNavigationEventArgs e) { navigationEvents.Add(e); };

            navigationStack.NavigateTo(new PageInfo("Page 2", null));

            Assert.Equal(1, navigationEvents.Count);
            PageNavigationEventArgs navigationEvent = navigationEvents[0];
            Assert.Equal(PageNavigationMode.New, navigationEvent.NavigationMode);
            Assert.Equal("Page 2", navigationEvent.Page.PageName);
        }

        [Fact]
        public void NavigatedTo_IsNotCalledWhenFirstPageNavigatedThenBack()
        {
            NavigationStack navigationStack = new NavigationStack();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));

            List<PageNavigationEventArgs> navigationEvents = new List<PageNavigationEventArgs>();
            navigationStack.NavigatedTo += delegate (object sender, PageNavigationEventArgs e) { navigationEvents.Add(e); };

            navigationStack.GoBack();

            Assert.Equal(0, navigationEvents.Count);
        }

        [Fact]
        public void NavigatedTo_IsCalledWhenSecondPageNavigatedThenBack()
        {
            NavigationStack navigationStack = new NavigationStack();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));
            navigationStack.NavigateTo(new PageInfo("Page 2", null));

            List<PageNavigationEventArgs> navigationEvents = new List<PageNavigationEventArgs>();
            navigationStack.NavigatedTo += delegate (object sender, PageNavigationEventArgs e) { navigationEvents.Add(e); };

            navigationStack.GoBack();

            Assert.Equal(1, navigationEvents.Count);
            PageNavigationEventArgs navigationEvent = navigationEvents[0];
            Assert.Equal(PageNavigationMode.Back, navigationEvent.NavigationMode);
            Assert.Equal("Page 1", navigationEvent.Page.PageName);
        }

        [Fact]
        public void NavigatedTo_IsCalledWhenNavigatingBackToAPage()
        {
            NavigationStack navigationStack = new NavigationStack();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));
            navigationStack.NavigateTo(new PageInfo("Page 2", null));
            navigationStack.NavigateTo(new PageInfo("Page 3", null));

            List<PageNavigationEventArgs> navigationEvents = new List<PageNavigationEventArgs>();
            navigationStack.NavigatedTo += delegate (object sender, PageNavigationEventArgs e) { navigationEvents.Add(e); };

            navigationStack.GoBackTo(navigationStack[0]);

            Assert.Equal(1, navigationEvents.Count);
            PageNavigationEventArgs navigationEvent = navigationEvents[0];
            Assert.Equal(PageNavigationMode.Back, navigationEvent.NavigationMode);
            Assert.Equal("Page 1", navigationEvent.Page.PageName);
        }

        [Fact]
        public void NavigatedTo_IsCalledWhenPagesArePushedOntoStack()
        {
            NavigationStack navigationStack = new NavigationStack();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));
            navigationStack.NavigateTo(new PageInfo("Page 2", null));

            List<PageNavigationEventArgs> navigationEvents = new List<PageNavigationEventArgs>();
            navigationStack.NavigatedTo += delegate (object sender, PageNavigationEventArgs e) { navigationEvents.Add(e); };

            navigationStack.Push(new[]
                    {
                        new PageInfo("Page 3", null),
                        new PageInfo("Page 4", null)
                    });

            Assert.Equal(1, navigationEvents.Count);
            PageNavigationEventArgs navigationEvent = navigationEvents[0];
            Assert.Equal(PageNavigationMode.New, navigationEvent.NavigationMode);
            Assert.Equal("Page 4", navigationEvent.Page.PageName);
        }

        [Fact]
        public void NavigatedTo_IsNotCalledWhenNoPagesArePushedOntoStack()
        {
            NavigationStack navigationStack = new NavigationStack();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));
            navigationStack.NavigateTo(new PageInfo("Page 2", null));

            List<PageNavigationEventArgs> navigationEvents = new List<PageNavigationEventArgs>();
            navigationStack.NavigatedTo += delegate (object sender, PageNavigationEventArgs e) { navigationEvents.Add(e); };

            navigationStack.Push(new PageInfo[]
                    {
                    });

            Assert.Equal(0, navigationEvents.Count);
        }

        [Fact]
        public void NavigatingFrom_IsNotCalledWhenFirstPageNavigated()
        {
            NavigationStack navigationStack = new NavigationStack();

            List<PageNavigationEventArgs> navigationEvents = new List<PageNavigationEventArgs>();
            navigationStack.NavigatingFrom += delegate (object sender, PageNavigationEventArgs e) { navigationEvents.Add(e); };

            navigationStack.NavigateTo(new PageInfo("Page 1", null));

            Assert.Equal(0, navigationEvents.Count);
        }

        [Fact]
        public void NavigatingFrom_IsCalledWhenSecondPageNavigated()
        {
            NavigationStack navigationStack = new NavigationStack();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));

            List<PageNavigationEventArgs> navigationEvents = new List<PageNavigationEventArgs>();
            navigationStack.NavigatingFrom += delegate (object sender, PageNavigationEventArgs e) { navigationEvents.Add(e); };

            navigationStack.NavigateTo(new PageInfo("Page 2", null));

            Assert.Equal(1, navigationEvents.Count);
            PageNavigationEventArgs navigationEvent = navigationEvents[0];
            Assert.Equal(PageNavigationMode.New, navigationEvent.NavigationMode);
            Assert.Equal("Page 1", navigationEvent.Page.PageName);
        }

        [Fact]
        public void NavigatingFrom_IsCalledWhenSecondPageNavigatedThenBack()
        {
            NavigationStack navigationStack = new NavigationStack();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));
            navigationStack.NavigateTo(new PageInfo("Page 2", null));

            List<PageNavigationEventArgs> navigationEvents = new List<PageNavigationEventArgs>();
            navigationStack.NavigatingFrom += delegate (object sender, PageNavigationEventArgs e) { navigationEvents.Add(e); };

            navigationStack.GoBack();

            Assert.Equal(1, navigationEvents.Count);
            PageNavigationEventArgs navigationEvent = navigationEvents[0];
            Assert.Equal(PageNavigationMode.Back, navigationEvent.NavigationMode);
            Assert.Equal("Page 2", navigationEvent.Page.PageName);
        }

        [Fact]
        public void NavigatingFrom_IsCalledWhenNavigationStackIsCleared()
        {
            NavigationStack navigationStack = new NavigationStack();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));
            navigationStack.NavigateTo(new PageInfo("Page 2", null));

            List<PageNavigationEventArgs> navigationEvents = new List<PageNavigationEventArgs>();
            navigationStack.NavigatingFrom += delegate (object sender, PageNavigationEventArgs e) { navigationEvents.Add(e); };

            navigationStack.Clear();

            Assert.Equal(1, navigationEvents.Count);
            PageNavigationEventArgs navigationEvent = navigationEvents[0];
            Assert.Equal(PageNavigationMode.Back, navigationEvent.NavigationMode);
            Assert.Equal("Page 2", navigationEvent.Page.PageName);
        }

        [Fact]
        public void NavigatingFrom_IsCalledWhenNavigatingBackToAPage()
        {
            NavigationStack navigationStack = new NavigationStack();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));
            navigationStack.NavigateTo(new PageInfo("Page 2", null));
            navigationStack.NavigateTo(new PageInfo("Page 3", null));

            List<PageNavigationEventArgs> navigationEvents = new List<PageNavigationEventArgs>();
            navigationStack.NavigatingFrom += delegate (object sender, PageNavigationEventArgs e) { navigationEvents.Add(e); };

            navigationStack.GoBackTo(navigationStack[0]);

            Assert.Equal(1, navigationEvents.Count);
            PageNavigationEventArgs navigationEvent = navigationEvents[0];
            Assert.Equal(PageNavigationMode.Back, navigationEvent.NavigationMode);
            Assert.Equal("Page 3", navigationEvent.Page.PageName);
        }

        [Fact]
        public void NavigatingFrom_IsNotCalledWhenNoPagesArePushedOntoTheStack()
        {
            NavigationStack navigationStack = new NavigationStack();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));
            navigationStack.NavigateTo(new PageInfo("Page 2", null));

            List<PageNavigationEventArgs> navigationEvents = new List<PageNavigationEventArgs>();
            navigationStack.NavigatingFrom += delegate (object sender, PageNavigationEventArgs e) { navigationEvents.Add(e); };

            navigationStack.Push(new PageInfo[]
                    {
                    });

            Assert.Equal(0, navigationEvents.Count);
        }

        [Fact]
        public void PageDisposed_IsCalledWhenGoingBack()
        {
            NavigationStack navigationStack = new NavigationStack();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));
            navigationStack.NavigateTo(new PageInfo("Page 2", null));

            List<PageNavigationEventArgs> pageDisposedEvents = new List<PageNavigationEventArgs>();
            navigationStack.PageDisposed += delegate (object sender, PageNavigationEventArgs e) { pageDisposedEvents.Add(e); };

            navigationStack.GoBack();

            Assert.Equal(1, pageDisposedEvents.Count);
            PageNavigationEventArgs pageDisposedEvent = pageDisposedEvents[0];
            Assert.Equal(PageNavigationMode.Back, pageDisposedEvent.NavigationMode);
            Assert.Equal("Page 2", pageDisposedEvent.Page.PageName);
        }

        [Fact]
        public void PageDisposed_IsCalledWhenNavigatingBackMultiplePages()
        {
            NavigationStack navigationStack = new NavigationStack();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));
            navigationStack.NavigateTo(new PageInfo("Page 2", null));
            navigationStack.NavigateTo(new PageInfo("Page 3", null));

            List<PageNavigationEventArgs> pageDisposedEvents = new List<PageNavigationEventArgs>();
            navigationStack.PageDisposed += delegate (object sender, PageNavigationEventArgs e) { pageDisposedEvents.Add(e); };

            navigationStack.GoBackTo(navigationStack[0]);

            Assert.Equal(2, pageDisposedEvents.Count);
            PageNavigationEventArgs pageDisposedEventPage2 = pageDisposedEvents.First(e => e.Page.PageName == "Page 2");
            PageNavigationEventArgs pageDisposedEventPage3 = pageDisposedEvents.First(e => e.Page.PageName == "Page 3");
            Assert.Equal(PageNavigationMode.Back, pageDisposedEventPage2.NavigationMode);
            Assert.Equal("Page 2", pageDisposedEventPage2.Page.PageName);
            Assert.Equal(PageNavigationMode.Back, pageDisposedEventPage3.NavigationMode);
            Assert.Equal("Page 3", pageDisposedEventPage3.Page.PageName);
        }

        [Fact]
        public void PageDisposed_IsNotCalledWhenNavigatingToANewPage()
        {
            NavigationStack navigationStack = new NavigationStack();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));

            List<PageNavigationEventArgs> pageDisposedEvents = new List<PageNavigationEventArgs>();
            navigationStack.PageDisposed += delegate (object sender, PageNavigationEventArgs e) { pageDisposedEvents.Add(e); };

            navigationStack.NavigateTo(new PageInfo("Page 2", null));

            Assert.Equal(0, pageDisposedEvents.Count);
        }

        [Fact]
        public void PropertyChanged_CanGoBack_IsCalledWhenFirstPageNavigated()
        {
            NavigationStack navigationStack = new NavigationStack();

            int changedCount = 0;
            navigationStack.PropertyChanged += delegate (object sender, PropertyChangedEventArgs e) { if (e.PropertyName == "CanGoBack") changedCount++; };

            navigationStack.NavigateTo(new PageInfo("Page 1", null));

            Assert.Equal(1, changedCount);
        }

        [Fact]
        public void PropertyChanged_CanGoBack_IsNotCalledWhenSecondPageNavigated()
        {
            NavigationStack navigationStack = new NavigationStack();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));

            int changedCount = 0;
            navigationStack.PropertyChanged += delegate (object sender, PropertyChangedEventArgs e) { if (e.PropertyName == "CanGoBack") changedCount++; };

            navigationStack.NavigateTo(new PageInfo("Page 2", null));

            Assert.Equal(0, changedCount);
        }

        [Fact]
        public void PropertyChanged_CanGoBack_IsNotCalledWhenSecondPageNavigatedThenBack()
        {
            NavigationStack navigationStack = new NavigationStack();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));
            navigationStack.NavigateTo(new PageInfo("Page 2", null));

            int changedCount = 0;
            navigationStack.PropertyChanged += delegate (object sender, PropertyChangedEventArgs e) { if (e.PropertyName == "CanGoBack") changedCount++; };

            navigationStack.GoBack();

            Assert.Equal(0, changedCount);
        }

        [Fact]
        public void PropertyChanged_CanGoBack_IsCalledWhenFirstPageNavigatedThenBack()
        {
            NavigationStack navigationStack = new NavigationStack();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));

            int changedCount = 0;
            navigationStack.PropertyChanged += delegate (object sender, PropertyChangedEventArgs e) { if (e.PropertyName == "CanGoBack") changedCount++; };

            navigationStack.GoBack();

            Assert.Equal(1, changedCount);
        }

        [Fact]
        public void PropertyChanged_CanGoBack_IsNotCalledWheNavigatingBackToFirstPage()
        {
            NavigationStack navigationStack = new NavigationStack();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));
            navigationStack.NavigateTo(new PageInfo("Page 2", null));
            navigationStack.NavigateTo(new PageInfo("Page 3", null));

            int changedCount = 0;
            navigationStack.PropertyChanged += delegate (object sender, PropertyChangedEventArgs e) { if (e.PropertyName == "CanGoBack") changedCount++; };

            navigationStack.GoBackTo(navigationStack[0]);

            Assert.Equal(0, changedCount);
        }

        [Fact]
        public void PropertyChanged_CanGoBack_IsCalledWhenNavigationStackIsCleared()
        {
            NavigationStack navigationStack = new NavigationStack();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));

            int changedCount = 0;
            navigationStack.PropertyChanged += delegate (object sender, PropertyChangedEventArgs e) { if (e.PropertyName == "CanGoBack") changedCount++; };

            navigationStack.Clear();

            Assert.Equal(1, changedCount);
        }

        [Fact]
        public void PropertyChanged_CanGoBack_IsNotCalledWhenEmptyNavigationStackIsCleared()
        {
            NavigationStack navigationStack = new NavigationStack();

            int changedCount = 0;
            navigationStack.PropertyChanged += delegate (object sender, PropertyChangedEventArgs e) { if (e.PropertyName == "CanGoBack") changedCount++; };

            navigationStack.Clear();

            Assert.Equal(0, changedCount);
        }

        [Fact]
        public void PropertyChanged_CanGoBack_IsCalledWhenItemsArePushedOntoEmptyStack()
        {
            NavigationStack navigationStack = new NavigationStack();

            int changedCount = 0;
            navigationStack.PropertyChanged += delegate (object sender, PropertyChangedEventArgs e) { if (e.PropertyName == "CanGoBack") changedCount++; };

            navigationStack.Push(new[]
                    {
                        new PageInfo("Page 3", null),
                        new PageInfo("Page 4", null)
                    });

            Assert.Equal(1, changedCount);
        }

        [Fact]
        public void PropertyChanged_CanGoBack_IsNotCalledWhenItemsArePushedOntoExistingStack()
        {
            NavigationStack navigationStack = new NavigationStack();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));

            int changedCount = 0;
            navigationStack.PropertyChanged += delegate (object sender, PropertyChangedEventArgs e) { if (e.PropertyName == "CanGoBack") changedCount++; };

            navigationStack.Push(new[]
                    {
                        new PageInfo("Page 3", null),
                        new PageInfo("Page 4", null)
                    });

            Assert.Equal(0, changedCount);
        }

        [Fact]
        public void PropertyChanged_CanGoBack_IsNotCalledWhenNoPagesArePushedOntoEmptyStack()
        {
            NavigationStack navigationStack = new NavigationStack();

            int changedCount = 0;
            navigationStack.PropertyChanged += delegate (object sender, PropertyChangedEventArgs e) { if (e.PropertyName == "CanGoBack") changedCount++; };

            navigationStack.Push(new PageInfo[]
                    {
                    });

            Assert.Equal(0, changedCount);
        }

        [Fact]
        public void PropertyChanged_Count_IsCalledWhenFirstPageNavigated()
        {
            NavigationStack navigationStack = new NavigationStack();

            int changedCount = 0;
            navigationStack.PropertyChanged += delegate (object sender, PropertyChangedEventArgs e) { if (e.PropertyName == "Count") changedCount++; };

            navigationStack.NavigateTo(new PageInfo("Page 1", null));

            Assert.Equal(1, changedCount);
        }

        [Fact]
        public void PropertyChanged_Count_IsCalledWhenSecondPageNavigated()
        {
            NavigationStack navigationStack = new NavigationStack();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));

            int changedCount = 0;
            navigationStack.PropertyChanged += delegate (object sender, PropertyChangedEventArgs e) { if (e.PropertyName == "Count") changedCount++; };

            navigationStack.NavigateTo(new PageInfo("Page 2", null));

            Assert.Equal(1, changedCount);
        }

        [Fact]
        public void PropertyChanged_Count_IsCalledWhenSecondPageNavigatedThenBack()
        {
            NavigationStack navigationStack = new NavigationStack();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));
            navigationStack.NavigateTo(new PageInfo("Page 2", null));

            int changedCount = 0;
            navigationStack.PropertyChanged += delegate (object sender, PropertyChangedEventArgs e) { if (e.PropertyName == "Count") changedCount++; };

            navigationStack.GoBack();

            Assert.Equal(1, changedCount);
        }

        [Fact]
        public void PropertyChanged_Count_IsCalledWhenWhenNavigatingBackTo()
        {
            NavigationStack navigationStack = new NavigationStack();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));
            navigationStack.NavigateTo(new PageInfo("Page 2", null));
            navigationStack.NavigateTo(new PageInfo("Page 3", null));

            int changedCount = 0;
            navigationStack.PropertyChanged += delegate (object sender, PropertyChangedEventArgs e) { if (e.PropertyName == "Count") changedCount++; };

            navigationStack.GoBackTo(navigationStack[0]);

            Assert.Equal(1, changedCount);
        }

        [Fact]
        public void PropertyChanged_Count_IsCalledWhenNavigationStackIsCleared()
        {
            NavigationStack navigationStack = new NavigationStack();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));
            navigationStack.NavigateTo(new PageInfo("Page 2", null));

            int changedCount = 0;
            navigationStack.PropertyChanged += delegate (object sender, PropertyChangedEventArgs e) { if (e.PropertyName == "Count") changedCount++; };

            navigationStack.Clear();

            Assert.Equal(1, changedCount);
        }

        [Fact]
        public void PropertyChanged_Count_IsNotCalledWhenEmptyNavigationStackIsCleared()
        {
            NavigationStack navigationStack = new NavigationStack();

            int changedCount = 0;
            navigationStack.PropertyChanged += delegate (object sender, PropertyChangedEventArgs e) { if (e.PropertyName == "Count") changedCount++; };

            navigationStack.Clear();

            Assert.Equal(0, changedCount);
        }

        [Fact]
        public void PropertyChanged_Count_IsCalledWhenItemsArePushedOntoStack()
        {
            NavigationStack navigationStack = new NavigationStack();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));
            navigationStack.NavigateTo(new PageInfo("Page 2", null));

            int changedCount = 0;
            navigationStack.PropertyChanged += delegate (object sender, PropertyChangedEventArgs e) { if (e.PropertyName == "Count") changedCount++; };

            navigationStack.Push(new[]
                    {
                        new PageInfo("Page 3", null),
                        new PageInfo("Page 4", null)
                    });

            Assert.Equal(1, changedCount);
        }

        [Fact]
        public void PropertyChanged_Count_IsNotCalledWhenNoPagesArePushedOntoStack()
        {
            NavigationStack navigationStack = new NavigationStack();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));
            navigationStack.NavigateTo(new PageInfo("Page 2", null));

            int changedCount = 0;
            navigationStack.PropertyChanged += delegate (object sender, PropertyChangedEventArgs e) { if (e.PropertyName == "Count") changedCount++; };

            navigationStack.Push(new PageInfo[]
                    {
                    });

            Assert.Equal(0, changedCount);
        }

        [Fact]
        public void PropertyChanged_CurrentPage_IsCalledWhenFirstPageNavigated()
        {
            NavigationStack navigationStack = new NavigationStack();

            int changedCount = 0;
            navigationStack.PropertyChanged += delegate (object sender, PropertyChangedEventArgs e) { if (e.PropertyName == "CurrentPage") changedCount++; };

            navigationStack.NavigateTo(new PageInfo("Page 1", null));

            Assert.Equal(1, changedCount);
        }

        [Fact]
        public void PropertyChanged_CurrentPage_IsCalledWhenSecondPageNavigated()
        {
            NavigationStack navigationStack = new NavigationStack();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));

            int changedCount = 0;
            navigationStack.PropertyChanged += delegate (object sender, PropertyChangedEventArgs e) { if (e.PropertyName == "CurrentPage") changedCount++; };

            navigationStack.NavigateTo(new PageInfo("Page 2", null));

            Assert.Equal(1, changedCount);
        }

        [Fact]
        public void PropertyChanged_CurrentPage_IsCalledWhenSecondPageNavigatedThenBack()
        {
            NavigationStack navigationStack = new NavigationStack();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));
            navigationStack.NavigateTo(new PageInfo("Page 2", null));

            int changedCount = 0;
            navigationStack.PropertyChanged += delegate (object sender, PropertyChangedEventArgs e) { if (e.PropertyName == "CurrentPage") changedCount++; };

            navigationStack.GoBack();

            Assert.Equal(1, changedCount);
        }

        [Fact]
        public void PropertyChanged_CurrentPage_IsCalledWhenNavigatingBackTo()
        {
            NavigationStack navigationStack = new NavigationStack();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));
            navigationStack.NavigateTo(new PageInfo("Page 2", null));
            navigationStack.NavigateTo(new PageInfo("Page 3", null));

            int changedCount = 0;
            navigationStack.PropertyChanged += delegate (object sender, PropertyChangedEventArgs e) { if (e.PropertyName == "CurrentPage") changedCount++; };

            navigationStack.GoBackTo(navigationStack[0]);

            Assert.Equal(1, changedCount);
        }

        [Fact]
        public void PropertyChanged_CurrentPage_IsCalledWhenNavigationStackIsCleared()
        {
            NavigationStack navigationStack = new NavigationStack();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));
            navigationStack.NavigateTo(new PageInfo("Page 2", null));

            int changedCount = 0;
            navigationStack.PropertyChanged += delegate (object sender, PropertyChangedEventArgs e) { if (e.PropertyName == "CurrentPage") changedCount++; };

            navigationStack.Clear();

            Assert.Equal(1, changedCount);
        }

        [Fact]
        public void PropertyChanged_CurrentPage_IsNotCalledWhenEmptyNavigationStackIsCleared()
        {
            NavigationStack navigationStack = new NavigationStack();

            int changedCount = 0;
            navigationStack.PropertyChanged += delegate (object sender, PropertyChangedEventArgs e) { if (e.PropertyName == "CurrentPage") changedCount++; };

            navigationStack.Clear();

            Assert.Equal(0, changedCount);
        }

        [Fact]
        public void PropertyChanged_CurrentPage_IsCalledWhenItemsArePushedOntoStack()
        {
            NavigationStack navigationStack = new NavigationStack();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));
            navigationStack.NavigateTo(new PageInfo("Page 2", null));

            int changedCount = 0;
            navigationStack.PropertyChanged += delegate (object sender, PropertyChangedEventArgs e) { if (e.PropertyName == "CurrentPage") changedCount++; };

            navigationStack.Push(new[]
                    {
                        new PageInfo("Page 3", null),
                        new PageInfo("Page 4", null)
                    });

            Assert.Equal(1, changedCount);
        }

        [Fact]
        public void PropertyChanged_CurrentPage_IsNotCalledWhenNoItemsArePushedOntoStack()
        {
            NavigationStack navigationStack = new NavigationStack();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));
            navigationStack.NavigateTo(new PageInfo("Page 2", null));

            int changedCount = 0;
            navigationStack.PropertyChanged += delegate (object sender, PropertyChangedEventArgs e) { if (e.PropertyName == "CurrentPage") changedCount++; };

            navigationStack.Push(new PageInfo[]
                    {
                    });

            Assert.Equal(0, changedCount);
        }

        [Fact]
        public void OnCollectionChanged_ThrowsException_NullEventArgs()
        {
            TestableNavigationStack navigationStack = new TestableNavigationStack();
            var e = Assert.Throws<ArgumentNullException>(() =>
            {
                navigationStack.OnCollectionChanged(null);
            });

            Assert.Equal("Value cannot be null.\r\nParameter name: args", e.Message);
            Assert.Equal("args", e.ParamName);
        }

        [Fact]
        public void OnNavigatingFrom_ThrowsException_NullPageInfo()
        {
            TestableNavigationStack navigationStack = new TestableNavigationStack();
            var e = Assert.Throws<ArgumentNullException>(() =>
            {
                navigationStack.OnNavigatingFrom(null, PageNavigationMode.New);
            });

            Assert.Equal("Value cannot be null.\r\nParameter name: page", e.Message);
            Assert.Equal("page", e.ParamName);
        }

        [Fact]
        public void OnNavigatingFrom_ThrowsException_InvalidNavigationMode()
        {
            TestableNavigationStack navigationStack = new TestableNavigationStack();
            var e = Assert.Throws<ArgumentException>(() =>
            {
                navigationStack.OnNavigatingFrom(new PageInfo("Page 1", null), (PageNavigationMode)100);
            });

            Assert.Equal("The argument contains an undefined enumeration value.\r\nParameter name: navigationMode", e.Message);
            Assert.Equal("navigationMode", e.ParamName);
        }

        [Fact]
        public void OnNavigatedTo_ThrowsException_NullPageInfo()
        {
            TestableNavigationStack navigationStack = new TestableNavigationStack();
            var e = Assert.Throws<ArgumentNullException>(() =>
            {
                navigationStack.OnNavigatedTo(null, PageNavigationMode.New);
            });

            Assert.Equal("Value cannot be null.\r\nParameter name: page", e.Message);
            Assert.Equal("page", e.ParamName);
        }

        [Fact]
        public void OnNavigatedTo_ThrowsException_InvalidNavigationMode()
        {
            TestableNavigationStack navigationStack = new TestableNavigationStack();
            var e = Assert.Throws<ArgumentException>(() =>
            {
                navigationStack.OnNavigatedTo(new PageInfo("Page 1", null), (PageNavigationMode)100);
            });

            Assert.Equal("The argument contains an undefined enumeration value.\r\nParameter name: navigationMode", e.Message);
            Assert.Equal("navigationMode", e.ParamName);
        }

        [Fact]
        public void OnPageDisposed_ThrowsException_NullPageInfo()
        {
            TestableNavigationStack navigationStack = new TestableNavigationStack();
            var e = Assert.Throws<ArgumentNullException>(() =>
            {
                navigationStack.OnPageDisposed(null, PageNavigationMode.New);
            });

            Assert.Equal("Value cannot be null.\r\nParameter name: page", e.Message);
            Assert.Equal("page", e.ParamName);
        }

        [Fact]
        public void OnPageDisposed_ThrowsException_InvalidNavigationMode()
        {
            TestableNavigationStack navigationStack = new TestableNavigationStack();
            var e = Assert.Throws<ArgumentException>(() =>
            {
                navigationStack.OnPageDisposed(new PageInfo("Page 1", null), (PageNavigationMode)100);
            });

            Assert.Equal("The argument contains an undefined enumeration value.\r\nParameter name: navigationMode", e.Message);
            Assert.Equal("navigationMode", e.ParamName);
        }

        // *** Private sub-classes ***

        private class TestableNavigationStack : NavigationStack
        {
            public new void OnCollectionChanged(NotifyCollectionChangedEventArgs args)
            {
                base.OnCollectionChanged(args);
            }

            public new void OnNavigatingFrom(PageInfo page, PageNavigationMode navigationMode)
            {
                base.OnNavigatingFrom(page, navigationMode);
            }

            public new void OnNavigatedTo(PageInfo page, PageNavigationMode navigationMode)
            {
                base.OnNavigatedTo(page, navigationMode);
            }

            public new void OnPageDisposed(PageInfo page, PageNavigationMode navigationMode)
            {
                base.OnPageDisposed(page, navigationMode);
            }
        }
    }
}
