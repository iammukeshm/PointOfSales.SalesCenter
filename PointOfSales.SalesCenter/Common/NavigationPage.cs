using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace PointOfSales.SalesCenter.Common
{
    public class NavigationPage : Page
    {
        protected virtual void OnNavigatedTo(NavigationEventArgs e) { }
        protected virtual void OnNavigatedFrom(NavigationEventArgs e) { }
        protected virtual void OnNavigatingFrom(NavigatingCancelEventArgs e) { }

        internal void InternalOnNavigatedTo(NavigationEventArgs e) => OnNavigatedTo(e);
        internal void InternalOnNavigatedFrom(NavigationEventArgs e) => OnNavigatedFrom(e);
        internal void InternalOnNavigatingFrom(NavigatingCancelEventArgs e) => OnNavigatingFrom(e);
    }
}
