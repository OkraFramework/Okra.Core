using Okra.Navigation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Okra.Tests.Mocks
{
    public class MockViewFactory : IViewFactory
    {
        // *** Fields ***

        private readonly Func<string, INavigationContext, IViewLifetimeContext> viewLifetimeContextFactory;

        // *** Constructors ***

        public MockViewFactory(Func<string, INavigationContext, IViewLifetimeContext> viewLifetimeContextFactory)
        {
            this.viewLifetimeContextFactory = viewLifetimeContextFactory;
        }

        // *** Methods ***

        public IViewLifetimeContext CreateView(string name, INavigationContext context)
        {
            IViewLifetimeContext view = viewLifetimeContextFactory(name, null);

            if (view == null)
                throw new InvalidOperationException();

            return viewLifetimeContextFactory(name, context);
        }

        public bool IsViewDefined(string name)
        {
            IViewLifetimeContext view = viewLifetimeContextFactory(name, null);
            return view != null;
        }

        // *** Static Properties ***

        public static MockViewFactory WithPageAndViewModel
        {
            get
            {
                return new MockViewFactory((name, context) => new MockViewLifetimeContext(name, "ViewModel", context));
            }
        }

        public static MockViewFactory WithPageOnly
        {
            get
            {
                return new MockViewFactory((name, context) => new MockViewLifetimeContext(name, null, context));
            }
        }

        public static IViewFactory WithNavigationAware
        {
            get
            {
                return new MockViewFactory((name, context) => new MockViewLifetimeContext(name, "View Model", context, pageType: typeof(MockPage_NavigationAware), viewModelType: typeof(MockViewModel_NavigationAware)));
            }
        }

        public static IViewFactory WithActivatable
        {
            get
            {
                return new MockViewFactory((name, context) => new MockViewLifetimeContext(name, "View Model", context, pageType: typeof(MockPage), viewModelType: typeof(MockViewModel_Activatable)));
            }
        }

        public static IViewFactory NoPageDefined
        {
            get
            {
                return new MockViewFactory((name, context) => null);
            }
        }
    }
}
