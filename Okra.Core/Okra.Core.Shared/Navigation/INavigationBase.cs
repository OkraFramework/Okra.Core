using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okra.Navigation
{
    public interface INavigationBase
    {
        // *** Events ***

        [Obsolete("Use INavigationStack.PropertyChanged event for future compatibility")]
        event EventHandler CanGoBackChanged;

        [Obsolete("Use INavigationStack.NavigatingFrom event for future compatibility")]
        event EventHandler<PageNavigationEventArgs> NavigatingFrom;

        [Obsolete("Use INavigationStack.NavigatedTo event for future compatibility")]
        event EventHandler<PageNavigationEventArgs> NavigatedTo;

        // *** Properties ***

        [Obsolete("Use INavigationStack.CanGoBack property for future compatibility")]
        bool CanGoBack { get; }
        INavigationStack NavigationStack { get; }

        // *** Methods ***

        bool CanNavigateTo(string pageName);
        IEnumerable<object> GetPageElements(PageInfo page);
    }
}
