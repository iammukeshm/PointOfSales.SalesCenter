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
        public DC context = new DC();
        public SalesCenter()
        {
            InitializeComponent();
            customerComponent.GetDataFromChild += new CustomerComponent.ChildDelegate(UpdateDataFromCustomerComponent);
            productListComponent.GetProduct += new ProductListComponent.ChildDelegate(UpdateCart);
            cartComponent.EmptyCart += new CartComponent.ChildDelegate(EmptyCart);
            productGroupComponent.FilterByProductGroup += new ProductGroupListElement.ChildDelegate(FilterByProductGroup);
            cartComponent.CartListView.ItemsSource = context.CartItems;
            UpdateTotals(context.CartItems.Sum(a=>a.Total));
        }

        private void FilterByProductGroup(ProductGroupViewModel data)
        {
            if(data == null)
            {
                this.productListComponent.ProductListView.ItemsSource = this.productListComponent.productFiltered;
                this.productListComponent.productCaption.Text = "Products";
            }
            else
            {
                this.productListComponent.productFiltered = new ObservableCollection<ProductViewModel>(this.productListComponent.product.Where(a => a.ProductGroupId == data.Id));
                this.productListComponent.productCaption.Text = $"Products : {data.Name}";
                this.productListComponent.ProductListView.ItemsSource = this.productListComponent.productFiltered; ;
            }
            
        }

        private void EmptyCart()
        {
            this.context.CartItems = new ObservableCollection<InvoiceDetailModel>();
            cartComponent.CartListView.ItemsSource = context.CartItems;
            UpdateTotals(0);
        }
        private void UpdateTotals(decimal subTotal)
        {
            var totalTax = Math.Round(subTotal * GetTax(), GetRoundFactor());
            var total = Math.Round(subTotal * GetTax() + context.CartItems.Sum(a => a.Total), GetRoundFactor());
            totalComponent.subTotalTextBox.Text = $"{subTotal}$";
            totalComponent.totalTaxTextBox.Text = $"{totalTax}$";
            totalComponent.totalTextBox.Text = $"{total}$";

        }
        private void UpdateCart(ProductViewModel data)
        {

            if (context.CartItems.Any(a => a.ProductId == data.Id))
            {
                //Item Exists


                foreach (InvoiceDetailModel item in context.CartItems)
                {
                    if (item.ProductId == data.Id)
                    {
                        item.QuantityInCart++;
                        item.Total = item.QuantityInCart * item.Rate;
                        item.OnPropertyChange("QuantityInCart");
                        item.OnPropertyChange("Total");
                    }
                }
                UpdateTotals(context.CartItems.Sum(a => a.Total));

            }
            else
            {
                var cartItem = new InvoiceDetailModel
                {
                    ProductId = data.Id,
                    QuantityInCart = 1,
                    Name = data.Name,
                    Rate = data.Rate,
                    Barcode = data.Barcode,
                    Total = data.Rate

                };
                UpdateTotals(cartItem.Total);
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
            public ObservableCollection<InvoiceDetailModel> CartItems { get; set; } = new ObservableCollection<InvoiceDetailModel>();
        }

        public decimal GetTax()
        {
            return 0.14m;
        }
        public int GetRoundFactor()
        {
            return 2;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void ThemeButton_Click(object sender, RoutedEventArgs e)
        {
            this.ToggleTheme();
        }

        private async void CheckoutButton_Click(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrEmpty(this.context.CustomerID))
            {
                await Dialog.InformationDialog("Not Allowed!", "Select a customer to continue!");
            }
            else if (this.context.CartItems.Count == 0)
            {
                await Dialog.InformationDialog("Not Allowed!", "Cart Empty!");
            }
            else
            {
                var confirmDialog = await Dialog.ConfirmarionContentDialog("Confirm?", "Do you want to confirm cart and checkout?", "Confirm", "Close");
                if (confirmDialog == ContentDialogResult.Primary)
                {
                    Checkout();
                }
            }
           
        }

        private async void Checkout()
        {
            try
            {
                //Disable Buttons
                ToggleButtons();
                var cart = new InvoiceModel();
                cart.InvoiceDetails = this.context.CartItems.ToList();
                cart.CustomerId = Convert.ToInt32(this.context.CustomerID);
                var command = new CreateInvoiceCommand();
                command.invoice = cart;
                var api = SessionContext.ApiHelper;
                var result = await api.PostAsync<Result<int>>(APIEndpoints.Invoice, command);
                if (result.Succeeded)
                {
                    await Dialog.InformationDialog("Success", "Invoice Added!");
                    ResetSalesCenter();
                }
            }
            catch (Exception ex)
            {

                //throw;
            }
            
        }

        private void ToggleButtons()
        {
            this.customerComponent.chooseButton.IsEnabled = this.customerComponent.chooseButton.IsEnabled ? false : true;
            this.CheckoutButton.IsEnabled = this.CheckoutButton.IsEnabled ? false : true;
        }

        private void ResetSalesCenter()
        {
            this.context = new DC();
            this.cartComponent.cart = new List<InvoiceDetailModel>();
            this.cartComponent.cartFiltered = new ObservableCollection<InvoiceDetailModel>();
            this.cartComponent.CartListView.ItemsSource = this.cartComponent.cartFiltered;
            this.EmptyCart();
            this.customerComponent.ResetCustomer();
            this.customerComponent.customerDetails.Visibility = Visibility.Collapsed;
            ToggleButtons();
            //throw new NotImplementedException();
        }
    }
}
