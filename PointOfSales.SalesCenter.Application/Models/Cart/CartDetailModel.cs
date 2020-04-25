using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace PointOfSales.SalesCenter.Application.Models.Cart
{
    public class InvoiceDetailModel : INotifyPropertyChanged
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public decimal Rate { get; set; }
        public string Barcode { get; set; }
        public decimal Total { get; set; }
        public int QuantityInCart { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChange([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
