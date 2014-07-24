using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
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

namespace Okra.Tests.Navigation
{
    [TestClass]
    public class NavigationStackFixture
    {
        // *** Property Tests ***

        [TestMethod]
        public void CanGoBack_IsFalseIfNoPagesNavigated()
        {
            NavigationStack navigationStack = new NavigationStack();

            Assert.AreEqual(false, navigationStack.CanGoBack);
        }

        [TestMethod]
        public void CanGoBack_IsTrueIfOnePageNavigated()
        {
            NavigationStack navigationStack = new NavigationStack();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));

            Assert.AreEqual(true, navigationStack.CanGoBack);
        }

        [TestMethod]
        public void CanGoBack_IsFalseIfOnePageNavigatedThenBack()
        {
            NavigationStack navigationStack = new NavigationStack();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));
            navigationStack.GoBack();

            Assert.AreEqual(false, navigationStack.CanGoBack);
        }

        [TestMethod]
        public void Count_IsInitiallyZero()
        {
            NavigationStack navigationStack = new NavigationStack();

            Assert.AreEqual(0, navigationStack.Count);
        }

        [TestMethod]
        public void CurrentPage_IsInitiallyNull()
        {
            NavigationStack navigationStack = new NavigationStack();

            Assert.AreEqual(null, navigationStack.CurrentPage);
        }

        // *** Method Tests ***

        [TestMethod]
        public void Clear_TwoPagesNavigatedThenClear_CountIsZero()
        {
            NavigationStack navigationStack = new NavigationStack();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));
            navigationStack.NavigateTo(new PageInfo("Page 2", null));
            navigationStack.Clear();

            Assert.AreEqual(0, navigationStack.Count);
        }

        [TestMethod]
        public void Clear_TwoPagesNavigatedThenClear_CurrentPageIsNull()
        {
            NavigationStack navigationStack = new NavigationStack();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));
            navigationStack.NavigateTo(new PageInfo("Page 2", null));
            navigationStack.Clear();

            Assert.AreEqual(null, navigationStack.CurrentPage);
        }

        [TestMethod]
        public void GetEnumerator_Generic_EnumeratesPagesInStack()
        {
            NavigationStack navigationStack = new NavigationStack();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));
            navigationStack.NavigateTo(new PageInfo("Page 2", null));

            IEnumerable<PageInfo> pages = (IEnumerable<PageInfo>)navigationStack;
            string[] pageNames = pages.Select(entry => entry.PageName).ToArray();

            CollectionAssert.AreEqual(new string[] { "Page 1", "Page 2" }, pageNames);
        }

        [TestMethod]
        public void GetEnumerator_NonGeneric_EnumeratesPagesInStack()
        {
            NavigationStack navigationStack = new NavigationStack();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));
            navigationStack.NavigateTo(new PageInfo("Page 2", null));

            List<string> pageNames = new List<string>();

            foreach (PageInfo pageInfo in (IEnumerable)navigationStack)
                pageNames.Add(pageInfo.PageName);

            CollectionAssert.AreEqual(new string[] { "Page 1", "Page 2" }, pageNames);
        }

        [TestMethod]
        public void GoBack_TwoPagesNavigatedThenBack_CountIsOne()
        {
            NavigationStack navigationStack = new NavigationStack();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));
            navigationStack.NavigateTo(new PageInfo("Page 2", null));
            navigationStack.GoBack();

            Assert.AreEqual(1, navigationStack.Count);
        }

        [TestMethod]
        public void GoBack_TwoPagesNavigatedThenBack_CorrectPageRemains()
        {
            NavigationStack navigationStack = new NavigationStack();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));
            navigationStack.NavigateTo(new PageInfo("Page 2", null));
            navigationStack.GoBack();

            Assert.AreEqual("Page 1", navigationStack[0].PageName);
        }

        [TestMethod]
        public void GoBack_TwoPagesNavigatedThenBack_CurrentPageIsLastPage()
        {
            NavigationStack navigationStack = new NavigationStack();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));
            navigationStack.NavigateTo(new PageInfo("Page 2", null));
            navigationStack.GoBack();

            Assert.AreEqual("Page 1", navigationStack.CurrentPage.PageName);
        }

        [TestMethod]
        public void GoBack_ThrowsException_NoPageInBackStack()
        {
            NavigationStack navigationStack = new NavigationStack();

            Assert.ThrowsException<InvalidOperationException>(() => navigationStack.GoBack());
        }

        [TestMethod]
        public void NavigateTo_OnePageNavigated_CountIsOne()
        {
            NavigationStack navigationStack = new NavigationStack();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));

            Assert.AreEqual(1, navigationStack.Count);
        }

        [TestMethod]
        public void NavigateTo_TwoPagesNavigated_CountIsTwo()
        {
            NavigationStack navigationStack = new NavigationStack();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));
            navigationStack.NavigateTo(new PageInfo("Page 2", null));

            Assert.AreEqual(2, navigationStack.Count);
        }

        [TestMethod]
        public void NavigateTo_OnePageNavigated_AddsSpecifiedPage()
        {
            NavigationStack navigationStack = new NavigationStack();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));

            Assert.AreEqual("Page 1", navigationStack[0].PageName);
        }

        [TestMethod]
        public void NavigateTo_TwoPagesNavigated_AddsSpecifiedPages()
        {
            NavigationStack navigationStack = new NavigationStack();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));
            navigationStack.NavigateTo(new PageInfo("Page 2", null));

            Assert.AreEqual("Page 1", navigationStack[0].PageName);
            Assert.AreEqual("Page 2", navigationStack[1].PageName);
        }

        [TestMethod]
        public void NavigateTo_OnePageNavigated_CurrentPageIsLastPage()
        {
            NavigationStack navigationStack = new NavigationStack();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));

            Assert.AreEqual("Page 1", navigationStack.CurrentPage.PageName);
        }

        [TestMethod]
        public void NavigateTo_TwoPagesNavigated_CurrentPageIsLastPage()
        {
            NavigationStack navigationStack = new NavigationStack();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));
            navigationStack.NavigateTo(new PageInfo("Page 2", null));

            Assert.AreEqual("Page 2", navigationStack.CurrentPage.PageName);
        }

        [TestMethod]
        public void NavigateTo_ThrowsException_NullPage()
        {
            NavigationStack navigationStack = new NavigationStack();

            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                navigationStack.NavigateTo(null);
            });
        }

        [TestMethod]
        public void Push_OntoEmptyStack_ReplacesStack()
        {
            NavigationStack navigationStack = new NavigationStack();

            navigationStack.Push(new[]
                    {
                        new PageInfo("Page 1", null),
                        new PageInfo("Page 2", null)
                    });

            Assert.AreEqual(2, navigationStack.Count);
        }

        [TestMethod]
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

            Assert.AreEqual(4, navigationStack.Count);
        }

        [TestMethod]
        public void Push_EmptyCollection_DoesNotChangeStack()
        {
            NavigationStack navigationStack = new NavigationStack();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));
            navigationStack.NavigateTo(new PageInfo("Page 2", null));

            navigationStack.Push(new PageInfo[]
                    {
                    });

            Assert.AreEqual(2, navigationStack.Count);
        }

        [TestMethod]
        public void Push_AddsSpecifiedPages()
        {
            NavigationStack navigationStack = new NavigationStack();

            navigationStack.Push(new[]
                    {
                        new PageInfo("Page 1", null),
                        new PageInfo("Page 2", null)
                    });

            Assert.AreEqual("Page 1", navigationStack[0].PageName);
            Assert.AreEqual("Page 2", navigationStack[1].PageName);
        }

        [TestMethod]
        public void Push_WhenPagesPushed_CurrentPageIsLastPage()
        {
            NavigationStack navigationStack = new NavigationStack();

            navigationStack.Push(new[]
                    {
                        new PageInfo("Page 1", null),
                        new PageInfo("Page 2", null)
                    });

            Assert.AreEqual("Page 2", navigationStack.CurrentPage.PageName);
        }

        [TestMethod]
        public void Push_WhenEmptyPagesPushed_CurrentPageIsLastPage()
        {
            NavigationStack navigationStack = new NavigationStack();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));
            navigationStack.NavigateTo(new PageInfo("Page 2", null));

            navigationStack.Push(new PageInfo[]
                    {
                    });

            Assert.AreEqual("Page 2", navigationStack.CurrentPage.PageName);
        }

        [TestMethod]
        public void Push_ThrowsException_NullPages()
        {
            NavigationStack navigationStack = new NavigationStack();

            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                navigationStack.Push(null);
            });
        }

        // *** Behavior Tests ***
        
        [TestMethod]
        public void CollectionChanged_IsCalledWhenFirstPageNavigated()
        {
            NavigationStack navigationStack = new NavigationStack();

            List<NotifyCollectionChangedEventArgs> changeEvents = new List<NotifyCollectionChangedEventArgs>();
            navigationStack.CollectionChanged += delegate(object sender, NotifyCollectionChangedEventArgs e) { changeEvents.Add(e); };

            navigationStack.NavigateTo(new PageInfo("Page 1", null));

            Assert.AreEqual(1, changeEvents.Count);
            NotifyCollectionChangedEventArgs changeEvent = changeEvents[0];
            Assert.AreEqual(NotifyCollectionChangedAction.Add, changeEvent.Action);
            Assert.AreEqual(1, changeEvent.NewItems.Count);
            Assert.AreEqual("Page 1", ((PageInfo)changeEvent.NewItems[0]).PageName);
            Assert.AreEqual(0, changeEvent.NewStartingIndex);
        }

        [TestMethod]
        public void CollectionChanged_IsCalledWhenSecondPageNavigated()
        {
            NavigationStack navigationStack = new NavigationStack();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));

            List<NotifyCollectionChangedEventArgs> changeEvents = new List<NotifyCollectionChangedEventArgs>();
            navigationStack.CollectionChanged += delegate(object sender, NotifyCollectionChangedEventArgs e) { changeEvents.Add(e); };

            navigationStack.NavigateTo(new PageInfo("Page 2", null));

            Assert.AreEqual(1, changeEvents.Count);
            NotifyCollectionChangedEventArgs changeEvent = changeEvents[0];
            Assert.AreEqual(NotifyCollectionChangedAction.Add, changeEvent.Action);
            Assert.AreEqual(1, changeEvent.NewItems.Count);
            Assert.AreEqual("Page 2", ((PageInfo)changeEvent.NewItems[0]).PageName);
            Assert.AreEqual(1, changeEvent.NewStartingIndex);
        }

        [TestMethod]
        public void CollectionChanged_IsCalledWhenSecondPageNavigatedThenBack()
        {
            NavigationStack navigationStack = new NavigationStack();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));
            navigationStack.NavigateTo(new PageInfo("Page 2", null));

            List<NotifyCollectionChangedEventArgs> changeEvents = new List<NotifyCollectionChangedEventArgs>();
            navigationStack.CollectionChanged += delegate(object sender, NotifyCollectionChangedEventArgs e) { changeEvents.Add(e); };

            navigationStack.GoBack();

            Assert.AreEqual(1, changeEvents.Count);
            NotifyCollectionChangedEventArgs changeEvent = changeEvents[0];
            Assert.AreEqual(NotifyCollectionChangedAction.Remove, changeEvent.Action);
            Assert.AreEqual(1, changeEvent.OldItems.Count);
            Assert.AreEqual("Page 2", ((PageInfo)changeEvent.OldItems[0]).PageName);
            Assert.AreEqual(1, changeEvent.OldStartingIndex);
        }

        [TestMethod]
        public void CollectionChanged_IsCalledWhenNavigationStackIsCleared()
        {
            NavigationStack navigationStack = new NavigationStack();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));
            navigationStack.NavigateTo(new PageInfo("Page 2", null));

            List<NotifyCollectionChangedEventArgs> changeEvents = new List<NotifyCollectionChangedEventArgs>();
            navigationStack.CollectionChanged += delegate(object sender, NotifyCollectionChangedEventArgs e) { changeEvents.Add(e); };

            navigationStack.Clear();

            Assert.AreEqual(1, changeEvents.Count);
            NotifyCollectionChangedEventArgs changeEvent = changeEvents[0];
            Assert.AreEqual(NotifyCollectionChangedAction.Reset, changeEvent.Action);
        }

        [TestMethod]
        public void CollectionChanged_IsCalledWhenPagesArePushedOntoStack()
        {
            NavigationStack navigationStack = new NavigationStack();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));
            navigationStack.NavigateTo(new PageInfo("Page 2", null));

            List<NotifyCollectionChangedEventArgs> changeEvents = new List<NotifyCollectionChangedEventArgs>();
            navigationStack.CollectionChanged += delegate(object sender, NotifyCollectionChangedEventArgs e) { changeEvents.Add(e); };

            navigationStack.Push(new[]
                    {
                        new PageInfo("Page 3", null),
                        new PageInfo("Page 4", null)
                    });

            Assert.AreEqual(1, changeEvents.Count);
            NotifyCollectionChangedEventArgs changeEvent = changeEvents[0];
            Assert.AreEqual(NotifyCollectionChangedAction.Add, changeEvent.Action);
            Assert.AreEqual(2, changeEvent.NewItems.Count);
            Assert.AreEqual("Page 3", ((PageInfo)changeEvent.NewItems[0]).PageName);
            Assert.AreEqual("Page 4", ((PageInfo)changeEvent.NewItems[1]).PageName);
            Assert.AreEqual(2, changeEvent.NewStartingIndex);
        }

        [TestMethod]
        public void CollectionChanged_IsNotCalledWhenNoPagesArePushedOntoStack()
        {
            NavigationStack navigationStack = new NavigationStack();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));
            navigationStack.NavigateTo(new PageInfo("Page 2", null));

            List<NotifyCollectionChangedEventArgs> changeEvents = new List<NotifyCollectionChangedEventArgs>();
            navigationStack.CollectionChanged += delegate(object sender, NotifyCollectionChangedEventArgs e) { changeEvents.Add(e); };

            navigationStack.Push(new PageInfo[]
                    {
                    });

            Assert.AreEqual(0, changeEvents.Count);
        }

        [TestMethod]
        public void NavigatedTo_IsCalledWhenFirstPageNavigated()
        {
            NavigationStack navigationStack = new NavigationStack();

            List<PageNavigationEventArgs> navigationEvents = new List<PageNavigationEventArgs>();
            navigationStack.NavigatedTo += delegate(object sender, PageNavigationEventArgs e) { navigationEvents.Add(e); };

            navigationStack.NavigateTo(new PageInfo("Page 1", null));

            Assert.AreEqual(1, navigationEvents.Count);
            PageNavigationEventArgs navigationEvent = navigationEvents[0];
            Assert.AreEqual(PageNavigationMode.New, navigationEvent.NavigationMode);
            Assert.AreEqual("Page 1", navigationEvent.Page.PageName);
        }

        [TestMethod]
        public void NavigatedTo_IsCalledWhenSecondPageNavigated()
        {
            NavigationStack navigationStack = new NavigationStack();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));

            List<PageNavigationEventArgs> navigationEvents = new List<PageNavigationEventArgs>();
            navigationStack.NavigatedTo += delegate(object sender, PageNavigationEventArgs e) { navigationEvents.Add(e); };

            navigationStack.NavigateTo(new PageInfo("Page 2", null));

            Assert.AreEqual(1, navigationEvents.Count);
            PageNavigationEventArgs navigationEvent = navigationEvents[0];
            Assert.AreEqual(PageNavigationMode.New, navigationEvent.NavigationMode);
            Assert.AreEqual("Page 2", navigationEvent.Page.PageName);
        }

        [TestMethod]
        public void NavigatedTo_IsNotCalledWhenFirstPageNavigatedThenBack()
        {
            NavigationStack navigationStack = new NavigationStack();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));

            List<PageNavigationEventArgs> navigationEvents = new List<PageNavigationEventArgs>();
            navigationStack.NavigatedTo += delegate(object sender, PageNavigationEventArgs e) { navigationEvents.Add(e); };

            navigationStack.GoBack();

            Assert.AreEqual(0, navigationEvents.Count);
        }

        [TestMethod]
        public void NavigatedTo_IsCalledWhenSecondPageNavigatedThenBack()
        {
            NavigationStack navigationStack = new NavigationStack();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));
            navigationStack.NavigateTo(new PageInfo("Page 2", null));

            List<PageNavigationEventArgs> navigationEvents = new List<PageNavigationEventArgs>();
            navigationStack.NavigatedTo += delegate(object sender, PageNavigationEventArgs e) { navigationEvents.Add(e); };

            navigationStack.GoBack();

            Assert.AreEqual(1, navigationEvents.Count);
            PageNavigationEventArgs navigationEvent = navigationEvents[0];
            Assert.AreEqual(PageNavigationMode.Back, navigationEvent.NavigationMode);
            Assert.AreEqual("Page 1", navigationEvent.Page.PageName);
        }

        [TestMethod]
        public void NavigatedTo_IsCalledWhenPagesArePushedOntoStack()
        {
            NavigationStack navigationStack = new NavigationStack();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));
            navigationStack.NavigateTo(new PageInfo("Page 2", null));

            List<PageNavigationEventArgs> navigationEvents = new List<PageNavigationEventArgs>();
            navigationStack.NavigatedTo += delegate(object sender, PageNavigationEventArgs e) { navigationEvents.Add(e); };

            navigationStack.Push(new[]
                    {
                        new PageInfo("Page 3", null),
                        new PageInfo("Page 4", null)
                    });

            Assert.AreEqual(1, navigationEvents.Count);
            PageNavigationEventArgs navigationEvent = navigationEvents[0];
            Assert.AreEqual(PageNavigationMode.New, navigationEvent.NavigationMode);
            Assert.AreEqual("Page 4", navigationEvent.Page.PageName);
        }

        [TestMethod]
        public void NavigatedTo_IsNotCalledWhenNoPagesArePushedOntoStack()
        {
            NavigationStack navigationStack = new NavigationStack();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));
            navigationStack.NavigateTo(new PageInfo("Page 2", null));

            List<PageNavigationEventArgs> navigationEvents = new List<PageNavigationEventArgs>();
            navigationStack.NavigatedTo += delegate(object sender, PageNavigationEventArgs e) { navigationEvents.Add(e); };

            navigationStack.Push(new PageInfo[]
                    {
                    });

            Assert.AreEqual(0, navigationEvents.Count);
        }

        [TestMethod]
        public void NavigatingFrom_IsNotCalledWhenFirstPageNavigated()
        {
            NavigationStack navigationStack = new NavigationStack();

            List<PageNavigationEventArgs> navigationEvents = new List<PageNavigationEventArgs>();
            navigationStack.NavigatingFrom += delegate(object sender, PageNavigationEventArgs e) { navigationEvents.Add(e); };

            navigationStack.NavigateTo(new PageInfo("Page 1", null));

            Assert.AreEqual(0, navigationEvents.Count);
        }

        [TestMethod]
        public void NavigatingFrom_IsCalledWhenSecondPageNavigated()
        {
            NavigationStack navigationStack = new NavigationStack();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));

            List<PageNavigationEventArgs> navigationEvents = new List<PageNavigationEventArgs>();
            navigationStack.NavigatingFrom += delegate(object sender, PageNavigationEventArgs e) { navigationEvents.Add(e); };

            navigationStack.NavigateTo(new PageInfo("Page 2", null));

            Assert.AreEqual(1, navigationEvents.Count);
            PageNavigationEventArgs navigationEvent = navigationEvents[0];
            Assert.AreEqual(PageNavigationMode.New, navigationEvent.NavigationMode);
            Assert.AreEqual("Page 1", navigationEvent.Page.PageName);
        }

        [TestMethod]
        public void NavigatingFrom_IsCalledWhenNavigationStackIsCleared()
        {
            NavigationStack navigationStack = new NavigationStack();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));
            navigationStack.NavigateTo(new PageInfo("Page 2", null));

            List<PageNavigationEventArgs> navigationEvents = new List<PageNavigationEventArgs>();
            navigationStack.NavigatingFrom += delegate(object sender, PageNavigationEventArgs e) { navigationEvents.Add(e); };

            navigationStack.GoBack();

            Assert.AreEqual(1, navigationEvents.Count);
            PageNavigationEventArgs navigationEvent = navigationEvents[0];
            Assert.AreEqual(PageNavigationMode.Back, navigationEvent.NavigationMode);
            Assert.AreEqual("Page 2", navigationEvent.Page.PageName);
        }

        [TestMethod]
        public void NavigatingFrom_IsCalledWhenSecondPageNavigatedThenBack()
        {
            NavigationStack navigationStack = new NavigationStack();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));
            navigationStack.NavigateTo(new PageInfo("Page 2", null));

            List<PageNavigationEventArgs> navigationEvents = new List<PageNavigationEventArgs>();
            navigationStack.NavigatingFrom += delegate(object sender, PageNavigationEventArgs e) { navigationEvents.Add(e); };

            navigationStack.Clear();

            Assert.AreEqual(1, navigationEvents.Count);
            PageNavigationEventArgs navigationEvent = navigationEvents[0];
            Assert.AreEqual(PageNavigationMode.Back, navigationEvent.NavigationMode);
            Assert.AreEqual("Page 2", navigationEvent.Page.PageName);
        }

        [TestMethod]
        public void NavigatingFrom_IsNotCalledWhenNoPagesArePushedOntoTheStack()
        {
            NavigationStack navigationStack = new NavigationStack();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));
            navigationStack.NavigateTo(new PageInfo("Page 2", null));

            List<PageNavigationEventArgs> navigationEvents = new List<PageNavigationEventArgs>();
            navigationStack.NavigatingFrom += delegate(object sender, PageNavigationEventArgs e) { navigationEvents.Add(e); };

            navigationStack.Push(new PageInfo[]
                    {
                    });

            Assert.AreEqual(0, navigationEvents.Count);
        }

        [TestMethod]
        public void PageDisposed_IsCalledWhenGoingBack()
        {
            NavigationStack navigationStack = new NavigationStack();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));
            navigationStack.NavigateTo(new PageInfo("Page 2", null));

            List<PageNavigationEventArgs> pageDisposedEvents = new List<PageNavigationEventArgs>();
            navigationStack.PageDisposed += delegate(object sender, PageNavigationEventArgs e) { pageDisposedEvents.Add(e); };

            navigationStack.GoBack();

            Assert.AreEqual(1, pageDisposedEvents.Count);
            PageNavigationEventArgs pageDisposedEvent = pageDisposedEvents[0];
            Assert.AreEqual(PageNavigationMode.Back, pageDisposedEvent.NavigationMode);
            Assert.AreEqual("Page 2", pageDisposedEvent.Page.PageName);
        }

        [TestMethod]
        public void PageDisposed_IsNotCalledWhenNavigatingToANewPage()
        {
            NavigationStack navigationStack = new NavigationStack();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));            

            List<PageNavigationEventArgs> pageDisposedEvents = new List<PageNavigationEventArgs>();
            navigationStack.PageDisposed += delegate(object sender, PageNavigationEventArgs e) { pageDisposedEvents.Add(e); };

            navigationStack.NavigateTo(new PageInfo("Page 2", null));

            Assert.AreEqual(0, pageDisposedEvents.Count);
        }
        
        [TestMethod]
        public void PropertyChanged_CanGoBack_IsCalledWhenFirstPageNavigated()
        {
            NavigationStack navigationStack = new NavigationStack();

            int changedCount = 0;
            navigationStack.PropertyChanged += delegate(object sender, PropertyChangedEventArgs e) { if (e.PropertyName == "CanGoBack") changedCount++; };

            navigationStack.NavigateTo(new PageInfo("Page 1", null));

            Assert.AreEqual(1, changedCount);
        }

        [TestMethod]
        public void PropertyChanged_CanGoBack_IsNotCalledWhenSecondPageNavigated()
        {
            NavigationStack navigationStack = new NavigationStack();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));

            int changedCount = 0;
            navigationStack.PropertyChanged += delegate(object sender, PropertyChangedEventArgs e) { if (e.PropertyName == "CanGoBack") changedCount++; };

            navigationStack.NavigateTo(new PageInfo("Page 2", null));

            Assert.AreEqual(0, changedCount);
        }

        [TestMethod]
        public void PropertyChanged_CanGoBack_IsNotCalledWhenSecondPageNavigatedThenBack()
        {
            NavigationStack navigationStack = new NavigationStack();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));
            navigationStack.NavigateTo(new PageInfo("Page 2", null));

            int changedCount = 0;
            navigationStack.PropertyChanged += delegate(object sender, PropertyChangedEventArgs e) { if (e.PropertyName == "CanGoBack") changedCount++; };

            navigationStack.GoBack();

            Assert.AreEqual(0, changedCount);
        }

        [TestMethod]
        public void PropertyChanged_CanGoBack_IsCalledWhenFirstPageNavigatedThenBack()
        {
            NavigationStack navigationStack = new NavigationStack();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));

            int changedCount = 0;
            navigationStack.PropertyChanged += delegate(object sender, PropertyChangedEventArgs e) { if (e.PropertyName == "CanGoBack") changedCount++; };

            navigationStack.GoBack();

            Assert.AreEqual(1, changedCount);
        }

        [TestMethod]
        public void PropertyChanged_CanGoBack_IsCalledWhenNavigationStackIsCleared()
        {
            NavigationStack navigationStack = new NavigationStack();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));

            int changedCount = 0;
            navigationStack.PropertyChanged += delegate(object sender, PropertyChangedEventArgs e) { if (e.PropertyName == "CanGoBack") changedCount++; };

            navigationStack.Clear();

            Assert.AreEqual(1, changedCount);
        }

        [TestMethod]
        public void PropertyChanged_CanGoBack_IsNotCalledWhenEmptyNavigationStackIsCleared()
        {
            NavigationStack navigationStack = new NavigationStack();

            int changedCount = 0;
            navigationStack.PropertyChanged += delegate(object sender, PropertyChangedEventArgs e) { if (e.PropertyName == "CanGoBack") changedCount++; };

            navigationStack.Clear();

            Assert.AreEqual(0, changedCount);
        }

        [TestMethod]
        public void PropertyChanged_CanGoBack_IsCalledWhenItemsArePushedOntoEmptyStack()
        {
            NavigationStack navigationStack = new NavigationStack();

            int changedCount = 0;
            navigationStack.PropertyChanged += delegate(object sender, PropertyChangedEventArgs e) { if (e.PropertyName == "CanGoBack") changedCount++; };

            navigationStack.Push(new[]
                    {
                        new PageInfo("Page 3", null),
                        new PageInfo("Page 4", null)
                    });

            Assert.AreEqual(1, changedCount);
        }

        [TestMethod]
        public void PropertyChanged_CanGoBack_IsNotCalledWhenItemsArePushedOntoExistingStack()
        {
            NavigationStack navigationStack = new NavigationStack();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));

            int changedCount = 0;
            navigationStack.PropertyChanged += delegate(object sender, PropertyChangedEventArgs e) { if (e.PropertyName == "CanGoBack") changedCount++; };

            navigationStack.Push(new[]
                    {
                        new PageInfo("Page 3", null),
                        new PageInfo("Page 4", null)
                    });

            Assert.AreEqual(0, changedCount);
        }

        [TestMethod]
        public void PropertyChanged_CanGoBack_IsNotCalledWhenNoPagesArePushedOntoEmptyStack()
        {
            NavigationStack navigationStack = new NavigationStack();

            int changedCount = 0;
            navigationStack.PropertyChanged += delegate(object sender, PropertyChangedEventArgs e) { if (e.PropertyName == "CanGoBack") changedCount++; };

            navigationStack.Push(new PageInfo[]
                    {
                    });

            Assert.AreEqual(0, changedCount);
        }

        [TestMethod]
        public void PropertyChanged_Count_IsCalledWhenFirstPageNavigated()
        {
            NavigationStack navigationStack = new NavigationStack();

            int changedCount = 0;
            navigationStack.PropertyChanged += delegate(object sender, PropertyChangedEventArgs e) { if (e.PropertyName == "Count") changedCount++; };

            navigationStack.NavigateTo(new PageInfo("Page 1", null));

            Assert.AreEqual(1, changedCount);
        }

        [TestMethod]
        public void PropertyChanged_Count_IsCalledWhenSecondPageNavigated()
        {
            NavigationStack navigationStack = new NavigationStack();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));

            int changedCount = 0;
            navigationStack.PropertyChanged += delegate(object sender, PropertyChangedEventArgs e) { if (e.PropertyName == "Count") changedCount++; };

            navigationStack.NavigateTo(new PageInfo("Page 2", null));

            Assert.AreEqual(1, changedCount);
        }

        [TestMethod]
        public void PropertyChanged_Count_IsCalledWhenSecondPageNavigatedThenBack()
        {
            NavigationStack navigationStack = new NavigationStack();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));
            navigationStack.NavigateTo(new PageInfo("Page 2", null));

            int changedCount = 0;
            navigationStack.PropertyChanged += delegate(object sender, PropertyChangedEventArgs e) { if (e.PropertyName == "Count") changedCount++; };

            navigationStack.GoBack();

            Assert.AreEqual(1, changedCount);
        }

        [TestMethod]
        public void PropertyChanged_Count_IsCalledWhenNavigationStackIsCleared()
        {
            NavigationStack navigationStack = new NavigationStack();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));
            navigationStack.NavigateTo(new PageInfo("Page 2", null));

            int changedCount = 0;
            navigationStack.PropertyChanged += delegate(object sender, PropertyChangedEventArgs e) { if (e.PropertyName == "Count") changedCount++; };

            navigationStack.Clear();

            Assert.AreEqual(1, changedCount);
        }

        [TestMethod]
        public void PropertyChanged_Count_IsNotCalledWhenEmptyNavigationStackIsCleared()
        {
            NavigationStack navigationStack = new NavigationStack();
            
            int changedCount = 0;
            navigationStack.PropertyChanged += delegate(object sender, PropertyChangedEventArgs e) { if (e.PropertyName == "Count") changedCount++; };

            navigationStack.Clear();

            Assert.AreEqual(0, changedCount);
        }

        [TestMethod]
        public void PropertyChanged_Count_IsCalledWhenItemsArePushedOntoStack()
        {
            NavigationStack navigationStack = new NavigationStack();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));
            navigationStack.NavigateTo(new PageInfo("Page 2", null));

            int changedCount = 0;
            navigationStack.PropertyChanged += delegate(object sender, PropertyChangedEventArgs e) { if (e.PropertyName == "Count") changedCount++; };

            navigationStack.Push(new[]
                    {
                        new PageInfo("Page 3", null),
                        new PageInfo("Page 4", null)
                    });

            Assert.AreEqual(1, changedCount);
        }

        [TestMethod]
        public void PropertyChanged_Count_IsNotCalledWhenNoPagesArePushedOntoStack()
        {
            NavigationStack navigationStack = new NavigationStack();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));
            navigationStack.NavigateTo(new PageInfo("Page 2", null));

            int changedCount = 0;
            navigationStack.PropertyChanged += delegate(object sender, PropertyChangedEventArgs e) { if (e.PropertyName == "Count") changedCount++; };

            navigationStack.Push(new PageInfo[]
                    {
                    });

            Assert.AreEqual(0, changedCount);
        }

        [TestMethod]
        public void PropertyChanged_CurrentPage_IsCalledWhenFirstPageNavigated()
        {
            NavigationStack navigationStack = new NavigationStack();

            int changedCount = 0;
            navigationStack.PropertyChanged += delegate(object sender, PropertyChangedEventArgs e) { if (e.PropertyName == "CurrentPage") changedCount++; };

            navigationStack.NavigateTo(new PageInfo("Page 1", null));

            Assert.AreEqual(1, changedCount);
        }

        [TestMethod]
        public void PropertyChanged_CurrentPage_IsCalledWhenSecondPageNavigated()
        {
            NavigationStack navigationStack = new NavigationStack();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));

            int changedCount = 0;
            navigationStack.PropertyChanged += delegate(object sender, PropertyChangedEventArgs e) { if (e.PropertyName == "CurrentPage") changedCount++; };

            navigationStack.NavigateTo(new PageInfo("Page 2", null));

            Assert.AreEqual(1, changedCount);
        }

        [TestMethod]
        public void PropertyChanged_CurrentPage_IsCalledWhenSecondPageNavigatedThenBack()
        {
            NavigationStack navigationStack = new NavigationStack();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));
            navigationStack.NavigateTo(new PageInfo("Page 2", null));

            int changedCount = 0;
            navigationStack.PropertyChanged += delegate(object sender, PropertyChangedEventArgs e) { if (e.PropertyName == "CurrentPage") changedCount++; };

            navigationStack.GoBack();

            Assert.AreEqual(1, changedCount);
        }

        [TestMethod]
        public void PropertyChanged_CurrentPage_IsCalledWhenNavigationStackIsCleared()
        {
            NavigationStack navigationStack = new NavigationStack();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));
            navigationStack.NavigateTo(new PageInfo("Page 2", null));

            int changedCount = 0;
            navigationStack.PropertyChanged += delegate(object sender, PropertyChangedEventArgs e) { if (e.PropertyName == "CurrentPage") changedCount++; };

            navigationStack.Clear();

            Assert.AreEqual(1, changedCount);
        }

        [TestMethod]
        public void PropertyChanged_CurrentPage_IsNotCalledWhenEmptyNavigationStackIsCleared()
        {
            NavigationStack navigationStack = new NavigationStack();

            int changedCount = 0;
            navigationStack.PropertyChanged += delegate(object sender, PropertyChangedEventArgs e) { if (e.PropertyName == "CurrentPage") changedCount++; };

            navigationStack.Clear();

            Assert.AreEqual(0, changedCount);
        }

        [TestMethod]
        public void PropertyChanged_CurrentPage_IsCalledWhenItemsArePushedOntoStack()
        {
            NavigationStack navigationStack = new NavigationStack();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));
            navigationStack.NavigateTo(new PageInfo("Page 2", null));

            int changedCount = 0;
            navigationStack.PropertyChanged += delegate(object sender, PropertyChangedEventArgs e) { if (e.PropertyName == "CurrentPage") changedCount++; };

            navigationStack.Push(new[]
                    {
                        new PageInfo("Page 3", null),
                        new PageInfo("Page 4", null)
                    });

            Assert.AreEqual(1, changedCount);
        }

        [TestMethod]
        public void PropertyChanged_CurrentPage_IsNotCalledWhenNoItemsArePushedOntoStack()
        {
            NavigationStack navigationStack = new NavigationStack();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));
            navigationStack.NavigateTo(new PageInfo("Page 2", null));

            int changedCount = 0;
            navigationStack.PropertyChanged += delegate(object sender, PropertyChangedEventArgs e) { if (e.PropertyName == "CurrentPage") changedCount++; };

            navigationStack.Push(new PageInfo[]
                    {
                    });

            Assert.AreEqual(0, changedCount);
        }
    }
}
