using CoreLibrary.Exceptions;
using ModernWpf.Controls;
using PointOfSales.SalesCenter.Application.DTOs;
using PointOfSales.SalesCenter.Application.Models.Person;
using PointOfSales.SalesCenter.Common;
using PointOfSales.SalesCenter.ContentDialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading;
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
    /// Interaction logic for CustomerComponent.xaml
    /// </summary>
    public partial class CustomerComponent : UserControl
    {
        public CustomerSelectionDialog _customerDialog = new CustomerSelectionDialog();
        public delegate void ChildDelegate(int customer);
        public event ChildDelegate GetDataFromChild;

        public CustomerComponent()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            _customerDialog = new CustomerSelectionDialog
            {
                Title = ((Button)sender).Content,
                PrimaryButtonText = "Select",
                CloseButtonText = "Cancel",
                IsShadowEnabled = false
            };
            Thread loadData = new Thread(LoadDataAsync);
            loadData.Start();
            _customerDialog.Visibility = Visibility.Hidden;
            ContentDialogResult contentResult = await _customerDialog.ShowAsync();
            if (contentResult == ContentDialogResult.Primary)
            {
                var selectedCustomer = (PersonViewModel)_customerDialog.Tag;
                personName.Text = selectedCustomer.Name;
                personPicture.Initials = selectedCustomer.Initials;
                personContact.Text = $"{selectedCustomer.MobileNumber} | {selectedCustomer.PhoneNumber}";
                personEmail.Text = selectedCustomer.Email;
                GetDataFromChild(selectedCustomer.Id);
                customerDetails.Visibility = Visibility.Visible;
            }
        }
        private async void LoadDataAsync()
        {

            try
            {
                var api = SessionContext.ApiHelper;
                var result = await api.GetAsync<Result<List<PersonViewModel>>>(APIEndpoints.GetAllPeeople);

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
                        _customerDialog.Hide();
                        await Dialog.UserNotAuthorizedToUseApiEndpointDialog(APIEndpoints.GetAllPeeople);
                    });

                }
            }
        }
        private void PopulateDataToUI(List<PersonViewModel> data)
        {
            _customerDialog.Visibility = Visibility.Visible;
            _customerDialog.people = data;
            _customerDialog.peopleFiltered = new ObservableCollection<PersonViewModel>(_customerDialog.people);
            _customerDialog.FilteredListView.ItemsSource = _customerDialog.peopleFiltered;

        }
    }
}
