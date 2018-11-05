using System;
using System.Collections.Generic;

namespace Shop.Models
{
    public class Cart
    {
        public List<Product> Products;
        public decimal Sum;

        public Cart()
        {
        }
    }
}
