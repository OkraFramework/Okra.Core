using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using Okra.Navigation;
using System.Composition;
using System.Composition.Hosting.Core;
using Okra.MEF.Tests.Mocks;
using Xunit;

namespace Okra.MEF.Tests.Navigation
{
    public class ViewFactoryFixture
    {
        // *** Constructor Tests ***

        [Fact]
        public void Constructor_ThrowsException_IfCompositionContextFactoryIsNull()
        {
            Lazy<object, PageMetadata>[] lazyPageExports = new Lazy<object, PageMetadata>[]
                            {
                                new Lazy<object, PageMetadata>(() => {throw new InvalidOperationException();}, new PageMetadata() { PageName = "Page 1"}),
                                new Lazy<object, PageMetadata>(() => {throw new InvalidOperationException();}, new PageMetadata() { PageName = "Page 2"}),
                                new Lazy<object, PageMetadata>(() => {throw new InvalidOperationException();}, new PageMetadata() { PageName = "Page 3"})
                            };

            Assert.Throws<ArgumentNullException>(() => new ViewFactory(null, lazyPageExports));
        }

        [Fact]
        public void Constructor_ThrowsException_IfPageExportsIsNull()
        {
            Dictionary<CompositionContract, Func<object>> exportFactories = new Dictionary<CompositionContract, Func<object>>();
            ExportFactory<CompositionContext> compositionContextFactory = new ExportFactory<CompositionContext>(() => CreateCompositionContext(exportFactories));

            Assert.Throws<ArgumentNullException>(() => new ViewFactory(compositionContextFactory, null));
        }

        // *** Method Tests ***

        [Fact]
        public void AttachViewModel_ThrowsException_IfPageIsNull()
        {
            TestableViewFactory viewFactory = CreateViewFactory();

            Assert.Throws<ArgumentNullException>(() => viewFactory.CallAttachViewModel(null, new object()));
        }

        [Fact]
        public void AttachViewModel_ThrowsException_IfViewModelIsNull()
        {
            TestableViewFactory viewFactory = CreateViewFactory();

            Assert.Throws<ArgumentNullException>(() => viewFactory.CallAttachViewModel(new object(), null));
        }

        [Fact]
        public void CreateView_CreatesNewPage_WithViewModel()
        {
            IViewFactory viewFactory = CreateViewFactory();
            INavigationContext navigationContext = CreateNavigationContext();

            IViewLifetimeContext lifetimeContext = viewFactory.CreateView("Page 2", navigationContext);
            object view = lifetimeContext.View;

            Assert.NotNull(view);
            Assert.IsAssignableFrom(typeof(MockPage),view);
            Assert.Equal("Page 2", ((MockPage)view).PageName);
        }

        [Fact]
        public void CreateView_CreatesNewPage_WithoutViewModel()
        {
            IViewFactory viewFactory = CreateViewFactory();
            INavigationContext navigationContext = CreateNavigationContext();

            IViewLifetimeContext lifetimeContext = viewFactory.CreateView("Page 3", navigationContext);
            object view = lifetimeContext.View;

            Assert.NotNull(view);
            Assert.IsAssignableFrom(typeof(MockPage),view);
            Assert.Equal("Page 3", ((MockPage)view).PageName);
        }

        [Fact]
        public void CreateView_CreatesNewViewModel_WithViewModel()
        {
            IViewFactory viewFactory = CreateViewFactory();
            INavigationContext navigationContext = CreateNavigationContext();

            IViewLifetimeContext lifetimeContext = viewFactory.CreateView("Page 2", navigationContext);
            object viewModel = lifetimeContext.ViewModel;

            Assert.NotNull(viewModel);
            Assert.IsAssignableFrom(typeof(MockViewModel<string, string>),viewModel);
            Assert.Equal("ViewModel 2", ((MockViewModel<string, string>)viewModel).Name);
        }

        [Fact]
        public void CreateView_ViewModelIsNull_WithoutViewModel()
        {
            IViewFactory viewFactory = CreateViewFactory();
            INavigationContext navigationContext = CreateNavigationContext();

            IViewLifetimeContext lifetimeContext = viewFactory.CreateView("Page 3", navigationContext);
            object viewModel = lifetimeContext.ViewModel;

            Assert.Null(viewModel);
        }

        [Fact]
        public void CreateView_SetsViewModel_WithViewModel()
        {
            IViewFactory viewFactory = CreateViewFactory();
            INavigationContext navigationContext = CreateNavigationContext();

            IViewLifetimeContext lifetimeContext = viewFactory.CreateView("Page 2", navigationContext);
            MockPage page = lifetimeContext.View as MockPage;
            object viewModel = lifetimeContext.ViewModel;

            Assert.Equal(viewModel, page.DataContext);
        }

        [Fact]
        public void CreateView_SetsViewModel_ToNullWithoutViewModel()
        {
            IViewFactory viewFactory = CreateViewFactory();
            INavigationContext navigationContext = CreateNavigationContext();

            IViewLifetimeContext lifetimeContext = viewFactory.CreateView("Page 3", navigationContext);
            MockPage page = lifetimeContext.View as MockPage;
            object viewModel = page.DataContext;

            Assert.Null(viewModel);
        }

        [Fact]
        public void CreateView_InjectsNavigationContextIntoProxy()
        {
            NavigationContextProxy proxy = new NavigationContextProxy();
            IViewFactory viewFactory = CreateViewFactory(proxy);
            INavigationContext navigationContext = CreateNavigationContext();

            IViewLifetimeContext lifetimeContext = viewFactory.CreateView("Page 2", navigationContext);

            Assert.Equal(navigationContext.GetCurrent(), proxy.GetCurrent());
        }

        [Fact]
        public void CreateView_ThrowsException_NoPageWithSpecifiedName()
        {
            IViewFactory viewFactory = CreateViewFactory();
            INavigationContext navigationContext = CreateNavigationContext();

            Assert.Throws<InvalidOperationException>(() => viewFactory.CreateView("Page X", navigationContext));
        }

        [Fact]
        public void CreateView_ThrowsException_PageNameIsNull()
        {
            IViewFactory viewFactory = CreateViewFactory();
            INavigationContext navigationContext = CreateNavigationContext();

            Assert.Throws<ArgumentException>(() => viewFactory.CreateView(null, navigationContext));
        }

        [Fact]
        public void CreateView_ThrowsException_PageNameIsEmpty()
        {
            IViewFactory viewFactory = CreateViewFactory();
            INavigationContext navigationContext = CreateNavigationContext();

            Assert.Throws<ArgumentException>(() => viewFactory.CreateView("", navigationContext));
        }

        [Fact]
        public void CreateView_ThrowsException_IfNavigationContextIsNull()
        {
            IViewFactory viewFactory = CreateViewFactory();

            Assert.Throws<ArgumentNullException>(() => viewFactory.CreateView("Page 2", null));
        }

        [Fact]
        public void IsViewDefined_ReturnsTrue_SpecifiedPageExists()
        {
            IViewFactory viewFactory = CreateViewFactory();

            bool viewDefined = viewFactory.IsViewDefined("Page 2");

            Assert.Equal(true, viewDefined);
        }

        [Fact]
        public void IsViewDefined_ReturnsFalse_NoPageWithSpecifiedName()
        {
            IViewFactory viewFactory = CreateViewFactory();

            bool viewDefined = viewFactory.IsViewDefined("Page X");

            Assert.Equal(false, viewDefined);
        }

        [Fact]
        public void IsViewDefined_ThrowsException_IfPageNameIsNull()
        {
            IViewFactory viewFactory = CreateViewFactory();
            INavigationContext navigationContext = CreateNavigationContext();

            Assert.Throws<ArgumentException>(() => viewFactory.IsViewDefined(null));
        }

        [Fact]
        public void IsViewDefined_ThrowsException_IfPageNameIsEmpty()
        {
            IViewFactory viewFactory = CreateViewFactory();
            INavigationContext navigationContext = CreateNavigationContext();

            Assert.Throws<ArgumentException>(() => viewFactory.IsViewDefined(""));
        }

        // *** Behaviour Tests ***

        [Fact]
        public void DisposingViewLifetimeContext_DisposesCurrentPage_WithViewModel()
        {
            IViewFactory viewFactory = CreateViewFactory();
            INavigationContext navigationContext = CreateNavigationContext();

            IViewLifetimeContext lifetimeContext = viewFactory.CreateView("Page 2", navigationContext);
            MockPage page = lifetimeContext.View as MockPage;

            lifetimeContext.Dispose();

            Assert.Equal(true, page.IsDisposed);
        }

        [Fact]
        public void DisposingViewLifetimeContext_DisposesCurrentPage_WithoutViewModel()
        {
            IViewFactory viewFactory = CreateViewFactory();
            INavigationContext navigationContext = CreateNavigationContext();

            IViewLifetimeContext lifetimeContext = viewFactory.CreateView("Page 3", navigationContext);
            MockPage page = lifetimeContext.View as MockPage;

            lifetimeContext.Dispose();

            Assert.Equal(true, page.IsDisposed);
        }

        [Fact]
        public void DisposingViewLifetimeContext_DisposesCurrentViewModel()
        {
            IViewFactory viewFactory = CreateViewFactory();
            INavigationContext navigationContext = CreateNavigationContext();

            IViewLifetimeContext lifetimeContext = viewFactory.CreateView("Page 2", navigationContext);
            MockPage page = lifetimeContext.View as MockPage;
            MockViewModel<string, string> viewModel = page.DataContext as MockViewModel<string, string>;

            lifetimeContext.Dispose();

            Assert.Equal(true, viewModel.IsDisposed);
        }

        // *** Private Methods ***

        private TestableViewFactory CreateViewFactory(NavigationContextProxy navigationContextProxy = null)
        {
            if (navigationContextProxy == null)
                navigationContextProxy = new NavigationContextProxy();

            // Create the composition context exporter

            Dictionary<CompositionContract, Func<object>> exportFactories = new Dictionary<CompositionContract, Func<object>>();

            exportFactories[CreatePageContract("Page 1")] = () => new MockPage() { PageName = "Page 1" };
            exportFactories[CreatePageContract("Page 2")] = () => new MockPage() { PageName = "Page 2" };
            exportFactories[CreatePageContract("Page 3")] = () => new MockPage() { PageName = "Page 3" };

            exportFactories[CreateViewModelContract("Page 1")] = () => new MockViewModel<string, string>() { Name = "ViewModel 1" };
            exportFactories[CreateViewModelContract("Page 2")] = () => new MockViewModel<string, string>() { Name = "ViewModel 2" };

            exportFactories[new CompositionContract(typeof(INavigationContext))] = () => navigationContextProxy;

            ExportFactory<CompositionContext> compositionContextFactory = new ExportFactory<CompositionContext>(() => CreateCompositionContext(exportFactories));

            // Create the lazy page exports (for identifying if a view exists)
            // NB: These should never be used to create

            Lazy<object, PageMetadata>[] lazyPageExports = new Lazy<object, PageMetadata>[]
                            {
                                new Lazy<object, PageMetadata>(() => {throw new InvalidOperationException();}, new PageMetadata() { PageName = "Page 1"}),
                                new Lazy<object, PageMetadata>(() => {throw new InvalidOperationException();}, new PageMetadata() { PageName = "Page 2"}),
                                new Lazy<object, PageMetadata>(() => {throw new InvalidOperationException();}, new PageMetadata() { PageName = "Page 3"})
                            };

            // Return a new IViewFactory

            return new TestableViewFactory(compositionContextFactory, lazyPageExports);
        }

        private Tuple<CompositionContext, Action> CreateCompositionContext(Dictionary<CompositionContract, Func<object>> exportFactories)
        {
            MockCompositionContext compositionContext = new MockCompositionContext(exportFactories);

            return new Tuple<CompositionContext, Action>(compositionContext, () => compositionContext.Dispose());
        }

        private CompositionContract CreatePageContract(string pageName)
        {
            Dictionary<string, object> metadata = new Dictionary<string, object>();
            metadata["PageName"] = pageName;
            return new CompositionContract(typeof(object), "OkraPage", metadata);
        }

        private CompositionContract CreateViewModelContract(string viewModelName)
        {
            Dictionary<string, object> metadata = new Dictionary<string, object>();
            metadata["PageName"] = viewModelName;
            return new CompositionContract(typeof(object), "OkraViewModel", metadata);
        }

        private INavigationContext CreateNavigationContext(INavigationBase current = null)
        {
            if (current == null)
                current = new MockNavigationManager();

            return new MockNavigationContext(current);
        }

        // *** Private Sub-classes ***

        private class TestableViewFactory : ViewFactory
        {
            // *** Constructors ***

            public TestableViewFactory(ExportFactory<CompositionContext> compositionContextFactory, Lazy<object, PageMetadata>[] lazyPageExports)
                : base(compositionContextFactory, lazyPageExports)
            {
            }

            // *** Methods ***

            public void CallAttachViewModel(object page, object viewModel)
            {
                base.AttachViewModel(page, viewModel);
            }

            // *** Overriden Base Methods ***

            protected override void AttachViewModel(object page, object viewModel)
            {
                if (page is MockPage)
                    ((MockPage)page).DataContext = viewModel;
            }
        }

        private class MockCompositionContext : CompositionContext
        {
            private Dictionary<CompositionContract, Func<object>> _exportFactories;
            private IList<object> _exports = new List<object>();

            // *** Constructors ***

            public MockCompositionContext(Dictionary<CompositionContract, Func<object>> exportFactories)
            {
                _exportFactories = exportFactories;
            }

            // *** Overriden Base Methods ***

            public override bool TryGetExport(CompositionContract contract, out object export)
            {
                Func<object> exportFactory;

                if (_exportFactories.TryGetValue(contract, out exportFactory))
                {
                    export = exportFactory();
                    _exports.Add(export);
                    return true;
                }

                export = null;
                return false;
            }

            // *** Methods ***

            public void Dispose()
            {
                foreach (object export in _exports)
                {
                    if (export is IDisposable)
                        ((IDisposable)export).Dispose();
                }
            }
        }

        private class MockPage : IDisposable
        {
            // *** Properties ***

            public bool IsDisposed { get; set; }
            public string PageName { get; set; }
            public object DataContext { get; set; }

            // *** Methods ***

            public void Dispose()
            {
                IsDisposed = true;
            }
        }

        private class MockViewModel<TArguments, TState> : IActivatable, IDisposable
        {
            // *** Properties ***

            public TArguments ActivationArguments { get; private set; }
            public TState ActivationState { get; private set; }
            public bool IsActivated { get; private set; }
            public bool IsDisposed { get; set; }
            public string Name { get; set; }
            public TState State { get; set; }

            // *** Methods ***

            public void Activate(PageInfo pageInfo)
            {
                IsActivated = true;
                ActivationArguments = pageInfo.GetArguments<TArguments>();

                TState state;
                if (pageInfo.TryGetState<TState>("State", out state))
                    State = state;
                else
                    State = default(TState);
            }

            public void SaveState(PageInfo pageInfo)
            {
                pageInfo.SetState<TState>("State", State);
            }

            public void Dispose()
            {
                IsDisposed = true;
            }
        }
    }
}