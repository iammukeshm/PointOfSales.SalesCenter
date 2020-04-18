using ModernWpf;
using ModernWpf.Controls;
using PointOfSales.SalesCenter.Common;
using PointOfSales.SalesCenter.ContentDialogs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PointOfSales.SalesCenter
{
    /// <summary>
    /// Interaction logic for SalesCenter.xaml
    /// </summary>
    public partial class SalesCenter : Window
    {
        public ContentDialog _dialog = new CustomerSelectionDialog();
        public SalesCenter()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            _dialog = new CustomerSelectionDialog
            {
                Title = ((Button)sender).Content,
                PrimaryButtonText = "Select",
                CloseButtonText = "Cancel",
                IsShadowEnabled = false
            };
            ContentDialogResult contentResult = await _dialog.ShowAsync();
            if(contentResult == ContentDialogResult.Primary)
            {
                var selectedCustomerId = _dialog.Tag.ToString();
                await Dialog.InformationDialogResult("Success!", $"You Have Selected Customer {selectedCustomerId}");
            }
        }
    }
}
