using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Okra.Navigation
{
    public class WindowNavigationTarget : INavigationTarget
    {
        // *** Fields ***

        private ContentControl contentHost;

        // *** Methods ***

        public void NavigateTo(object page, INavigationBase navigationManager)
        {
            SetWindowContent(page);
        }

        // *** Protected Methods ***

        protected virtual void SetWindowContent(object page)
        {
            // If the content host has not been created then create this

            if (contentHost == null)
            {
                contentHost = new ContentControl()
                {
                    HorizontalContentAlignment = HorizontalAlignment.Stretch,
                    VerticalContentAlignment = VerticalAlignment.Stretch
                };
            }

            // Ensure that the window content is set to the content host

            if (Window.Current.Content != contentHost)
                Window.Current.Content = contentHost;

            // Set the content to display

            contentHost.Content = page;
        }
    }
}
