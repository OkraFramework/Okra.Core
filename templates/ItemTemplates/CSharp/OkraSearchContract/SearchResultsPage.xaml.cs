using Okra.Navigation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Search Contract item template is documented at http://go.microsoft.com/fwlink/?LinkId=234240

namespace $rootnamespace$
{
    $wizardcomment$/// <summary>
    /// This page displays search results when a global search is directed to this application.
    /// </summary>
    [PageExport(SpecialPageNames.Search)]
    public sealed partial class $safeitemname$ : $safeprojectname$.Common.LayoutAwarePage
    {
        public $safeitemname$()
        {
            this.InitializeComponent();
            this.Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            // HACK: Since we do not have behaviours then bind the VisualStates to the view model in code-behind

            if (DataContext is $safeitemname$ViewModel)
            {
                (($safeitemname$ViewModel)DataContext).PropertyChanged += ViewModel_PropertyChanged;
                SetResultState();
            }
        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ShowResults")
                SetResultState();
        }

        private void SetResultState()
        {
            bool showResults = (($safeitemname$ViewModel)DataContext).ShowResults;
            string stateName = showResults ? "ResultsFound" : "NoResultsFound";
            VisualStateManager.GoToState(this, stateName, true);
        }
    }
}
