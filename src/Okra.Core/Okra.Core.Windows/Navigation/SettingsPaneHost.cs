using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Okra.Navigation
{
    internal class SettingsPaneHost : SettingsFlyout
    {
        // *** Fields ***

        private ControlTemplate _chromelessTemplate;

        // *** Events ***

        public new BackClickEventHandler BackClick;

        // *** Constructors ***

        public SettingsPaneHost()
        {
            string chromelessXaml = "<ControlTemplate xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\"><ContentPresenter/></ControlTemplate>";
            _chromelessTemplate = (ControlTemplate)Windows.UI.Xaml.Markup.XamlReader.Load(chromelessXaml);

            base.BackClick += SettingsPaneHost_BackClick;
        }

        // *** Overriden base methods ***

        protected override void OnContentChanged(object oldContent, object newContent)
        {
            base.OnContentChanged(oldContent, newContent);

            // If the old content is a SettingsFlyout then detach from the BackClick event

            if (oldContent is SettingsFlyout)
            {
                ((SettingsFlyout)oldContent).BackClick -= SettingsPaneHost_BackClick;
            }

            // If the content is a SettingsFlyout then remove the host chrome and attach to the BackClick event
            // Otherwise use the default SettingsFlyout chrome

            if (newContent is SettingsFlyout)
            {
                SettingsFlyout newContentFlyout = (SettingsFlyout)newContent;

                this.Template = _chromelessTemplate;
                this.Width = newContentFlyout.Width;

                newContentFlyout.BackClick += SettingsPaneHost_BackClick;
            }
            else
            {
                this.ClearValue(TemplateProperty);

                // Style the settings flyout based upon properties specified by the page

                if (newContent is DependencyObject)
                {
                    DependencyObject contentObject = (DependencyObject)newContent;

                    this.Title = SettingsPaneInfo.GetTitle(contentObject);
                    this.Width = SettingsPaneInfo.GetWidth(contentObject);
                    this.IconSource = SettingsPaneInfo.GetIconSource(contentObject);

                    Brush headerBackground = SettingsPaneInfo.GetHeaderBackground(contentObject);

                    if (headerBackground != null)
                        this.HeaderBackground = headerBackground;
                    else
                        this.ClearValue(SettingsFlyout.HeaderBackgroundProperty);

                    Brush headerForeground = SettingsPaneInfo.GetHeaderForeground(contentObject);

                    if (headerForeground != null)
                        this.HeaderForeground = headerForeground;
                    else
                        this.ClearValue(SettingsFlyout.HeaderForegroundProperty);
                }
            }
        }

        private void SettingsPaneHost_BackClick(object sender, BackClickEventArgs e) => OnBackClick(e);

        // *** Protected Methods ***

        protected virtual void OnBackClick(BackClickEventArgs e) => BackClick?.Invoke(this, e);
    }
}
