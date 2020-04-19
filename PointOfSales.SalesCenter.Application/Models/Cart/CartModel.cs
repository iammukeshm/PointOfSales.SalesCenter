using System;
using System.Collections.Generic;
using System.Text;

namespace PointOfSales.SalesCenter.Application.Models.Cart
{
    public class CartModel
    {
        public List<CartDetailModel> CartDetails { get; set; }
        public int CustomerId { get; set; }
    }
}
