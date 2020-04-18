using CoreLibrary.Helpers;
using ModernWpf.Controls;
using PointOfSales.SalesCenter.Application.DTOs;
using PointOfSales.SalesCenter.Application.Models.Account;
using PointOfSales.SalesCenter.Common;
using PointOfSales.SalesCenter.ControlPages;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public static MainWindow Current { get; private set; }
        public static Frame RootFrame { get; private set; }
        private bool _ignoreSelectionChange;
        private readonly ControlPagesData _controlPagesData = new ControlPagesData();
        private string _startPage;
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new DC();
            Current = this;
            RootFrame = rootFrame;

            SetStartPage();
            if (!string.IsNullOrEmpty(_startPage))
            {
                PagesList.SelectedItem = PagesList.Items.OfType<ControlInfoDataItem>().FirstOrDefault(
                    x => x.NavigateUri.ToString().Split('/').Last().Equals(_startPage + ".xaml", StringComparison.OrdinalIgnoreCase));
            }
            NavigateToSelectedPage();

            if (Debugger.IsAttached)
            {
                //DebugMenuItem.Visibility = Visibility.Visible;
            }
        }
        public class DC
        {
            public string appTitle { get; set; }

            public DC()
            {
                appTitle = $"{ApplicationSettings.Name}";
            }
        }
        partial void SetStartPage();
        private void ToggleButton_Click(object sender, RoutedEventArgs e)
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
    }
    public class ControlPagesData : List<ControlInfoDataItem>
    {
        public ControlPagesData()
        {
            AddPage(typeof(LoginPage), "Login");
           
        }

        private void AddPage(Type pageType, string displayName = null)
        {
            Add(new ControlInfoDataItem(pageType, displayName));
        }
    }
    public class ControlInfoDataItem
    {
        public ControlInfoDataItem(Type pageType, string title = null)
        {
            Title = title ?? pageType.Name.Replace("Page", null);
            NavigateUri = new Uri($"ControlPages/{pageType.Name}.xaml", UriKind.Relative);
        }

        public string Title { get; }

        public Uri NavigateUri { get; }

        public override string ToString()
        {
            return Title;
        }
    }
}
