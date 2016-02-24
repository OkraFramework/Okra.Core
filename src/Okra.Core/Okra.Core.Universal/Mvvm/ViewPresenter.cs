using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Okra.Mvvm
{
    public class ViewPresenter : Frame
    {
        public static readonly DependencyProperty CurrentViewProperty =
            DependencyProperty.Register("CurrentView", typeof(object), typeof(ViewPresenter), new PropertyMetadata(null, CurrentViewChanged));
        
        public ViewPresenter()
        {
        }

        public object CurrentView
        {
            get { return (object)GetValue(CurrentViewProperty); }
            set { SetValue(CurrentViewProperty, value); }
        }

        private static void CurrentViewChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ViewPresenter)d).CurrentViewChanged(e.OldValue, e.NewValue);
        }

        public void CurrentViewChanged(object oldView, object newView)
        {
            this.Navigate(typeof(ViewHost), newView);
        }
    }
}
