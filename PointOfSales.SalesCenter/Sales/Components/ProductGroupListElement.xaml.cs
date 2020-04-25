using CoreLibrary.Exceptions;
using PointOfSales.SalesCenter.Application.DTOs;
using PointOfSales.SalesCenter.Application.Models.Product;
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
    /// Interaction logic for ProductGroupListElement.xaml
    /// </summary>
    public partial class ProductGroupListElement : UserControl
    {
        public List<ProductGroupViewModel> productGroup = new List<ProductGroupViewModel>();

        public ObservableCollection<ProductGroupViewModel> productGroupFiltered = new ObservableCollection<ProductGroupViewModel>();

        public delegate void ChildDelegate(ProductGroupViewModel data);

        public event ChildDelegate FilterByProductGroup;
        public ProductGroupListElement()
        {
            InitializeComponent();
        }
        private async void LoadProductGroupDataAsync()
        {
            try
            {
                var api = SessionContext.ApiHelper;
                var result = await api.GetAsync<Result<List<ProductGroupViewModel>>>(APIEndpoints.ProductGroup);

                //Since Data Load and UI are on different threads, data thread cannot access elements of UI.
                //So we access the Dispacther (Main Thread / UI) and Invoke method async to it.
                this.Dispatcher.Invoke(() =>
                {
                    PopulateDataToUI(result.Data);
                });
            }
            catch (Exception ex)
            {
                if (ex.Message == ExceptionEnum.NotAuthorized.ToString())
                {
                    await this.Dispatcher.InvokeAsync(async () =>
                    {
                        await Dialog.UserNotAuthorizedToUseApiEndpointDialog(APIEndpoints.ProductGroup);
                    });

                }
            }
        }
        private void PopulateDataToUI(List<ProductGroupViewModel> data)
        {
            this.Visibility = Visibility.Visible;
            this.productGroup = data;
            this.productGroupFiltered = new ObservableCollection<ProductGroupViewModel>(this.productGroup);
            this.ProductGroupListView.ItemsSource = this.productGroupFiltered;

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadProductGroupDataAsync();
        }

        private void ProductGroupListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = (ProductGroupViewModel)this.ProductGroupListView.SelectedItem;
            //Refil the ItemSource
            FilterByProductGroup(item);



        }

        private void clearGroupButton_Click(object sender, RoutedEventArgs e)
        {

            this.ProductGroupListView.SelectedIndex = -1;
            FilterByProductGroup(null);
        }
    }
}
