using System.Collections.Generic;

namespace PointOfSales.SalesCenter.Application.Models.Cart
{
    public class InvoiceModel
    {
        public List<InvoiceDetailModel> InvoiceDetails { get; set; }
        public int CustomerId { get; set; }
    }
}
