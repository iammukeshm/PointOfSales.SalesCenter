using System;
using System.Collections.Generic;
using System.Text;

namespace PointOfSales.SalesCenter.Application.Models.Product
{
    public class ProductViewModel 
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Barcode { get; set; }
        public bool IsActive { get; set; } = true;
        public string Description { get; set; }

        public int ProductGroupId { get; set; }
        public decimal Rate { get; set; }

    }
}
