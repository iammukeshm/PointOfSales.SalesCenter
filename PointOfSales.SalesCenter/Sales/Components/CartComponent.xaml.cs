using ModernWpf.Controls;
using PointOfSales.SalesCenter.Application.Models.Cart;
using PointOfSales.SalesCenter.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PointOfSales.SalesCenter.Sales.Components
{
    /// <summary>
    /// Interaction logic for CartComponent.xaml
    /// </summary>
    public partial class CartComponent : UserControl
    {
        public List<CartDetailModel> cart = new List<CartDetailModel>();

        public ObservableCollection<CartDetailModel> cartFiltered = new ObservableCollection<CartDetailModel>();

        public delegate void ChildDelegate();

        public event ChildDelegate EmptyCart;
        public CartComponent()
        {
            InitializeComponent();
        }

        private void OnFilterChanged(object sender, TextChangedEventArgs e)
        {

        }

        private async void clearCartButton_Click(object sender, RoutedEventArgs e)
        {
            var confirmDialog = await Dialog.ConfirmarionContentDialog("Confirm?", "Do you want to clear the cart?", "Confirm", "Close");
            if (confirmDialog == ContentDialogResult.Primary)
            {
                EmptyCart();
            }
            
        }
    }
}
