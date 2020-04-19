using CoreLibrary.Exceptions;
using ModernWpf.Controls;
using PointOfSales.SalesCenter.Application.DTOs;
using PointOfSales.SalesCenter.Application.Models.Cart;
using PointOfSales.SalesCenter.Application.Models.Person;
using PointOfSales.SalesCenter.Application.Models.Product;
using PointOfSales.SalesCenter.Common;
using PointOfSales.SalesCenter.ContentDialogs;
using PointOfSales.SalesCenter.Enums;
using PointOfSales.SalesCenter.Sales.Components;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace PointOfSales.SalesCenter
{
    /// <summary>
    /// Interaction logic for SalesCenter.xaml
    /// </summary>
    public partial class SalesCenter : Window
    {
        public CartModel cart = new CartModel();
        public DC context = new DC();
        public ProductListComponent _productComponent = new ProductListComponent();
        public SalesCenter()
        {
            InitializeComponent();
            customerComponent.GetDataFromChild += new CustomerComponent.ChildDelegate(UpdateDataFromCustomerComponent);
            productListComponent.GetProduct += new ProductListComponent.ChildDelegate(UpdateCart);
            cartComponent.CartListView.ItemsSource = context.CartItems;
        }

        private void UpdateCart(ProductViewModel data)
        {
           
            if(context.CartItems.Any(a=>a.ProductId == data.Id))
            {
                //Item Exists

                
                foreach(CartDetailModel item in context.CartItems)
                {
                    if(item.ProductId == data.Id)
                    {
                        item.QuantityInCart++;
                        item.OnPropertyChange("QuantityInCart");
                    }
                }
            }
            else
            {
                var cartItem = new CartDetailModel
                {
                    ProductId = data.Id,
                    QuantityInCart = 1,
                    Name = data.Name,
                    Rate = data.RetailPrice,
                    Barcode = data.Barcode

                };
                context.CartItems.Add(cartItem);
            }
           
            
        }

        private void UpdateDataFromCustomerComponent(int customer)
        {
            context.CustomerID = customer.ToString();
        }

        public class DC
        {
            public string CustomerID { get; set; }
            //TODO : Change to CartModel to track Quantities
            public ObservableCollection<CartDetailModel> CartItems { get; set; } = new ObservableCollection<CartDetailModel>();
        }

       

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void ThemeButton_Click(object sender, RoutedEventArgs e)
        {
            this.ToggleTheme();
        }
    }
}
