using System;
using System.Collections.Generic;
using System.Composition;
using System.Composition.Hosting.Core;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Okra.Helpers;
using Windows.UI.Xaml;

namespace Okra.Navigation
{
    [Export(typeof(IViewFactory))]
    [Shared]
    public class ViewFactory : IViewFactory
    {
        // *** Fields ***

        private readonly ExportFactory<CompositionContext> compositionContextFactory;
        private readonly Lazy<object, PageMetadata>[] lazyPageExports;

        // *** Constructors ***

        [ImportingConstructor]
        public ViewFactory([Import, SharingBoundary("page")]ExportFactory<CompositionContext> compositionContextFactory, [ImportMany("OkraPage")]Lazy<object, PageMetadata>[] lazyPageExports)
        {
            if (compositionContextFactory == null)
                throw new ArgumentNullException("compositionContextFactory");

            if (lazyPageExports == null)
                throw new ArgumentNullException("lazyPageExports");

            this.compositionContextFactory = compositionContextFactory;
            this.lazyPageExports = lazyPageExports;
        }

        // *** Methods ***

        public IViewLifetimeContext CreateView(string name, INavigationContext context)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException(ResourceHelper.GetErrorResource("Exception_ArgumentException_StringIsNullOrEmpty"), "name");

            if (context == null)
                throw new ArgumentNullException("context");

            // Create a new composition context for the page (this allows a sharing boundary to be formed)

            Export<CompositionContext> compositionContextExport = compositionContextFactory.CreateExport();
            CompositionContext compositionContext = compositionContextExport.Value;

            // Since MEF does not support parameterized composition, inject a proxy INavigationContext into this sharing boundary

            NavigationContextProxy contextProxy = compositionContext.GetExport<INavigationContext>() as NavigationContextProxy;
            contextProxy.SetNavigationContext(context);

            // Setup the metadata contstraints (these are shared for both page and view model imports)

            Dictionary<string, object> metadataConstriants = new Dictionary<string, object>();
            metadataConstriants["PageName"] = name;

            // Get the requested page (if no suitable page is found then throw an exception)

            object page;

            if (!compositionContext.TryGetExport(new CompositionContract(typeof(object), "OkraPage", metadataConstriants), out page))
                throw new InvalidOperationException(string.Format(ResourceHelper.GetErrorResource("Exception_InvalidOperation_CannotNavigateAsPageIsNotFound"), name));
            
            // Get the requested view model (if one exists)

            object viewModel;

            if (compositionContext.TryGetExport(new CompositionContract(typeof(object), "OkraViewModel", metadataConstriants), out viewModel))
            {
                // Attach the view-model to the page
                // NB: Do this via a virtual method call to help with unit testing

                AttachViewModel(page, viewModel);
            }

            // Return a new IViewLifetimeContext

            return new ViewLifetimeContext(compositionContextExport, page, viewModel);
        }

        public bool IsViewDefined(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException(ResourceHelper.GetErrorResource("Exception_ArgumentException_StringIsNullOrEmpty"), "name");

            // Check the list of page exports for the specified page name
            // NB: Since these are lazy exports they will never be created.

            Lazy<object, PageMetadata> lazyPage = lazyPageExports.FirstOrDefault(p => p.Metadata.PageName == name);
            return lazyPage != null;
        }

        // *** Protected Methods ***

        protected virtual void AttachViewModel(object page, object viewModel)
        {
            if (page == null)
                throw new ArgumentNullException("page");

            if (viewModel == null)
                throw new ArgumentNullException("viewModel");

            if (page is FrameworkElement)
                ((FrameworkElement)page).DataContext = viewModel;
        }

        // *** Private Sub-Classes ***

        private sealed class ViewLifetimeContext : IViewLifetimeContext
        {
            // *** Fields ***

            private readonly Export<CompositionContext> compositionContextExport;
            private readonly object page;
            private readonly object viewModel;

            // *** Constructors ***

            public ViewLifetimeContext(Export<CompositionContext> compositionContextExport, object page, object viewModel)
            {
                this.compositionContextExport = compositionContextExport;
                this.page = page;
                this.viewModel = viewModel;
            }

            // *** Properties ***

            public object View
            {
                get
                {
                    return page;
                }
            }

            public object ViewModel
            {
                get
                {
                    return viewModel;
                }
            }

            // *** IDisposable Implementation ***

            public void Dispose()
            {
                // NB: No need for a more complex Dispose implementation as this class is sealed

                compositionContextExport.Dispose();
            }
        }
    }
}
