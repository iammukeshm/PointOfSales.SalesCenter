using ModernWpf.Controls;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Windows.Threading;

namespace PointOfSales.SalesCenter.Common
{
    public class NavigationFrame : TransitionFrame
    {
        private object _oldContent;

        public NavigationFrame()
        {
            Navigating += OnNavigating;
            Navigated += OnNavigated;
        }

        protected override void OnContentChanged(object oldContent, object newContent)
        {
            base.OnContentChanged(oldContent, newContent);
            _oldContent = oldContent;
        }

        private void OnNavigating(object sender, NavigatingCancelEventArgs e)
        {
            (Content as NavigationPage)?.InternalOnNavigatingFrom(e);
        }

        private void OnNavigated(object sender, NavigationEventArgs e)
        {
            bool firstNavigation = _oldContent == null;

            if (_oldContent != null)
            {
                (_oldContent as NavigationPage)?.InternalOnNavigatedFrom(e);
                _oldContent = null;
            }

            (e.Content as NavigationPage)?.InternalOnNavigatedTo(e);

            if (!firstNavigation && e.Content is UIElement element)
            {
                Dispatcher.BeginInvoke(() =>
                {
                    element.MoveFocus(new TraversalRequest(FocusNavigationDirection.First));
                }, DispatcherPriority.Loaded);
            }
        }
    }
}
