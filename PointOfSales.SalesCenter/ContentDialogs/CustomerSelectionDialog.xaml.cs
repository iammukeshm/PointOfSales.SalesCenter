using ModernWpf;
using PointOfSales.SalesCenter.Application.DTOs;
using PointOfSales.SalesCenter.Application.Models.Person;
using PointOfSales.SalesCenter.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PointOfSales.SalesCenter.ContentDialogs
{
    /// <summary>
    /// Interaction logic for CustomerSelectionDialog.xaml
    /// </summary>
    public partial class CustomerSelectionDialog
    {
        public List<PersonViewModel> people = new List<PersonViewModel>();

        public ObservableCollection<PersonViewModel> peopleFiltered = new ObservableCollection<PersonViewModel>();
        public CustomerSelectionDialog()
        {
            
            InitializeComponent();
        }

        private async void ContentDialog_Opened(ModernWpf.Controls.ContentDialog sender, ModernWpf.Controls.ContentDialogOpenedEventArgs args)
        {
            
        }

        private void OnFilterChanged(object sender, TextChangedEventArgs e)
        {
            // Linq query that selects only items that return True after being passed through Filter function
            var filtered = people.Where(people => Filter(people));
            Remove_NonMatching(filtered);
            AddBack_Contacts(filtered);
        }
        private bool Filter(PersonViewModel people)
        {
            // When the text in any filter is changed, contact list is ran through all three filters to make sure
            // they can properly interact with each other (i.e. they can all be applied at the same time).

            return people.FirstName.IndexOf(FilterText.Text, StringComparison.InvariantCultureIgnoreCase) > -1 ||
                   people.LastName.IndexOf(FilterText.Text, StringComparison.InvariantCultureIgnoreCase) > -1 ||
                   people.Name.IndexOf(FilterText.Text, StringComparison.InvariantCultureIgnoreCase) > -1 ||
                   people.MobileNumber.IndexOf(FilterText.Text, StringComparison.InvariantCultureIgnoreCase) > -1 ||
                   people.Email.IndexOf(FilterText.Text, StringComparison.InvariantCultureIgnoreCase) > -1 ||
                   people.PhoneNumber.IndexOf(FilterText.Text, StringComparison.InvariantCultureIgnoreCase) > -1;
        }
        private void Remove_NonMatching(IEnumerable<PersonViewModel> filteredData)
        {
            for (int i = peopleFiltered.Count - 1; i >= 0; i--)
            {
                var item = peopleFiltered[i];
                // If contact is not in the filtered argument list, remove it from the ListView's source.
                if (!filteredData.Contains(item))
                {
                    peopleFiltered.Remove(item);
                }
            }
        }

        private void AddBack_Contacts(IEnumerable<PersonViewModel> filteredData)
        // When a user hits backspace, more contacts may need to be added back into the list
        {
            foreach (var item in filteredData)
            {
                // If item in filtered list is not currently in ListView's source collection, add it back in
                if (!peopleFiltered.Contains(item))
                {
                    peopleFiltered.Add(item);
                }
            }
        }

        private async void ContentDialog_PrimaryButtonClick(ModernWpf.Controls.ContentDialog sender, ModernWpf.Controls.ContentDialogButtonClickEventArgs args)
        {
            if(FilteredListView.SelectedItem==null)
            {
                args.Cancel = true;
                ErrorText.Text = "Select a Customer to Proceed.";
                ErrorText.Visibility = Visibility.Visible;
            }
           else
            {
                var selectedCustomer = (PersonViewModel)FilteredListView.SelectedItem;
                this.Tag = selectedCustomer;
            }
            
        }
    }
}
