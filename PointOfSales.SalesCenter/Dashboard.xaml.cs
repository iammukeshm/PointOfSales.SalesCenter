using ModernWpf;
using ModernWpf.Controls;
using PointOfSales.SalesCenter.Common;
using PointOfSales.SalesCenter.DashboardPages;
using PointOfSales.SalesCenter.Presets;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace PointOfSales.SalesCenter
{
    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard : Window
    {
        private const string AutoHideScrollBarsKey = "AutoHideScrollBars";
        public static Dashboard Current { get; private set; }
        public static Frame RootFrame { get; private set; }
        private bool _ignoreSelectionChange;
        private readonly DashboardPagesData _controlPagesData = new DashboardPagesData();
        private string _startPage;
        partial void SetStartPage();
        public Dashboard()
        {
            System.Windows.Application.Current.Resources[AutoHideScrollBarsKey] = true;
            InitializeComponent();
            Current = this;
            RootFrame = rootFrame;
            System.Windows.Application.Current.Resources[AutoHideScrollBarsKey] = true;
            SetStartPage();
            if (!string.IsNullOrEmpty(_startPage))
            {
                PagesList.SelectedItem = PagesList.Items.OfType<DashboardInfoDataItem>().FirstOrDefault(
                    x => x.NavigateUri.ToString().Split('/').Last().Equals(_startPage + ".xaml", StringComparison.OrdinalIgnoreCase));
            }
            NavigateToSelectedPage();

            if (Debugger.IsAttached)
            {
                //DebugMenuItem.Visibility = Visibility.Visible;
            }
        }
        public bool isClosing = false;

        private async void close_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(!isClosing)
            {
                e.Cancel = true;
                var confirmDialog = await Dialog.ConfirmarionContentDialog("Confirm?", "Do you want to Logout?", "Confirm", "Close");
                if (confirmDialog == ContentDialogResult.Primary)
                {
                    isClosing = true;
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                    var myWindow = Window.GetWindow(this);
                    myWindow.Close();
                    this.SwitchTheme(this, ref mainWindow);
                }
                else
                {
                    e.Cancel = true;
                }
            }
            else
            {
                SessionContext.Kill();
            }
            
            
        }
        private void SwitchTheme(Dashboard dashboard, ref MainWindow mainPage)
        {
            ElementTheme newTheme;
            if (ThemeManager.GetActualTheme(dashboard) == ElementTheme.Dark)
            {
                newTheme = ElementTheme.Dark;
            }
            else
            {
                newTheme = ElementTheme.Light;
            }
            ThemeManager.SetRequestedTheme(mainPage, newTheme);
        }
        private void ThemeButton_Click(object sender, RoutedEventArgs e)
        {
            this.ToggleTheme();
        }


        private void NavigateToSelectedPage()
        {
            if (PagesList.SelectedValue is Uri source)
            {
                RootFrame?.Navigate(source);
            }
        }
        private void RootFrame_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.Back)
            {
                RootFrame.RemoveBackEntry();
            }
        }

        private void RootFrame_Navigated(object sender, NavigationEventArgs e)
        {
            Debug.Assert(!RootFrame.CanGoForward);

            _ignoreSelectionChange = true;
            PagesList.SelectedValue = RootFrame.CurrentSource;
            _ignoreSelectionChange = false;
        }

        private void PagesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_ignoreSelectionChange)
            {
                NavigateToSelectedPage();
            }
        }
        private void PresetMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is MenuItem menuItem)
            {
                PresetManager.Current.ColorPreset = (string)menuItem.Header;
            }
        }
    }

    public class DashboardPagesData : List<DashboardInfoDataItem>
    {
        public DashboardPagesData()
        {
            AddPage(typeof(WelcomePage), "Welcome");

        }

        private void AddPage(Type pageType, string displayName = null)
        {
            Add(new DashboardInfoDataItem(pageType, displayName));
        }
    }
    public class DashboardInfoDataItem
    {
        public DashboardInfoDataItem(Type pageType, string title = null)
        {
            Title = title ?? pageType.Name.Replace("Page", null);
            NavigateUri = new Uri($"DashboardPages/{pageType.Name}.xaml", UriKind.Relative);
        }

        public string Title { get; }

        public Uri NavigateUri { get; }

        public override string ToString()
        {
            return Title;
        }
    }
}
