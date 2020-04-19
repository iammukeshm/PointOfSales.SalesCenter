using CoreLibrary.Exceptions;
using PointOfSales.SalesCenter.Application.DTOs;
using PointOfSales.SalesCenter.Application.Models.Product;
using PointOfSales.SalesCenter.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
    /// Interaction logic for ProductListComponent.xaml
    /// </summary>
    public partial class ProductListComponent
    {
        public List<ProductViewModel> product = new List<ProductViewModel>();

        public ObservableCollection<ProductViewModel> productFiltered = new ObservableCollection<ProductViewModel>();

        public delegate void ChildDelegate(ProductViewModel data);

        public event ChildDelegate GetProduct;
        public ProductListComponent()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadProductDataAsync();
        }
        private async void LoadProductDataAsync()
        {

            try
            {
                var api = SessionContext.ApiHelper;
                var result = await api.GetAsync<Result<List<ProductViewModel>>>(APIEndpoints.GetAllProduct);

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
                        await Dialog.UserNotAuthorizedToUseApiEndpointDialog(APIEndpoints.GetAllPeeople);
                    });

                }
            }
        }
        private void PopulateDataToUI(List<ProductViewModel> data)
        {
            this.Visibility = Visibility.Visible;
            this.product = data;
            this.productFiltered = new ObservableCollection<ProductViewModel>(this.product);
            this.ProductListView.ItemsSource = this.productFiltered;

        }
        private void OnFilterChanged(object sender, TextChangedEventArgs e)
        {
            // Linq query that selects only items that return True after being passed through Filter function
            var filtered = product.Where(product => Filter(product));
            Remove_NonMatching(filtered);
            AddBack_Contacts(filtered);
        }
        private bool Filter(ProductViewModel product)
        {
            // When the text in any filter is changed, contact list is ran through all three filters to make sure
            // they can properly interact with each other (i.e. they can all be applied at the same time).

            return product.Name.IndexOf(FilterText.Text, StringComparison.InvariantCultureIgnoreCase) > -1 ||
                   product.Description.IndexOf(FilterText.Text, StringComparison.InvariantCultureIgnoreCase) > -1 ||
                   product.Barcode.IndexOf(FilterText.Text, StringComparison.InvariantCultureIgnoreCase) > -1;
        }
        private void Remove_NonMatching(IEnumerable<ProductViewModel> filteredData)
        {
            for (int i = productFiltered.Count - 1; i >= 0; i--)
            {
                var item = productFiltered[i];
                // If contact is not in the filtered argument list, remove it from the ListView's source.
                if (!filteredData.Contains(item))
                {
                    productFiltered.Remove(item);
                }
            }
        }

        private void AddBack_Contacts(IEnumerable<ProductViewModel> filteredData)
        // When a user hits backspace, more contacts may need to be added back into the list
        {
            foreach (var item in filteredData)
            {
                // If item in filtered list is not currently in ListView's source collection, add it back in
                if (!productFiltered.Contains(item))
                {
                    productFiltered.Add(item);
                }
            }
        }

        private void ProductListView_Selected(object sender, RoutedEventArgs e)
        {
            var item = this.ProductListView.SelectedItem;
        }

        private void ProductListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = (ProductViewModel)this.ProductListView.SelectedItem;
            GetProduct(item);
        }
    }
}
