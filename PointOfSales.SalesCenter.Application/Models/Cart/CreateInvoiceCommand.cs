using System;
using System.Collections.Generic;
using System.Text;

namespace PointOfSales.SalesCenter.Application.Models.Cart
{
    public class CreateInvoiceCommand
    {
        public InvoiceModel invoice { get; set; }
    }
}
